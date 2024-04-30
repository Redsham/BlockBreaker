using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ExternalResources.Data;
using UnityEngine;
using Voxels;

namespace ExternalResources.Local
{
	public class LocalStorage : StorageBase
	{
		public static readonly string StorageDirectory;
		
		
		static LocalStorage()
		{
			StorageDirectory = Path.Combine(Application.persistentDataPath, "Storage");
			if(!Directory.Exists(StorageDirectory))
				Directory.CreateDirectory(StorageDirectory);
		}
		
		
		public static string GetAbsolutePath(params string[] relativePath) => Path.Combine(StorageDirectory, Path.Combine(relativePath));
		
		
		
		private StorageHeader m_Header;
		public override IEnumerable<Model> Models => m_Header.Models;
		
		public override void Prepare(Action onError = null)
		{
			if(File.Exists(GetAbsolutePath("header.json")))
			{
				try
				{
					m_Header = StorageHeader.FromJson(File.ReadAllText(GetAbsolutePath("header.json"), Encoding.UTF8));
				}
				catch(Exception e)
				{
					Debug.LogError("Failed to load local storage header: " + e.Message);
					m_Header = new StorageHeader();
				}
			}
			else
				m_Header = new StorageHeader();
			
			IsReady = true;
		}

		public override bool IsModelAvailable(string id, ModelComponent component = ModelComponent.Full)
		{
			LocalModel model = m_Header.Models.FirstOrDefault(model => model.Id == id);
			if(model == null)
				return false;

			return component switch
			{
				ModelComponent.Meta => model.MetaDownloaded,
				ModelComponent.Thumbnail => model.ThumbnailDownloaded,
				ModelComponent.Asset => model.AssetDownloaded,
				ModelComponent.Full => model.MetaDownloaded && model.ThumbnailDownloaded && model.AssetDownloaded,
				_ => throw new ArgumentOutOfRangeException(nameof(component), component, null)
			};
		}
		public override byte GetModelVersion(string id) => m_Header.Models.First(model => model.Id == id).Version;
		public override void LoadModelMeta(string id, Action<ModelMeta> callback, Action<string> errorCallback = null)
		{
			string path = GetAbsolutePath("models", id, "meta.json");
			if(File.Exists(path))
			{
				try
				{
					callback?.Invoke(ModelMeta.FromJson(File.ReadAllText(path, Encoding.UTF8)));
				}
				catch(Exception e)
				{
					errorCallback?.Invoke("Failed to load meta.");
				}
			}
			else
				errorCallback?.Invoke("Meta file not found.");
		}
		public override void LoadModelAsset(string id, Action<ModelAsset> callback, Action<string> errorCallback = null)
		{
			string path = GetAbsolutePath("models", id, "model.bbmodel");
			if(File.Exists(path))
			{
				try
				{
					callback?.Invoke(ModelAsset.Deserialize(File.ReadAllBytes(path)));
				}
				catch(Exception e)
				{
					errorCallback?.Invoke("Failed to load asset.");
				}
			}
			else
				errorCallback?.Invoke("Asset file not found.");
		}
		public override void LoadModelThumbnail(string id, Action<Texture2D> callback, Action<string> errorCallback = null)
		{
			string path = GetAbsolutePath("models", id, "thumbnail.png");
			if(File.Exists(path))
			{
				try
				{
					Texture2D texture = new Texture2D(0, 0);
					texture.LoadImage(File.ReadAllBytes(path));
					callback?.Invoke(texture);
				}
				catch(Exception e)
				{
					errorCallback?.Invoke("Failed to load thumbnail.");
				}
			}
			else
				errorCallback?.Invoke("Thumbnail file not found.");
		}
		
		
		public void SaveModelComponent(Model remoteModel, ModelComponent component, byte[] data)
		{
			LocalModel model;
			
			string path = GetAbsolutePath("models", remoteModel.Id);
			
			// Если отсутствует папка модели, то делаем вывод, что модель полностью отсутствует в локальном хранилище
			if (!Directory.Exists(path))
			{
				// Создаем папку модели
				Directory.CreateDirectory(path);
				
				// Создаем новую запись о модели
				model = new LocalModel()
				{
					Id = remoteModel.Id,
					Version = remoteModel.Version
				};
				m_Header.Models.Add(model);
			}
			else
			{
				// Ищем запись о модели
				model = m_Header.Models.First(x => x.Id == remoteModel.Id);
				
				// Если версия модели для сохранения больше, чем локальная, удаляем локальную
				if (model.Version < remoteModel.Version)
				{
					// Удаляем старые файлы
					Directory.Delete(path, true);
					Directory.CreateDirectory(path);

					// Обновляем версию модели
					model.Version = remoteModel.Version;
					model.MetaDownloaded = false;
					model.ThumbnailDownloaded = false;
					model.AssetDownloaded = false;
				}
				
				// Если версия модели для сохранения меньше, чем локальная, пропускаем сохранение
				if (model.Version > remoteModel.Version)
					return;
			}
			
			// Сохраняем компонент модели
			switch(component)
			{
				case ModelComponent.Meta:
					File.WriteAllBytesAsync(GetAbsolutePath("models", remoteModel.Id, "meta.json"), data);
					model.MetaDownloaded = true;
					break;
				case ModelComponent.Thumbnail:
					File.WriteAllBytesAsync(GetAbsolutePath("models", remoteModel.Id, "thumbnail.png"), data);
					model.ThumbnailDownloaded = true;
					break;
				case ModelComponent.Asset:
					File.WriteAllBytesAsync(GetAbsolutePath("models", remoteModel.Id, "model.bbmodel"), data);
					model.AssetDownloaded = true;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(component), component, null);
			}
			
			SaveHeader();
		}
		public void SaveHeader() => File.WriteAllTextAsync(GetAbsolutePath("header.json"), m_Header.ToJson(), Encoding.UTF8);
	}
}