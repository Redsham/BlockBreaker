using System;
using System.Collections;
using UnityEngine.Networking;
using Utilities;

namespace RemoteResources
{
	public class Downloading : IDisposable
	{
		public event Action<bool> OnDownloaded;
		public byte[] Data => m_WebRequest.downloadHandler.data;
		
		
		private readonly UnityWebRequest m_WebRequest;
		
		
		public Downloading(string url)
		{
			m_WebRequest = UnityWebRequest.Get(url);
			CoroutineProvider.Start(DownloadRoutine());
		}
		private IEnumerator DownloadRoutine()
		{
			yield return m_WebRequest.SendWebRequest();

			if (m_WebRequest.result == UnityWebRequest.Result.Success)
			{
				OnDownloaded?.Invoke(true);
				yield break;
			}
			
			OnDownloaded?.Invoke(false);
		}
		public void Dispose() => m_WebRequest.Dispose();
	}
}