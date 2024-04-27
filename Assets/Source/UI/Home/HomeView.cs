using System.Collections;
using Gameplay;
using Gameplay.Gamemodes;
using RemoteResources;
using RemoteResources.Downloadings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Home
{
	public class HomeView : MonoBehaviour
	{
		[SerializeField] private ModelsListView m_ModelsView;

		private void Start() => StartCoroutine(Initialization());
		
		private IEnumerator Initialization()
		{
			if (!RemoteResourcesManager.IsReady)
			{
				LoadingScreenManager.Show();
				Downloading headerDownloading = RemoteResourcesManager.RequestHeader();
				yield return new WaitUntil(() => headerDownloading.IsComplete);
				LoadingScreenManager.Hide();
			}

			m_ModelsView.Fill();
		}

		public void SelectModel(string id)
		{
			StartCoroutine(OpenModelProcess(id));
		}
		private IEnumerator OpenModelProcess(string id)
		{
			if(!RemoteResourcesManager.IsReady)
				yield break;
			
			LoadingScreenManager.Show();

			// Скачивание модели
			ModelDownloading modelDownloading = RemoteResourcesManager.RequestModel(id);
			yield return new WaitUntil(() => modelDownloading.IsComplete);

			if (!modelDownloading.IsSuccessful)
			{
				Debug.LogError("Model downloading failed");
				yield break;
			}
			
			// Создание сессии
			Session session = new Session(new Disassembly(), modelDownloading.Model);
			GamemodeHandler.Session = session;
			
			// Загрузка сцены
			yield return SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
			
			LoadingScreenManager.Hide();
		}
	}
}