using System;
using System.Text;
using RemoteResources.Data;
using RemoteResources.Downloadings;
using UnityEngine;

namespace RemoteResources
{
	public static class RemoteResourcesManager
	{
		public const string STORAGE_URL = "https://raw.githubusercontent.com/Redsham/BlockBreaker/resources/";
		public const string MODELS_DIRECTORY = "models/";

		
		/// <summary>
		/// Готово ли хранилище к работе
		/// </summary>
		public static bool IsReady { get; private set; }
		/// <summary>
		/// Заголовок хранилища с информацией о версии и доступных моделях
		/// </summary>
		public static StorageHeader Header { get; private set; }
		
		
		/// <summary>
		/// При необходимости загружает заголовок хранилища
		/// </summary>
		/// <returns></returns>
		public static Downloading RequestHeader()
		{
			if (IsReady)
				return null;
			
			Downloading downloading = new(STORAGE_URL + "header.json");
			
			downloading.OnComplete += successful =>
			{
				if(!successful)
					return;

				Header = JsonUtility.FromJson<StorageHeader>(Encoding.UTF8.GetString(downloading.Data));
				downloading.Dispose();

				IsReady = true;
				Debug.Log($"Header (version: {Header.Version}) successfully downloaded.");
			};
			
			return downloading;
		}
		/// <summary>
		/// Загружает модель по идентификатору
		/// </summary>
		/// <param name="modelId"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static ModelDownloading RequestModel(string modelId)
		{
			if (!IsReady)
				throw new Exception("You can't load resources before the header.");
			
			return new ModelDownloading(STORAGE_URL + MODELS_DIRECTORY + modelId + "/model.bbmodel");
		}
		/// <summary>
		/// Загружает миниатюру модели по идентификатору
		/// </summary>
		/// <param name="modelId"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static TextureDownloading RequestModelThumbnail(string modelId)
		{
			if (!IsReady)
				throw new Exception("You can't load resources before the header.");
			
			return new TextureDownloading(STORAGE_URL + MODELS_DIRECTORY + modelId + "/thumbnail.png");
		}
		/// <summary>
		/// Загружает мета-данные модели по идентификатору
		/// </summary>
		/// <param name="modelId"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static MetaDownloading RequestModelMeta(string modelId)
		{
			if (!IsReady)
				throw new Exception("You can't load resources before the header.");
			
			return new MetaDownloading(STORAGE_URL + MODELS_DIRECTORY + modelId + "/meta.json");
		}
	}
}