using Gameplay;
using TMPro;
using UnityEngine;

namespace UI.MainMenu
{
	public class FloatingMenuView : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI m_VoxelsText;
		private int m_LeanTweenVoxelsId;

		[SerializeField] private AuthorView m_AuthorView;
		

		private void Start()
		{
			InitVoxelsText();
		}
		
		private void InitVoxelsText()
		{
			m_VoxelsText.text = (UserStats.Moneys > 999 ? 999 : UserStats.Moneys).ToString("000");
			
			UserStats.OnMoneysChanged += (oldMoneys, newMoneys) =>
			{
				LeanTween.cancel(m_LeanTweenVoxelsId);
				
				m_LeanTweenVoxelsId = LeanTween.value(m_VoxelsText.gameObject, 0.0f, 1.0f, 0.5f)
					.setOnUpdate((float value) =>
					{
						m_VoxelsText.text = Mathf.Clamp(Mathf.Lerp(oldMoneys, newMoneys, value), 0.0f, 999.0f).ToString("000");
					}).id;
			};
		}

		
		public void OpenSettings()
		{
			// TODO: Сделать открытие настроек
		}
		public void OpenAuthor() => m_AuthorView.Show(null);
	}
}