using System.Collections;
using RemoteResources;
using RemoteResources.Data;
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
				MetaDownloading metaDownloading = RemoteResourcesManager.RequestModelMeta(model);
				yield return new WaitUntil(() => metaDownloading.IsComplete);
				CreateModel(model, metaDownloading.ModelMeta);
			}
		}

		private void CreateModel(string id, ModelMeta modelMeta)
		{
			ModelAdapterView adapter = Instantiate(m_ModelTemplate, m_Container);
			adapter.BindModel(id, modelMeta);
			adapter.OnClick.AddListener(() => { OnClick.Invoke(id); });
		}
	}
}