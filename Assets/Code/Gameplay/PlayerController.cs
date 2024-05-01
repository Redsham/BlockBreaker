using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Gameplay
{
	public class PlayerController : MonoBehaviour
	{
		public UnityEvent OnTap = new();
		public Vector2 TapScreenPosition { get; private set; }
		
		public Camera Camera { get; private set; }
		public Transform Transform { get; private set; }
		public Vector3 Origin
		{
			get => m_Origin;
			set
			{
				m_Origin = value;
				m_Offset = Vector3.zero;
				m_Bounds.center = m_Origin;
			}
		}
		public Vector3 Offset
		{
			get => m_Offset;
			set
			{
				m_Offset = value;
				m_Offset.x = Mathf.Clamp(m_Offset.x, -m_Bounds.extents.x, m_Bounds.extents.x);
				m_Offset.y = Mathf.Clamp(m_Offset.y, -m_Bounds.extents.y, m_Bounds.extents.y);
				m_Offset.z = Mathf.Clamp(m_Offset.z, -m_Bounds.extents.z, m_Bounds.extents.z);
			}
		}
		public Bounds Bounds
		{
			get => m_Bounds;
			set
			{
				m_Bounds = value;
				m_Origin = m_Bounds.center;
				
				m_Offset.x = Mathf.Clamp(m_Offset.x, -m_Bounds.extents.x, m_Bounds.extents.x);
				m_Offset.y = Mathf.Clamp(m_Offset.y, -m_Bounds.extents.y, m_Bounds.extents.y);
			}
		}
		
		private Bounds  m_Bounds;
		private Vector3 m_Origin;
		private Vector2 m_Direction;
		private Vector2 m_SmoothedDirection;
		private Vector3 m_Offset;
		private float m_Distance = 1.0f;
		
		private Vector2 m_TouchStartPos;

		
		private void Awake()
		{
			Camera = Camera.main;
			Transform = transform;
		}
		private void Update()
		{
			#if UNITY_EDITOR || UNITY_STANDALONE
			DesktopInput();
			#endif
			
			#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
			MobileInput();
			#endif

			m_SmoothedDirection.x = Mathf.LerpAngle(m_SmoothedDirection.x, m_Direction.x, Time.unscaledDeltaTime * 25.5f);
			m_SmoothedDirection.y = Mathf.LerpAngle(m_SmoothedDirection.y, m_Direction.y, Time.unscaledDeltaTime * 25.5f);

			Quaternion rotation = Quaternion.Euler(m_SmoothedDirection.y, m_SmoothedDirection.x, 0.0f);
			Transform.SetPositionAndRotation(
				rotation * Vector3.forward * -m_Distance + m_Origin + m_Offset,
				rotation
			);
		}
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(m_Bounds.center, m_Bounds.size);
			
			Gizmos.color = Color.magenta;
			Gizmos.DrawSphere(m_Origin + m_Offset, 0.1f);
		}

		private void DesktopInput()
		{
			// Тап
			if (Input.GetMouseButtonDown(0))
			{
				TapScreenPosition = Input.mousePosition;
				OnTap.Invoke();
			}
			
			// Поворот камеры
			if (Input.GetMouseButtonDown(1))
				m_TouchStartPos = Input.mousePosition;
			else if (Input.GetMouseButton(1))
			{
				Vector2 delta = (Vector2)Input.mousePosition - m_TouchStartPos;
				m_TouchStartPos = Input.mousePosition;
				
				delta.x = delta.x / Screen.width * 180.0f;
				delta.y = -delta.y / Screen.width * 180.0f;
				
				m_Direction += delta;
				m_Direction.y = Mathf.Clamp(m_Direction.y, -90.0f, 90.0f);
			}
			
			// Перемещение камеры
			if (Input.GetMouseButtonDown(2))
				m_TouchStartPos = Input.mousePosition;
			else if (Input.GetMouseButton(2))
			{
				Vector2 delta = (Vector2)Input.mousePosition - m_TouchStartPos;
				m_TouchStartPos = Input.mousePosition;
				
				delta.x = delta.x / Screen.width * m_Distance;
				delta.y = delta.y / Screen.width * m_Distance;
				
				Offset -= Transform.right * delta.x + Transform.up * delta.y;
			}
			
			// Приближение/удаление
			if (Input.mouseScrollDelta.y != 0.0f)
			{
				m_Distance -= Input.mouseScrollDelta.y * 0.25f;
				m_Distance = Mathf.Clamp(m_Distance, 0.5f, 10.0f);
			}
		}
		private void MobileInput()
		{
			int touchCount = Input.touchCount;
			switch (touchCount)
			{
				case 0:
					return;
				case 1:
				{
					Touch touch = Input.GetTouch(0);
					switch (touch.phase)
					{
						case TouchPhase.Began:
							m_TouchStartPos = touch.position;
							break;
						case TouchPhase.Moved:
						case TouchPhase.Stationary:
						{
							Vector2 delta = touch.deltaPosition;
							delta.x = delta.x / Screen.width * 180.0f;
							delta.y = -delta.y / Screen.width * 180.0f;
							m_Direction += delta;
							m_Direction.y = Mathf.Clamp(m_Direction.y, -90.0f, 90.0f);
							
							break;
						}
						case TouchPhase.Ended:
						{
							Vector2 delta = touch.position - m_TouchStartPos;
							
							// Если палец не двигался и не нажал на UI, то это тап
							if (delta.magnitude < 10.0f && !EventSystem.current.IsPointerOverGameObject())
							{
								TapScreenPosition = touch.position;
								OnTap.Invoke();
							}
							
							break;
						}
					}
					break;
				}
				default:
				{
					Touch touch0 = Input.GetTouch(0);
					Touch touch1 = Input.GetTouch(1);
				
					// Приближение/удаление
					Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
					Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
					float prevMagnitude = (touch0PrevPos - touch1PrevPos).magnitude;
					float magnitude = (touch0.position - touch1.position).magnitude;
					float deltaMagnitude = prevMagnitude - magnitude;
					if (Math.Abs(deltaMagnitude) > 0.0f)
					{
						m_Distance += deltaMagnitude / Mathf.Min(Screen.width, Screen.height) * 5.0f;
						m_Distance = Mathf.Clamp(m_Distance, 0.5f, 10.0f);
					}
				
					// Перемещение
					Vector2 center = (touch0.position + touch1.position) * 0.5f;
					Vector2 prevCenter = (touch0PrevPos + touch1PrevPos) * 0.5f;
					Vector2 delta = center - prevCenter;
					delta.x = delta.x / Screen.width * m_Distance;
					delta.y = delta.y / Screen.width * m_Distance;
					
					Offset -= Transform.right * delta.x + Transform.up * delta.y;
					
					break;
				}
			}
		}
	}
}