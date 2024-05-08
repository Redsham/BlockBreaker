using System;
using ExternalResources;
using ExternalResources.Data;
using Gameplay;
using TMPro;
using UI.Audio;
using UI.Dialogs.Core;
using UI.Other;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.MainMenu
{
	public class ModelDialog : DialogBox
	{
		[SerializeField] private ModelThumbnail m_Thumbnail;
		[SerializeField] private HoldingButton m_UnlockButton;
		[SerializeField] private AdvancedButton m_PlayButton;
		
		[SerializeField] private TextMeshProUGUI m_Cost;
		[SerializeField] private CanvasGroup m_CostGroup;
		
		[SerializeField] private TextMeshProUGUI m_Author;
		[SerializeField] private Image m_AuthorBackground;
		
		[Header("DialogBox Events")]
		[SerializeField] public UnityEvent<Model> OnPlay;
		
		private Model m_Model;
		private ModelMeta m_Meta;
		
		private int m_LeanTweenBuy;

		
		private void Awake() => Initialize();
		private void OnDestroy() => LeanTween.cancel(m_LeanTweenBuy);

		public void Show(Model modelId)
		{
			m_Model = modelId;
			
			gameObject.SetActive(true);
			m_Thumbnail.SetModel(modelId);
			
			bool isUnlocked = UserStats.IsModelUnlocked(modelId.Id);
			
			m_CostGroup.gameObject.SetActive(!isUnlocked);
			m_CostGroup.alpha = 1.0f;
			
			if (isUnlocked)
			{
				m_UnlockButton.gameObject.SetActive(false);
				m_PlayButton.gameObject.SetActive(true);
			}
			else
			{
				m_UnlockButton.gameObject.SetActive(true);
				m_PlayButton.gameObject.SetActive(false);
			}
			
			ExternalResourcesManager.LoadModelMeta(m_Model, meta =>
			{
				m_Meta = meta;
				m_Cost.text = meta.Cost.ToString();
				
				m_Author.text = meta.Author;
				m_AuthorBackground.color = meta.GetColor();
				
				LayoutRebuilder.ForceRebuildLayoutImmediate(m_CostGroup.transform as RectTransform);
			});
			
			base.Show(null);
		}
		public override void Hide()
		{
			LeanTween.cancel(m_LeanTweenBuy);
			base.Hide();
		}

		public void UnlockModel()
		{
			if(UserStats.Moneys < m_Meta.Cost)
				return;
			
			UserStats.Moneys -= m_Meta.Cost;
			UISounds.Play("buy");
			UserStats.UnlockModel(m_Model.Id);

			LTSeq sequence = LeanTween.sequence();
			sequence.append(LeanTween.value(gameObject, 0.0f, 1.0f, 0.5f).setOnUpdate((float value) =>
			{
				m_Cost.text = Mathf.Lerp(m_Meta.Cost, 0, value).ToString("F0");
			}));
			sequence.append(() =>
			{
				m_UnlockButton.gameObject.SetActive(false);
				m_PlayButton.gameObject.SetActive(true);
			});
			sequence.append(LeanTween.alphaCanvas(m_CostGroup, 0.0f, 0.5f));
			sequence.append(() =>
			{
				m_CostGroup.gameObject.SetActive(false);
			});
			
			m_LeanTweenBuy = sequence.id;
		}
		public void Play() => OnPlay?.Invoke(m_Model);
	}
}