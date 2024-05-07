using System.Collections;
using ExternalResources;
using ExternalResources.Data;
using Gameplay;
using Gameplay.Gamemodes;
using UI.Dialogs.Core;
using UI.Dialogs.Implementations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Voxels;

namespace UI.Home
{
	public class HomeView : MonoBehaviour
	{
		[SerializeField] private ModelsListView m_ModelsView;
		[SerializeField] private ModelDialog m_ModelDialog;

		private void Start()
		{
			m_ModelsView.Fill();
			m_ModelDialog.OnPlay.AddListener(model => StartCoroutine(OpenModelProcess(model)));
		}

		public void SelectModel(Model model)
		{
			m_ModelDialog.Show(model);
		}
		
		private IEnumerator OpenModelProcess(Model model)
		{
			if(ExternalResourcesManager.OfflineMode && !ExternalResourcesManager.IsModelAvailable(model))
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
			
			ExternalResourcesManager.LoadModelAsset(model, callback =>
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