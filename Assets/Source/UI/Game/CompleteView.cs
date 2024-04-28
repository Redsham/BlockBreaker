using UnityEngine;

namespace UI.Game
{
	public class CompleteView : MonoBehaviour
	{
		[SerializeField] private GameObject m_Root;

		public void Show() => m_Root.SetActive(true);
		public void Hide() => m_Root.SetActive(false);
	}
}