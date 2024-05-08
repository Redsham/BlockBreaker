using Gameplay;
using UnityEngine;

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
	}
}