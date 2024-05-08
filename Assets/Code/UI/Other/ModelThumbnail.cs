using System;
using System.Collections.Generic;
using ExternalResources;
using ExternalResources.Data;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Other
{
	public class ModelThumbnail : MonoBehaviour
	{
		private static Dictionary<int, ThumbnailData> Thumbnails = new Dictionary<int, ThumbnailData>();
		private static readonly int DiscolorationProperty = Shader.PropertyToID("_Discoloration");
		
		public RawImage Thumbnail => m_Thumbnail;
		public Image Background => m_Background;
		
		[SerializeField] private RawImage m_Thumbnail;
		[SerializeField] private Image m_Background;
		[SerializeField] private Shader m_ThumbnailShader;
		[SerializeField] private RectTransform m_Indicator;
		
		private Material m_ThumbnailMaterial;
		private ThumbnailData m_ThumbnailData;
		private int m_IndicatorTweenId;
		private Mask m_Mask;
		private int m_DiscolorationTweenId;


		private void Awake()
		{
			// Инициализация шейдера
			m_ThumbnailMaterial = m_Thumbnail.material = new Material(m_ThumbnailShader);
			
			UserStats.OnModelUnlocked += modelId =>
			{
				if (m_ThumbnailData == null || m_ThumbnailData.Model.Id != modelId)
					return;

				LeanTween.cancel(m_DiscolorationTweenId);
				m_DiscolorationTweenId = LeanTween.value(gameObject, 1.0f, 0.0f, 0.1f).setOnUpdate(value =>
				{
					m_Thumbnail.materialForRendering.SetFloat(DiscolorationProperty, value);
				}).id;
			};
		}
		private void OnDestroy()
		{
			Destroy(m_ThumbnailMaterial);
			m_ThumbnailData.UsageCount--;
		}

		
		public void SetModel(Model model)
		{
			// Отписка от старой миниатюры
			if (m_ThumbnailData != null)
				m_ThumbnailData.UsageCount--;
			
			// Получение миниатюры
			m_ThumbnailData = GetCachedThumbnail(model);
			m_ThumbnailData.UsageCount++;
			
			m_Thumbnail.materialForRendering.SetFloat(DiscolorationProperty, UserStats.IsModelUnlocked(model.Id) ? 0.0f : 1.0f);
			
			if (m_ThumbnailData.IsReady)
				ApplyThumbnail();
			else
			{
				// Отображение индикатора загрузки
				m_Indicator.gameObject.SetActive(true);
				m_IndicatorTweenId = LeanTween.rotateAround(m_Indicator, Vector3.forward, 360f, 1f).setLoopClamp().id;
				m_Thumbnail.enabled = false;
				
				m_ThumbnailData.OnLoaded += () =>
				{
					// Скрытие индикатора загрузки
					LeanTween.cancel(m_IndicatorTweenId);
					m_Indicator.gameObject.SetActive(false);
					m_Thumbnail.enabled = true;
					
					ApplyThumbnail();
				};
			}
		}
		private void ApplyThumbnail()
		{
			m_Thumbnail.texture = m_ThumbnailData.Texture;
			m_Background.color = m_ThumbnailData.Color;
		}


		private static ThumbnailData GetCachedThumbnail(Model model)
		{
			int hash = model.Id.GetHashCode();
			
			if (Thumbnails.TryGetValue(hash, out ThumbnailData thumbnailData))
				return thumbnailData;

			// Создание нового объекта
			thumbnailData = new ThumbnailData(model);
			Thumbnails.Add(hash, thumbnailData);
			
			// Загрузка мета-данных для цвета
			LoadMeta(thumbnailData);
			
			// Загрузка миниатюры
			LoadThumbnail(thumbnailData);

			return thumbnailData;
		}
		private static void LoadMeta(ThumbnailData thumbnailData)
		{
			ExternalResourcesManager.LoadModelMeta(thumbnailData.Model,
				modelMeta =>
				{
					thumbnailData.Color = modelMeta.GetColor();
				},
				error =>
				{
					Debug.LogError($"Failed to load model meta: {error}.");
				}
			);
		}
		private static void LoadThumbnail(ThumbnailData thumbnailData)
		{
			ExternalResourcesManager.LoadModelThumbnail(thumbnailData.Model,
				texture =>
				{
					thumbnailData.Texture = texture;
				},
				error =>
				{
					Debug.LogError($"Failed to load model thumbnail: {error}.");
				}
			);
		}

		
		private class ThumbnailData
		{
			public readonly Model Model;

			public Texture2D Texture
			{
				get => m_Texture;
				set
				{
					m_Texture = value;
					
					if (IsReady)
						OnLoaded?.Invoke();
				}
			}
			private Texture2D m_Texture;

			public Color Color
			{
				get => m_Color;
				set
				{
					m_Color = value;
					if(IsReady)
						OnLoaded?.Invoke();
				}
			}
			private Color m_Color;
			
			public ushort UsageCount
			{
				get => m_UsageCount;
				set
				{
					m_UsageCount = value;
					if (m_UsageCount == 0)
						Unload();
				}
			}
			private ushort m_UsageCount;
			
			// Ивенты
			public event Action OnLoaded;
			
			public bool IsReady => m_Texture != null && m_Color.a > 0;
			
			
			public ThumbnailData(Model model) => Model = model;

			public void Unload()
			{
				Destroy(Texture);
				
				Thumbnails.Remove(Model.Id.GetHashCode());
				Debug.Log($"Thumbnail for model {Model.Id} unloaded.");
			}
		}
	}
}