using System.Collections;
using System.Collections.Generic;
using Bootstrapping;
using UnityEngine;

namespace UI
{
	public class UISounds : MonoBehaviour, IBootstrappble
	{
		public const int MAX_SOUNDS = 4;
		
		
		public int Order => 0;

		[SerializeField] private Sound[] m_Sounds;
		
		private static readonly Dictionary<string, Sound> SoundsMap = new();
		private static readonly AudioSource[] AudioSources = new AudioSource[MAX_SOUNDS];
		private static int CurrentSourceIndex;
		
		public IEnumerator Bootstrap()
		{
			foreach (Sound sound in m_Sounds)
				SoundsMap.Add(sound.Name, sound);
			m_Sounds = null;
			
			for (int i = 0; i < MAX_SOUNDS; i++)
			{
				AudioSource audioSource = gameObject.AddComponent<AudioSource>();
				AudioSources[i] = audioSource;
				
				audioSource.bypassEffects = true;
				audioSource.bypassListenerEffects = true;
				audioSource.bypassReverbZones = true;
				
				audioSource.volume = 1.0f;
				audioSource.pitch = 1.0f;
				audioSource.spatialBlend = 0.0f;
				
				audioSource.playOnAwake = false;
				audioSource.loop = false;

				audioSource.minDistance = audioSource.maxDistance = float.MaxValue;
			}
			
			DontDestroyOnLoad(gameObject);
			yield break;
		}
		
		
		public static void Play(string soundName)
		{
			if (!SoundsMap.TryGetValue(soundName, out Sound sound))
			{
				Debug.LogError($"Sound {soundName} not found.");
				return;
			}
			
			AudioSource audioSource = AudioSources[CurrentSourceIndex];
			audioSource.PlayOneShot(sound.Clip);
			
			CurrentSourceIndex = (CurrentSourceIndex + 1) % MAX_SOUNDS;
		}
		
		
		[System.Serializable]
		public struct Sound
		{
			public string Name => m_Name;
			public AudioClip Clip => m_Clip;
			
			[SerializeField] private string m_Name;
			[SerializeField] private AudioClip m_Clip;
		}
	}
}