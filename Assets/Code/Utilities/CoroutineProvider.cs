using System.Collections;
using UnityEngine;

namespace Utilities
{
	public class CoroutineProvider : MonoBehaviour
	{
		private static CoroutineProvider Instance;
		private static CoroutineProvider SceneIndependentInstance;
		
		private static CoroutineProvider GetInstance()
		{
			if (Instance == null)
				Instance = new GameObject("CoroutineHelper").AddComponent<CoroutineProvider>();

			return Instance;
		}
		private static CoroutineProvider GetSceneIndependentInstance()
		{
			if (SceneIndependentInstance == null)
			{
				SceneIndependentInstance = new GameObject("CoroutineHelper").AddComponent<CoroutineProvider>();
				DontDestroyOnLoad(SceneIndependentInstance.gameObject);
			}

			return SceneIndependentInstance;
		}

		public static Coroutine Start(IEnumerator routine) => GetInstance().StartCoroutine(routine);
		public static Coroutine StartSceneIndependent(IEnumerator routine) => GetSceneIndependentInstance().StartCoroutine(routine);
		
		public static void Stop(IEnumerator routine) => GetInstance().StopCoroutine(routine);
		public static void StopSceneIndependent(IEnumerator routine) => GetSceneIndependentInstance().StopCoroutine(routine);
	}
}