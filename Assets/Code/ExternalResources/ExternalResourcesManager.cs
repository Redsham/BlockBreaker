using System;
using System.Collections;
using System.Collections.Generic;
using Bootstrapping;
using ExternalResources.Data;
using ExternalResources.Local;
using ExternalResources.Remote;
using UnityEngine;
using Voxels;

namespace ExternalResources
{
	/// <summary>
	/// Внешние ресурсы - это ресурсы, которые не входят в состав приложения и загружаются из внешних источников, в конкретном случае с репозитория на GitHub.
	/// На практике вместо репозитория на GitHub может быть любой другой источник данных, например, сервер с базой данных.
	/// </summary>
	public static class ExternalResourcesManager
	{
		private static readonly LocalStorage  Local = new();
		private static readonly RemoteStorage Remote = new();
		
		public static bool OfflineMode { get; private set; }
		public static bool IsReady => Local.IsReady && (OfflineMode || Remote.IsReady);
		
		/// <summary>
		/// Список всех известных моделей.
		/// Если ресурсы не готовы, возвращает пустой массив.
		/// Если включен оффлайн-режим, возвращает список моделей из локального хранилища.
		/// </summary>
		public static IEnumerable<Model> Models => IsReady ? (OfflineMode ? Local.Models : Remote.Models) : Array.Empty<Model>();
		
		
		
		[BootstrapMethod]
		public static IEnumerator Bootstrap()
		{
			Prepare();
			yield return new WaitUntil(() => IsReady);
		}
		
		
		
		/// <summary>
		/// Подготовка к работе с внешними ресурсами.
		/// </summary>
		public static void Prepare()
		{
			if(IsReady)
				return;
			
			// Подготовка локального хранилища
			Local.Prepare(() =>
			{
				Debug.LogError("Local storage preparation failed!");
			});
			
			// Проверка наличия интернет-соединения
			if(Application.internetReachability != NetworkReachability.NotReachable)
			{
				// Подготовка удаленного хранилища
				Remote.Prepare(() =>
				{
					Debug.LogWarning("Remote storage header downloading failed. Using only local storage.");
					OfflineMode = true;
				});
				
				// Подписка на событие загрузки компонента модели
				Remote.OnModelComponentDownloaded += (model, component, data) =>
				{
					Local.SaveModelComponent(model, component, data);
				};
			}
			else
				OfflineMode = true;
		}
		
		/// <summary>
		/// Проверка наличия компонента модели в хранилище.
		/// Если включен оффлайн-режим, проверяется только локальное хранилище.
		/// </summary>
		/// <param name="id">Идентификатор модели</param>
		/// <param name="component">Компонент модели</param>
		public static bool IsModelAvailable(string id, ModelComponent component = ModelComponent.Full) => OfflineMode ? Local.IsModelAvailable(id, component) : Remote.IsModelAvailable(id, component);
		/// <summary>
		/// Загрузка метаданных модели.
		/// </summary>
		public static void LoadModelMeta(string id, Action<ModelMeta> callback, Action<string> errorCallback = null)
		{
			if(OfflineMode)
				Local.LoadModelMeta(id, callback, errorCallback); // Загрузка ассета модели из локального хранилища
			else
			{
				// Если модель отсутствует в локальном хранилище или версия модели на сервере больше, чем локальная, загружаем с сервера
				if(!Local.IsModelAvailable(id, ModelComponent.Meta) || Remote.GetModelVersion(id) > Local.GetModelVersion(id))
					Remote.LoadModelMeta(id, callback, errorCallback);
				else
					Local.LoadModelMeta(id, callback, errorCallback);
			}
		}
		/// <summary>
		/// Загрузка миниатюры модели.
		/// </summary>
		public static void LoadModelThumbnail(string id, Action<Texture2D> callback, Action<string> errorCallback = null)
		{
			if(OfflineMode)
				Local.LoadModelThumbnail(id, callback, errorCallback); // Загрузка ассета модели из локального хранилища
			else
			{
				// Если модель отсутствует в локальном хранилище или версия модели на сервере больше, чем локальная, загружаем с сервера
				if(!Local.IsModelAvailable(id, ModelComponent.Thumbnail) || Remote.GetModelVersion(id) > Local.GetModelVersion(id))
					Remote.LoadModelThumbnail(id, callback, errorCallback);
				else
					Local.LoadModelThumbnail(id, callback, errorCallback);
			}
		}
		/// <summary>
		/// Загрузка ассета модели.
		/// </summary>
		public static void LoadModelAsset(string id, Action<ModelAsset> callback, Action<string> errorCallback = null)
		{
			if(OfflineMode)
				Local.LoadModelAsset(id, callback, errorCallback); // Загрузка ассета модели из локального хранилища
			else
			{
				// Если модель отсутствует в локальном хранилище или версия модели на сервере больше, чем локальная, загружаем с сервера
				if(!Local.IsModelAvailable(id, ModelComponent.Asset) || Remote.GetModelVersion(id) > Local.GetModelVersion(id))
					Remote.LoadModelAsset(id, callback, errorCallback);
				else
					Local.LoadModelAsset(id, callback, errorCallback);
			}
		}
	}
}