using System;
using UnityEngine;

namespace UI
{
	public class ProgressBar : MonoBehaviour
	{
		public float Value
		{
			get => m_Value;
			set
			{
				m_Value = Mathf.Clamp01(value);
				ApplyValue();
			}
		}
		[SerializeField, Range(0.0f, 1.0f)] private float m_Value;
		
		private RectTransform m_Body;
		private RectTransform m_Fill;
		private RectTransform m_Head;

		private void Start() => Init();
		private void OnValidate()
		{
			if (m_Body == null)
				Init();
			
			ApplyValue();
		}
		
		private void Init()
		{
			m_Body = transform as RectTransform;
			m_Fill = transform.Find("Fill") as RectTransform;
			m_Head = transform.Find("Head") as RectTransform;
			
			if (m_Fill == null)
				throw new Exception("Fill is not found");
		}
		private void ApplyValue()
		{
			Rect bodyRect = m_Body.rect;
			m_Fill.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0.0f, Mathf.Lerp(bodyRect.height, bodyRect.width, m_Value));
			
			if (m_Head != null)
				m_Head.anchoredPosition = new Vector2(Mathf.Lerp(bodyRect.height / 2.0f, bodyRect.width - bodyRect.height / 2.0f, m_Value), 0.0f);
		}
	}
}