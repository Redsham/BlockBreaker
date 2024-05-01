using System;
using System.Collections.Generic;
using System.Linq;
using ExternalResources.Data;
using ExternalResources.Remote.Downloads;
using UnityEngine;
using Voxels;

namespace ExternalResources.Remote
{
	public class RemoteStorage : StorageBase
	{
		public const string STORAGE_URL = "https://raw.githubusercontent.com/Redsham/BlockBreaker/resources/";
		
		
		private StorageHeader m_Header;
		public override IEnumerable<Model> Models => m_Header.Models;
		
		public event Action<Model, ModelComponent, byte[]> OnModelComponentDownloaded; 
		
		public override void Prepare(Action onError = null)
		{
			new HeaderDownloading(STORAGE_URL + "header.json").OnComplete += downloading =>
			{
				if(downloading.IsSuccess)
				{
					m_Header = downloading.Data;
					IsReady = true;
				}
				else
					onError?.Invoke();
				
				downloading.Dispose();
			};
		}


		public override bool IsModelAvailable(string id, ModelComponent component = ModelComponent.Full) => m_Header.Models.Any(model => model.Id == id);
		public override byte GetModelVersion(string id) => m_Header.Models.First(model => model.Id == id).Version;
		public override void LoadModelMeta(string id, Action<ModelMeta> callback, Action<string> errorCallback = null)
		{
			new MetaDownloading(STORAGE_URL + "models/" + id + "/meta.json").OnComplete += downloading =>
			{
				if (downloading.IsSuccess)
				{
					OnModelComponentDownloaded?.Invoke(m_Header.Models.First(model => model.Id == id), ModelComponent.Meta, downloading.RawData);
					callback(downloading.Data);
				}
				else
					errorCallback?.Invoke("Downloading failed.");
				
				downloading.DisposeRawData();
			};
		}
		public override void LoadModelAsset(string id, Action<ModelAsset> callback, Action<string> errorCallback = null)
		{
			new ModelDownloading(STORAGE_URL + "models/" + id + "/model.bbmodel").OnComplete += downloading =>
			{
				if (downloading.IsSuccess)
				{
					OnModelComponentDownloaded?.Invoke(m_Header.Models.First(model => model.Id == id), ModelComponent.Asset, downloading.RawData);
					callback(downloading.Data);
				}
				else
					errorCallback?.Invoke("Downloading failed.");
				
				downloading.DisposeRawData();
			};
		}
		public override void LoadModelThumbnail(string id, Action<Texture2D> callback, Action<string> errorCallback = null)
		{
			new TextureDownloading(STORAGE_URL + "models/" + id + "/thumbnail.png").OnComplete += downloading =>
			{
				if (downloading.IsSuccess)
				{
					OnModelComponentDownloaded?.Invoke(m_Header.Models.First(model => model.Id == id), ModelComponent.Thumbnail, downloading.RawData);
					callback(downloading.Data);
				}
				else
					errorCallback?.Invoke("Downloading failed.");
				
				downloading.DisposeRawData();
			};
		}
	}
}