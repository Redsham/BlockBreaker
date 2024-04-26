using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Utilities;

namespace RemoteResources
{
	public class Downloading : IDisposable
	{
		public event Action<bool> OnComplete;
		public byte[] Data => WebRequest.downloadHandler.data;
		public bool IsComplete { get; private set; }
		public bool IsSuccessful { get; private set; }
		protected UnityWebRequest WebRequest { get; }
		
		
		public Downloading(string url)
		{
			WebRequest = UnityWebRequest.Get(url);
			CoroutineProvider.Start(DownloadRoutine());
		}
		
		private IEnumerator DownloadRoutine()
		{
			yield return WebRequest.SendWebRequest();

			if (WebRequest.result == UnityWebRequest.Result.Success)
			{
				OnSuccessfulDownloaded();

				IsComplete = true;
				IsSuccessful = true;
				OnComplete?.Invoke(true);
				yield break;
			}

			IsComplete = true;
			OnComplete?.Invoke(false);
			Debug.LogError($"Download error: {WebRequest.error}");
		}
		protected virtual void OnSuccessfulDownloaded() { }
		public void Dispose() => WebRequest.Dispose();
	}
}