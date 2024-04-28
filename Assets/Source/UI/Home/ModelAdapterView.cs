using System.Collections;
using RemoteResources;
using RemoteResources.Data;
using RemoteResources.Downloadings;
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

		public string ModelID { get; private set; }
		
		public UnityEvent OnClick;


		private void Awake()
		{
			m_Model.enabled = false;
			m_Indicator.gameObject.SetActive(false);
		}
		public void OnPointerClick(PointerEventData eventData) => OnClick.Invoke();
		
		public void BindModel(string id, ModelMeta modelMeta)
		{
			ModelID = id;
			
			if (ColorUtility.TryParseHtmlString(modelMeta.Color, out Color color))
				m_Background.color = color;

			StartCoroutine(LoadThumbnail());
		}
		private IEnumerator LoadThumbnail()
		{
			TextureDownloading textureDownloading = RemoteResourcesManager.RequestModelThumbnail(ModelID);
			m_Indicator.gameObject.SetActive(true);
			while (!textureDownloading.IsComplete)
			{
				m_Indicator.Rotate(0.0f, 0.0f, -90.0f * Time.deltaTime);
				yield return null;
			}

			m_Model.texture = textureDownloading.IsSuccessful ? textureDownloading.Texture : m_PlaceholderTexture;
			
			m_Model.enabled = true;
			m_Indicator.gameObject.SetActive(false);
		}
	}
}