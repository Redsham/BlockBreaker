using System;
using System.Collections.Generic;
using ExternalResources.Data;
using UnityEngine;
using Voxels;

namespace ExternalResources
{
	public abstract class StorageBase
	{
		public bool IsReady { get; protected set; }
		
		public abstract void Prepare(Action onError = null);
		
		
		public abstract IEnumerable<Model> Models { get; }
		public abstract bool IsModelAvailable(string id, ModelComponent component = ModelComponent.Full);
		public abstract byte GetModelVersion(string id);
		public abstract void LoadModelMeta(string id, Action<ModelMeta> callback, Action<string> errorCallback = null);
		public abstract void LoadModelAsset(string id, Action<ModelAsset> callback, Action<string> errorCallback = null);
		public abstract void LoadModelThumbnail(string id, Action<Texture2D> callback, Action<string> errorCallback = null);
	}
}