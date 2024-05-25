using System;
using System.Collections.Generic;
using TMPro;
using UI.Audio;
using UI.Dialogs.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Dialogs.Implementations
{
	public class AlertBox : DialogBox
	{
		[SerializeField] private TextMeshProUGUI     m_TitleText;
		[SerializeField] private TextMeshProUGUI     m_BodyText;
		[SerializeField] private Image               m_Icon;
		[SerializeField] private AdvancedButton      m_Button;
		
		[Header("Styles")]
		[SerializeField] private List<AlertBoxStyle> m_Styles;


		private void Awake()
		{
			m_Button.OnClick.AddListener(() =>
			{
				UISounds.Play("click");
				Close();
			});
		}

		public void Show(string title, string body, string style)
		{
			SetStyle(style);
			
			m_TitleText.text = title;
			m_BodyText.text = body;

			Show(null);
		}
		
		private void SetStyle(string styleName)
		{
			AlertBoxStyle style = m_Styles.Find(s => s.Name == styleName);

			bool hasIcon = style.Icon != null;
			m_Icon.transform.parent.gameObject.SetActive(hasIcon);
			if (hasIcon)
			{
				m_Icon.sprite = style.Icon;
				m_Icon.color = style.MainColor;
			}
			
			m_Button.Background.color = style.MainColor;
			
			m_TitleText.horizontalAlignment = hasIcon ? HorizontalAlignmentOptions.Center : HorizontalAlignmentOptions.Left;
			m_BodyText.horizontalAlignment  = hasIcon ? HorizontalAlignmentOptions.Center : HorizontalAlignmentOptions.Left;
		}
		
		[System.Serializable]
		public struct AlertBoxStyle
		{
			public string Name;
			public Sprite Icon;
			public Color  MainColor;
		}
	}
}