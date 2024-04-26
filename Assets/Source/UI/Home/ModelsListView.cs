using System.Collections;
using RemoteResources;
using RemoteResources.Downloadings;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Home
{
	public class ModelsListView : MonoBehaviour
	{
		[SerializeField] private RectTransform m_Container;
		[SerializeField] private ModelAdapterView m_ModelTemplate;

		public UnityEvent<string> OnClick;

		public void Fill()
		{
			// Загрузка оффлайн моделей
			StartCoroutine(LoadOfflineModels());
			
			// Загрузка онлайн моделей
			StartCoroutine(LoadOnlineModels());
		}

		private IEnumerator LoadOfflineModels()
		{
			// TODO: Сделать загрузку оффлайн моделей
			yield break;
		}
		private IEnumerator LoadOnlineModels()
		{
			if(!RemoteResourcesManager.IsReady)
				yield break;

			foreach (string model in RemoteResourcesManager.Header.Models)
			{
				MetaDownloading metaDownloading = RemoteResourcesManager.RequestMeta(model);
				yield return new WaitUntil(() => metaDownloading.IsComplete);
				CreateModel(model, metaDownloading.Meta);
			}
		}

		private void CreateModel(string id, MetaResource meta)
		{
			ModelAdapterView adapter = Instantiate(m_ModelTemplate, m_Container);
			adapter.BindModel(id, meta);
			adapter.OnClick.AddListener(() => { OnClick.Invoke(id); });
		}
	}
}