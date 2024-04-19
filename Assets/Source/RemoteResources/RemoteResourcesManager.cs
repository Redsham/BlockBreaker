using System.Text;
using UnityEngine;

namespace RemoteResources
{
	public static class RemoteResourcesManager
	{
		public const string STORAGE_URL = "https://raw.githubusercontent.com/Redsham/BlockBreaker/resources/";
		
		
		public static RemoteResourcesHeader Header { get; private set; }
		

		public static Downloading RequestHeader()
		{
			if (Header != null)
				return null;
			
			Downloading downloading = new(STORAGE_URL + "header.json");
			
			downloading.OnDownloaded += successful =>
			{
				if(!successful)
					return;

				Header = JsonUtility.FromJson<RemoteResourcesHeader>(Encoding.UTF8.GetString(downloading.Data));
				downloading.Dispose();
				
				Debug.Log($"Header (version: {Header.Version}) successfully downloaded.");
			};
			
			return downloading;
		}
		public static Downloading RequestModel(string modelName) => new(STORAGE_URL + modelName);
	}
}