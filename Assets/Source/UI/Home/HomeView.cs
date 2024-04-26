using System;
using System.Collections;
using RemoteResources;
using UnityEngine;

namespace UI.Home
{
	public class HomeView : MonoBehaviour
	{
		[SerializeField] private ModelsListView m_ModelsView;

		private void Start()
		{
			StartCoroutine(Initialization());
		}

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
	}
}