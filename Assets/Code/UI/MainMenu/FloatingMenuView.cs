using System;
using Gameplay;
using TMPro;
using UnityEngine;

namespace UI.MainMenu
{
	public class FloatingMenuView : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI m_VoxelsText;
		private int m_LeanTweenVoxelsId;

		private void Start()
		{
			InitVoxelsText();
		}
		
		private void InitVoxelsText()
		{
			m_VoxelsText.text = UserStats.Moneys.ToString("000");
			
			UserStats.OnMoneysChanged += (oldMoneys, newMoneys) =>
			{
				LeanTween.cancel(m_LeanTweenVoxelsId);
				
				m_LeanTweenVoxelsId = LeanTween.value(m_VoxelsText.gameObject, 0.0f, 1.0f, 0.5f)
					.setOnUpdate((float value) =>
					{
						m_VoxelsText.text = Mathf.Lerp(oldMoneys, newMoneys, value).ToString("000");
					}).id;
			};
		}

		public void ShowAds()
		{
			// TODO: Сделать показ рекламы для получения награды
		}
		public void OpenSettings()
		{
			// TODO: Сделать открытие настроек
		}
		public void OpenAbout()
		{
			// TODO: Сделать открытие окна "О программе"
		}
	}
}