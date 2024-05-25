using UI.Dialogs.Core;
using UnityEngine;

namespace UI.MainMenu
{
	public class AuthorView : DialogBox
	{
		[Header("Socials")]
		[SerializeField] private Social[] m_Socials;
		[SerializeField] private AdvancedButton m_SocialBttnTemplate;

		private void Awake()
		{
			Initialize();
			
			FillSocial();
			m_SocialBttnTemplate.gameObject.SetActive(false);
		}
		
		private void FillSocial()
		{
			foreach (Social social in m_Socials)
			{
				AdvancedButton bttn = Instantiate(m_SocialBttnTemplate, m_SocialBttnTemplate.transform.parent);
				bttn.Background.sprite = social.Icon;
				bttn.OnClick.AddListener(() => Application.OpenURL(social.URL));
			}
		}


		[System.Serializable]
		public struct Social
		{
			public string URL;
			public Sprite Icon;
		}
	}
}