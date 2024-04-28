using TMPro;
using UI.Dialogs.Core;
using UnityEngine;

namespace UI.Dialogs.Implementations
{
	public class AlertBox : DialogBox
	{
		[SerializeField] private TextMeshProUGUI m_TitleText;
		[SerializeField] private TextMeshProUGUI m_BodyText;
		
		public override void Show(object[] args)
		{
			switch (args.Length)
			{
				case 1:
					if (args[0] is not string)
						throw new System.ArgumentException("Invalid argument type passed to the dialog.");

					m_TitleText.gameObject.SetActive(false);
					m_BodyText.text = (string)args[0];
					break;
				
				case 2:
					if (args[0] is not string || args[1] is not string)
						throw new System.ArgumentException("Invalid argument type passed to the dialog.");

					m_TitleText.text = (string)args[0];
					m_BodyText.text = (string)args[1];
					break;
				
				default:
					throw new System.ArgumentException("Invalid argument passed to the dialog.");
			}
			
			base.Show(args);
		}
	}
}