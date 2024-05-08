using ExternalResources.Data;
using UI.Audio;
using UI.Other;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI.MainMenu
{
	public class ModelAdapterView : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField] private ModelThumbnail m_Thumbnail;

		public Model Model { get; private set; }
		
		public UnityEvent OnClick;
		

		public void OnPointerClick(PointerEventData eventData)
		{
			OnClick.Invoke();
			UISounds.Play("click");
			PlayAnimation();
		}
		private void PlayAnimation()
		{
			// Отмена предыдущих анимаций
			LeanTween.cancel(m_Thumbnail.Background.gameObject);
			LeanTween.cancel(m_Thumbnail.Thumbnail.gameObject);
			
			// Фон
			m_Thumbnail.Background.rectTransform.localScale = Vector3.one * 0.75f;
			LeanTween.scale(m_Thumbnail.Background.gameObject, Vector3.one, 0.2f).setEaseOutCubic();

			// Модель
			m_Thumbnail.Thumbnail.rectTransform.localScale = Vector3.one * 0.75f;
			LeanTween.scale(m_Thumbnail.Thumbnail.gameObject, Vector3.one, 0.1f);
		}
		
		public void BindModel(Model model)
		{
			Model = model;
			m_Thumbnail.SetModel(model);
		}
	}
}