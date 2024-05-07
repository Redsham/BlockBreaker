using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
	[RequireComponent(typeof(Graphics))]
	public class AdvancedButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
	{
		public UnityEvent OnDown => m_OnDown;
		public UnityEvent OnClick => m_OnClick;
		public UnityEvent OnUp => m_OnUp;

		
		[SerializeField] private UnityEvent m_OnDown = new();
		[SerializeField] private UnityEvent m_OnClick = new();
		[SerializeField] private UnityEvent m_OnUp = new();
		

		public void OnPointerClick(PointerEventData eventData) => m_OnClick.Invoke();
		public void OnPointerDown(PointerEventData eventData) => m_OnDown.Invoke();
		public void OnPointerUp(PointerEventData eventData) => m_OnUp.Invoke();
	}
}
