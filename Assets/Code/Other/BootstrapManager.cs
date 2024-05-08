using System;
using System.Collections;
using Bootstrapping;
using UI.Other;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Other
{
	public class BootstrapManager : MonoBehaviour
	{
		private void Awake() => Bootstrapper.Active.OnCompleted.AddListener(OnBootstrappingCompleted);
		private void Start() => Bootstrapper.Active.Bootstrap();

		private void OnBootstrappingCompleted()
		{
			StartCoroutine(Process());
			
			IEnumerator Process()
			{
				int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
				AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nextSceneIndex);
				asyncOperation.allowSceneActivation = false;
				
				yield return new WaitUntil(() => asyncOperation.progress >= 0.9f);
				
				Fade.Show(() =>
				{
					asyncOperation.allowSceneActivation = true;
				});
			}
		}
	}
}