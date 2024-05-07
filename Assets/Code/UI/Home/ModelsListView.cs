using ExternalResources;
using ExternalResources.Data;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Home
{
	public class ModelsListView : MonoBehaviour
	{
		[SerializeField] private RectTransform m_Container;
		[SerializeField] private ModelAdapterView m_ModelTemplate;

		public UnityEvent<Model> OnClick;

		public void Fill()
		{
			foreach (Model model in ExternalResourcesManager.Models)
			{
				ExternalResourcesManager.LoadModelMeta(model, meta =>
				{
					if(!ExternalResourcesManager.OfflineMode || ExternalResourcesManager.IsModelAvailable(model))
						CreateModel(model, meta);
				}, error =>
				{
					Debug.LogError($"Model meta loading failed: {error}");
				});
			}
		}

		private void CreateModel(Model model, ModelMeta modelMeta)
		{
			ModelAdapterView adapter = Instantiate(m_ModelTemplate, m_Container);
			adapter.BindModel(model);
			adapter.OnClick.AddListener(() => { OnClick.Invoke(model); });
		}
	}
}