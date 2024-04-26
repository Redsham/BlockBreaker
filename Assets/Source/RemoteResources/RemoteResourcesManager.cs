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

		public static bool IsReady { get; private set; }
		public static RemoteResourcesHeader Header { get; private set; }
		

		public static Downloading RequestHeader()
		{
			if (IsReady)
				return null;
			
			Downloading downloading = new(STORAGE_URL + "header.json");
			
			downloading.OnComplete += successful =>
			{
				if(!successful)
					return;

				Header = JsonUtility.FromJson<RemoteResourcesHeader>(Encoding.UTF8.GetString(downloading.Data));
				downloading.Dispose();

				IsReady = true;
				Debug.Log($"Header (version: {Header.Version}) successfully downloaded.");
			};
			
			return downloading;
		}
		public static ModelDownloading RequestModel(string modelName)
		{
			if (!IsReady)
				throw new Exception("You can't load resources before the header.");
			
			return new ModelDownloading(STORAGE_URL + MODELS_DIRECTORY + modelName + "/model.bbmodel");
		}
		public static TextureDownloading RequestThumbnail(string modelName)
		{
			if (!IsReady)
				throw new Exception("You can't load resources before the header.");
			
			return new TextureDownloading(STORAGE_URL + MODELS_DIRECTORY + modelName + "/thumbnail.png");
		}
		public static MetaDownloading RequestMeta(string modelName)
		{
			if (!IsReady)
				throw new Exception("You can't load resources before the header.");
			
			return new MetaDownloading(STORAGE_URL + MODELS_DIRECTORY + modelName + "/meta.json");
		}
	}
}