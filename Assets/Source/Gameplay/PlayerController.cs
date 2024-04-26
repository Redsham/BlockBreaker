using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Gameplay
{
	public class PlayerController : MonoBehaviour
	{
		public UnityEvent OnTap = new UnityEvent();
		public Camera Camera { get; private set; }
		
		private Vector2 m_Origin;
		private Vector2 m_Direction;
		private Vector3 m_Offset;
		private float m_Distance = 1.0f;
		
		private Vector2 m_TouchStartPos;

		private void Awake()
		{
			Camera = Camera.main;
		}
		private void Update()
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
						
							UpdateTransform();
							break;
						}
						case TouchPhase.Ended:
						{
							Vector2 delta = touch.position - m_TouchStartPos;
							if (Math.Abs(delta.x) < 10.0f && Math.Abs(delta.y) < 10.0f && !EventSystem.current.IsPointerOverGameObject())
								OnTap.Invoke();
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
						m_Distance += deltaMagnitude / Mathf.Min(Screen.width, Screen.height) * 10.0f;
						m_Distance = Mathf.Clamp(m_Distance, 0.5f, 10.0f);
						
						UpdateTransform();
					}
				
					// Перемещение
					Vector2 center = (touch0.position + touch1.position) * 0.5f;
					Vector2 prevCenter = (touch0PrevPos + touch1PrevPos) * 0.5f;
					Vector2 delta = center - prevCenter;
					delta.x = delta.x / Screen.width * m_Distance;
					delta.y = delta.y / Screen.width * m_Distance;
					m_Offset -= transform.right * delta.x + transform.up * delta.y;
					
					UpdateTransform();
					break;
				}
			}
		}
		private void UpdateTransform()
		{
			transform.rotation = Quaternion.Euler(m_Direction.y, m_Direction.x, 0.0f);
			transform.position = transform.forward * -m_Distance + m_Offset;
		}
	}
}