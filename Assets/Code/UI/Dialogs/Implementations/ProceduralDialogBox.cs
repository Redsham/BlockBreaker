using System;
using TMPro;
using UI.Dialogs.Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Dialogs.Implementations
{
	public class ProceduralDialogBox : DialogBox
	{
		[SerializeField] private TextMeshProUGUI m_Title;
		[SerializeField] private TextMeshProUGUI m_Body;
		[SerializeField] private AdvancedButton  m_ButtonTemplate;
		[SerializeField] private RectTransform   m_ButtonsContainer;
		
		public void Show(string title, string body, ButtonsLayout layout, params Button[] buttons)
		{
			m_Title.text = title;
			m_Body.text = body;

			switch (layout)
			{
				case ButtonsLayout.Vertical:
					VerticalLayoutGroup verticalLayoutGroup = m_ButtonsContainer.AddComponent<VerticalLayoutGroup>();
					verticalLayoutGroup.childControlWidth = true;
					verticalLayoutGroup.childControlHeight = false;
					verticalLayoutGroup.spacing = 10.0f;
					break;
				case ButtonsLayout.Horizontal:
					HorizontalLayoutGroup horizontalLayoutGroup = m_ButtonsContainer.AddComponent<HorizontalLayoutGroup>();
					horizontalLayoutGroup.childControlWidth = true;
					horizontalLayoutGroup.childControlHeight = false;
					horizontalLayoutGroup.spacing = 10.0f;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(layout), layout, null);
			}
			
			foreach (Transform child in m_ButtonsContainer)
				if(child != m_ButtonTemplate.transform)
					Destroy(child.gameObject);
			
			foreach (Button button in buttons)
				AddButton(button);
			
			base.Show(null);
		}
		
		private void AddButton(Button button)
		{
			AdvancedButton advancedButton = Instantiate(m_ButtonTemplate, m_ButtonsContainer);
			advancedButton.gameObject.SetActive(true);
			
			advancedButton.Text = button.Text;
			advancedButton.Background.color = button.Color;
			
			if (button.Thumbnail != null)
				advancedButton.Thumbnail = button.Thumbnail;
			else
				advancedButton.ThumbnailComponent.gameObject.SetActive(false);
			
			advancedButton.OnClick.AddListener(() => button.OnClick.Invoke(this));
		}
		
		public struct Button
		{
			public string Text;
			public Sprite Thumbnail;
			public Color  Color;
			public Action<ProceduralDialogBox> OnClick;
			
			public Button(string text, Sprite thumbnail, Color color, Action<ProceduralDialogBox> onClick)
			{
				Text = text;
				Color = color;
				Thumbnail = thumbnail;
				OnClick = onClick;
			}
		}
		public enum ButtonsLayout
		{
			Vertical,
			Horizontal
		}
	}
}