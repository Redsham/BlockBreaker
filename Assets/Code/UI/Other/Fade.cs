using System;
using UnityEngine;

namespace UI.Other
{
	public class Fade : MonoBehaviour
	{
		private const string PREFAB_PATH = "UI/Other/Fade";
		private static Fade Prefab;
		
		
		private CanvasGroup m_CanvasGroup;
		private int m_FadeTweenId;
		
		[Bootstrapping.BootstrapMethod]
		public static void Initialize()
		{
			Prefab = Resources.Load<Fade>(PREFAB_PATH);
			
			if (Prefab == null)
				throw new Exception($"[Fade] Prefab not found at path: {PREFAB_PATH}");
		}
		public static void Show(Action onComplete)
		{
			Fade fade = Instantiate(Prefab);
			fade.Play(onComplete);
		}

		private void Awake()
		{
			m_CanvasGroup = GetComponent<CanvasGroup>();
			m_CanvasGroup.alpha = 0.0f;
			
			DontDestroyOnLoad(gameObject);
		}
		private void OnDestroy() => LeanTween.cancel(m_FadeTweenId);

		private void Play(Action onComplete)
		{
			LTSeq sequence = LeanTween.sequence();
			m_FadeTweenId = sequence.id;
			
			sequence.append(LeanTween.alphaCanvas(m_CanvasGroup, 1.0f, 0.5f).setEaseInBack());
			sequence.append(() =>
			{
				onComplete?.Invoke();
			});
			sequence.append(LeanTween.alphaCanvas(m_CanvasGroup, 0.0f, 0.5f).setEaseOutBack());
			sequence.append(() =>
			{
				Destroy(gameObject);
			});
		}
	}
}