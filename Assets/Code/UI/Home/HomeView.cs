using System.Collections;
using ExternalResources;
using ExternalResources.Data;
using Gameplay;
using Gameplay.Gamemodes;
using UI.Dialogs.Core;
using UI.Dialogs.Implementations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Voxels;

namespace UI.Home
{
	public class HomeView : MonoBehaviour
	{
		[SerializeField] private ModelsListView m_ModelsView;

		private void Start()
		{
			Application.targetFrameRate = 60;
			StartCoroutine(Initialization());
		}
		
		private IEnumerator Initialization()
		{
			if (!ExternalResourcesManager.IsReady)
			{
				LoadingScreenManager.Show();
				ExternalResourcesManager.Prepare();
				yield return new WaitUntil(() => ExternalResourcesManager.IsReady);
				LoadingScreenManager.Hide();
				
				AlertBox alertBox = DialogsManager.CreateDialog<AlertBox>();
				if (!ExternalResourcesManager.OfflineMode)
					alertBox.Show("Success", "Remote storage header downloaded. Ready to use.", "Success");
				else
					alertBox.Show("Error", "No internet connection. Using only local storage.", "Error");
			}

			m_ModelsView.Fill();
		}

		public void SelectModel(Model model)
		{
			//StartCoroutine(OpenModelProcess(model));
		}
		private IEnumerator OpenModelProcess(Model modelId)
		{
			if(ExternalResourcesManager.OfflineMode && !ExternalResourcesManager.IsModelAvailable(modelId.Id))
			{
				AlertBox alertBox = DialogsManager.CreateDialog<AlertBox>();
				alertBox.Show("Error", "Model is not available in offline mode.", "Error");
				yield break;
			}
			
			LoadingScreenManager.Show();

			// Скачивание модели
			bool isSuccessful = false;
			bool modelDownloading = true;
			ModelAsset modelAsset = null;
			
			ExternalResourcesManager.LoadModelAsset(modelId.Id, callback =>
			{
				modelAsset = callback;
				modelDownloading = false;
				isSuccessful = true;
			}, error =>
			{
				modelDownloading = false;
				isSuccessful = false;
				
				LoadingScreenManager.Hide();
				AlertBox alertBox = DialogsManager.CreateDialog<AlertBox>();
				alertBox.Show("Error", error, "Error");
			});
			
			// Ожидание завершения загрузки
			yield return new WaitWhile(() => modelDownloading);
			if (!isSuccessful)
				yield break;
			
			// Создание сессии
			Session session = new Session(new Disassembly(), modelAsset);
			GamemodeHandler.Session = session;
			
			// Загрузка сцены
			yield return SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
			
			LoadingScreenManager.Hide();
		}
	}
}