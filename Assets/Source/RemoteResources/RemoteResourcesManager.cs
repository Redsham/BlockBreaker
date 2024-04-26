using System;
using System.Text;
using RemoteResources.Downloadings;
using UnityEngine;

namespace RemoteResources
{
	public static class RemoteResourcesManager
	{
		public const string STORAGE_URL = "https://raw.githubusercontent.com/Redsham/BlockBreaker/resources/";
		public const string MODELS_DIRECTORY = "models/";
		
		
		public static RemoteResourcesHeader Header { get; private set; }
		

		public static Downloading RequestHeader()
		{
			if (Header != null)
				return null;
			
			Downloading downloading = new(STORAGE_URL + "header.json");
			
			downloading.OnComplete += successful =>
			{
				if(!successful)
					return;

				Header = JsonUtility.FromJson<RemoteResourcesHeader>(Encoding.UTF8.GetString(downloading.Data));
				downloading.Dispose();
				
				Debug.Log($"Header (version: {Header.Version}) successfully downloaded.");
			};
			
			return downloading;
		}
		public static ModelDownloading RequestModel(string modelName)
		{
			if (Header == null)
				throw new Exception("You can't load resources before the header.");
			
			return new(STORAGE_URL + MODELS_DIRECTORY + modelName + "/model.bbmodel");
		}
		public static TextureDownloading RequestThumbnail(string modelName)
		{
			if (Header == null)
				throw new Exception("You can't load resources before the header.");
			
			return new(STORAGE_URL + MODELS_DIRECTORY + modelName + "/thumbnail.png");
		}
	}
}