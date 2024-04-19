using System.Collections;
using UnityEngine;

namespace Utilities
{
	public class CoroutineProvider : MonoBehaviour
	{
		private static CoroutineProvider Instance;
		private static CoroutineProvider GetInstance()
		{
			if (Instance == null)
				Instance = new GameObject("CoroutineHelper").AddComponent<CoroutineProvider>();

			return Instance;
		}

		public static Coroutine Start(IEnumerator routine) => GetInstance().StartCoroutine(routine);
		public static void Stop(IEnumerator routine) => GetInstance().StopCoroutine(routine);
	}
}