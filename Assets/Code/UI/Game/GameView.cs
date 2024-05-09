using Gameplay;
using UI.Dialogs.Core;
using UI.Dialogs.Implementations;
using UI.Other;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Game
{
	public class GameView : MonoBehaviour
	{
		[Header("Content")]
		[SerializeField] private GamemodeHandler m_GamemodeHandler;
		
		[Header("UI")]
		[SerializeField] private ProgressBar m_ProgressBar;

		
		private void Start() => m_GamemodeHandler.OnProgressChanged.AddListener(OnProgressChanged);
		private void OnProgressChanged(float value) => m_ProgressBar.Value = value;


		public void GoToMenu()
		{
			DialogsManager.CreateDialog<ProceduralDialogBox>().Show("Are you sure?", "You will lose your progress", ProceduralDialogBox.ButtonsLayout.Horizontal,
				new ProceduralDialogBox.Button
				{
					Text = "Yes",
					Color = Palette.Alizarin,
					OnClick = dialog =>
					{
						dialog.Close();
						Fade.Show(() => SceneManager.LoadScene("Menu"));
					}
				},
				new ProceduralDialogBox.Button
				{
					Text = "No",
					Color = Palette.Emerald,
					OnClick = dialog => dialog.Close()
				});
		}
	}
}