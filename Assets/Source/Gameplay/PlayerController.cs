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
				ApplyTransform();
			}
		}

		private Vector3 m_Origin;
		private Vector2 m_Direction;
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
				
				ApplyTransform();
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
				m_Offset -= Transform.right * delta.x + Transform.up * delta.y;
				
				ApplyTransform();
			}
			
			// Приближение/удаление
			if (Input.mouseScrollDelta.y != 0.0f)
			{
				m_Distance -= Input.mouseScrollDelta.y * 0.25f;
				m_Distance = Mathf.Clamp(m_Distance, 0.5f, 10.0f);
				
				ApplyTransform();
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
						
							ApplyTransform();
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
						
						ApplyTransform();
					}
				
					// Перемещение
					Vector2 center = (touch0.position + touch1.position) * 0.5f;
					Vector2 prevCenter = (touch0PrevPos + touch1PrevPos) * 0.5f;
					Vector2 delta = center - prevCenter;
					delta.x = delta.x / Screen.width * m_Distance;
					delta.y = delta.y / Screen.width * m_Distance;
					m_Offset -= Transform.right * delta.x + Transform.up * delta.y;
					
					ApplyTransform();
					break;
				}
			}
		}
		
		private void ApplyTransform()
		{
			Transform.rotation = Quaternion.Euler(m_Direction.y, m_Direction.x, 0.0f);
			Transform.position = Transform.forward * -m_Distance + m_Origin + m_Offset;
		}
	}
}