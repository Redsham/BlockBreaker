using UnityEngine;

namespace UI.Audio
{
	public class UISoundPlayer : MonoBehaviour
	{
		[SerializeField] private string m_SoundName = "click";
		
		public void Play() => UISounds.Play(m_SoundName);
		public void Play(string soundName) => UISounds.Play(soundName);
	}
}