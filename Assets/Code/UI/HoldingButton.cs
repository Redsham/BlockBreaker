using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(Graphics))]
	public class HoldingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public UnityEvent OnClick => m_OnClick;
		public float Duration
		{
			get => m_Duration;
			set => m_Duration = value;
		}
		public float Time
		{
			get => m_Time;
			set
			{
				if(Math.Abs(value - m_Time) < 0.001f)
					return;
				
				m_Time = value;
				m_ProgressBar.fillAmount = Time / Duration;
			}
		}
		
		
		[SerializeField] private UnityEvent m_OnClick;
		[SerializeField] private float m_Duration = 1.0f;
		[SerializeField] private Image m_ProgressBar;
		private bool m_IsHolding;
		private float m_Time;
		

		public void OnPointerDown(PointerEventData eventData) => m_IsHolding = true;
		public void OnPointerUp(PointerEventData eventData) => m_IsHolding = false;
		
		private void Update()
		{
			if (m_IsHolding)
			{
				Time = Mathf.Clamp(Time + UnityEngine.Time.deltaTime, 0.0f, Duration);
				if (!(Math.Abs(Time - Duration) < 0.001f))
					return;

				m_OnClick.Invoke();
				m_IsHolding = false;
				Time = 0.0f;
			}
			else if (Time > 0.0f)
			{
				Time = Mathf.Clamp(Time - UnityEngine.Time.deltaTime * 5.0f, 0.0f, Duration);
			}
		}
	}
}