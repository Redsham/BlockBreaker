using TMPro;
using UI.Audio;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
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

		[Header("Behaviour")] [SerializeField] 
		private string m_SoundId = "click";
		[SerializeField] private AnimationModule m_AnimationModule;
		
		
		
		private void Awake()
		{
			if (m_Background == null)
				m_Background = GetComponent<Image>();
			
			if (m_Text == null)
				m_Text = GetComponentInChildren<TextMeshProUGUI>();
			
			if (m_Thumbnail == null)
				m_Thumbnail = GetComponentInChildren<Image>();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if(!string.IsNullOrEmpty(m_SoundId))
				UISounds.Play(m_SoundId);
			
			m_AnimationModule.Play();
			m_OnClick.Invoke();
		}
		public void OnPointerDown(PointerEventData eventData) => m_OnDown.Invoke();
		public void OnPointerUp(PointerEventData eventData) => m_OnUp.Invoke();

		[System.Serializable]
		public class AnimationModule
		{
			[SerializeField] private RectTransform m_Content;
			[SerializeField] private AnimationCurve m_SizeCurve = AnimationCurve.EaseInOut(0.0f, 0.85f, 1.0f, 1.0f);
			[SerializeField] private AnimationCurve m_RotationCurve = AnimationCurve.Constant(0.0f, 1.0f, 1.0f);
			[SerializeField] private float m_Duration = 0.1f;
			
			private int m_LeanTweenId;
			

			public void Play()
			{
				if(m_LeanTweenId == 0 && m_Content == null)
					return;
				
				LeanTween.cancel(m_LeanTweenId);

				float sizeDuration = m_SizeCurve.keys[^1].time;
				float rotationDuration = m_RotationCurve.keys[^1].time;

				m_LeanTweenId = LeanTween.value(
					m_Content.gameObject,
					(float time) =>
					{
						
						float size = m_SizeCurve.Evaluate(sizeDuration * time);
						m_Content.localScale = new Vector3(size, size, 1.0f);
						
						float rotation = m_RotationCurve.Evaluate(rotationDuration * time);
						m_Content.eulerAngles = new Vector3(0.0f, 0.0f, rotation);
						
					}, 0.0f, 1.0f, m_Duration).id;
			}
		}
	}
}
