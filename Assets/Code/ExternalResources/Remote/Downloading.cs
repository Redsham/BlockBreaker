using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Utilities;

namespace ExternalResources.Remote
{
	public class Downloading<T> : CustomYieldInstruction, IDisposable
	{
		public bool IsComplete { get; private set; }
		public bool IsSuccess { get; private set; }
		
		public T Data { get; protected set; }
		public byte[] RawData => m_Request.downloadHandler.data;
		
		public event Action<Downloading<T>> OnComplete;
		
		
		private readonly UnityWebRequest m_Request;
		private bool m_RawDataDisposed;

		protected Downloading(string url)
		{
			m_Request = UnityWebRequest.Get(url);
			CoroutineProvider.StartSceneIndependent(Download());
		}
		
		private IEnumerator Download()
		{
			yield return m_Request.SendWebRequest();
			
			if(m_Request.result == UnityWebRequest.Result.Success)
			{
				try
				{
					OnSuccessfulDownload();
				}
				catch(Exception e)
				{
					Debug.LogException(e);
					IsSuccess = false;
					IsComplete = true;
					OnComplete?.Invoke(this);
					yield break;
				}
				
				IsSuccess = true;
				IsComplete = true;
				OnComplete?.Invoke(this);
			}
			else
			{
				Debug.LogError($"Downloading failed: {m_Request.error}");
				IsSuccess = false;
				IsComplete = true;
				OnComplete?.Invoke(this);
			}
		}
		public override bool keepWaiting => !IsComplete;
		
		public void Dispose()
		{
			DisposeRawData();
			Data = default;
		}
		public void DisposeRawData()
		{
			if (m_RawDataDisposed)
				return;

			m_Request.downloadHandler.Dispose();
			m_RawDataDisposed = true;
		}
		
		protected virtual void OnSuccessfulDownload() {}
	}
}