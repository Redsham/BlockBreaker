using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(Image))]
	public class AdvancedButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
	{
		public UnityEvent OnDown => m_OnDown;
		public UnityEvent OnClick => m_OnClick;
		public UnityEvent OnUp => m_OnUp;
		
		public string Text
		{
			get => m_Text.text;
			set => m_Text.text = value;
		}
		public Sprite Thumbnail
		{
			get => m_Thumbnail.sprite;
			set => m_Thumbnail.sprite = value;
		}
		
		public Image Background => m_Background;
		public TextMeshProUGUI TextComponent => m_Text;
		public Image ThumbnailComponent => m_Thumbnail;
		

		[Header("Events")]
		[SerializeField] private UnityEvent m_OnDown = new();
		[SerializeField] private UnityEvent m_OnClick = new();
		[SerializeField] private UnityEvent m_OnUp = new();
		
		[Header("Components")]
		[SerializeField] private Image m_Background;
		[SerializeField] private TextMeshProUGUI m_Text;
		[SerializeField] private Image m_Thumbnail;
		
		private void Awake()
		{
			if (m_Background == null)
				m_Background = GetComponent<Image>();
			
			if (m_Text == null)
				m_Text = GetComponentInChildren<TextMeshProUGUI>();
			
			if (m_Thumbnail == null)
				m_Thumbnail = GetComponentInChildren<Image>();
		}

		public void OnPointerClick(PointerEventData eventData) => m_OnClick.Invoke();
		public void OnPointerDown(PointerEventData eventData) => m_OnDown.Invoke();
		public void OnPointerUp(PointerEventData eventData) => m_OnUp.Invoke();
	}
}
