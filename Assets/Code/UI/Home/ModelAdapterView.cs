using ExternalResources;
using ExternalResources.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Home
{
	public class ModelAdapterView : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField] private Image m_Background;
		[SerializeField] private RawImage m_Model;
		[SerializeField] private RectTransform m_Indicator;
		[SerializeField] private Texture2D m_PlaceholderTexture;

		public Model Model { get; private set; }
		
		public UnityEvent OnClick;


		private void Awake()
		{
			m_Model.enabled = false;
			m_Indicator.gameObject.SetActive(false);
		}
		public void OnPointerClick(PointerEventData eventData) => OnClick.Invoke();
		
		public void BindModel(Model model, ModelMeta modelMeta)
		{
			Model = model;
			
			if (ColorUtility.TryParseHtmlString(modelMeta.Color, out Color color))
				m_Background.color = color;

			LoadThumbnail();
		}
		private void LoadThumbnail()
		{
			LeanTween.rotateAround(m_Indicator, Vector3.forward, 360f, 1f).setLoopClamp();
			
			ExternalResourcesManager.LoadModelThumbnail(Model.Id, texture =>
			{
				m_Model.texture = texture;
				m_Model.enabled = true;
				m_Indicator.gameObject.SetActive(false);
			}, error =>
			{
				m_Model.texture = m_PlaceholderTexture;
				m_Model.enabled = true;
				m_Indicator.gameObject.SetActive(false);
			});
		}
	}
}