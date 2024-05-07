using System;
using ExternalResources;
using ExternalResources.Data;
using Gameplay;
using UI.Dialogs.Core;
using UI.Other;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Home
{
	public class ModelDialog : DialogBox
	{
		[SerializeField] private ModelThumbnail m_Thumbnail;
		[SerializeField] private HoldingButton m_UnlockButton;
		[SerializeField] private AdvancedButton m_PlayButton;
		
		[Header("DialogBox Events")]
		[SerializeField] public UnityEvent<Model> OnPlay;
		
		private Model m_Model;
		private ModelMeta m_Meta;

		
		private void Awake() => Initialize();
		public void Show(Model modelId)
		{
			m_Model = modelId;
			
			gameObject.SetActive(true);
			m_Thumbnail.SetModel(modelId);
			
			if (UserStats.IsModelUnlocked(modelId.Id))
			{
				m_UnlockButton.gameObject.SetActive(false);
				m_PlayButton.gameObject.SetActive(true);
			}
			else
			{
				m_UnlockButton.gameObject.SetActive(true);
				m_PlayButton.gameObject.SetActive(false);
				
				UserStats.OnModelUnlocked += modelId =>
				{
					if (modelId != m_Model.Id)
						return;

					m_UnlockButton.gameObject.SetActive(false);
					m_PlayButton.gameObject.SetActive(true);
				};
			}
			
			ExternalResourcesManager.LoadModelMeta(m_Model, meta =>
			{
				m_Meta = meta;
			});
			
			base.Show(null);
		}

		public void UnlockModel()
		{
			if(UserStats.Moneys < m_Meta.Cost)
				return;
			
			UserStats.Moneys -= m_Meta.Cost;
			UISounds.Play("buy");
			UserStats.UnlockModel(m_Model.Id);
		}
		public void Play() => OnPlay?.Invoke(m_Model);
	}
}