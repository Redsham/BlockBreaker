using UnityEngine;

namespace UI.Dialogs.Core
{
	public class DialogsContainer : MonoBehaviour
	{
		public static DialogsContainer Active
		{
			get
			{
				if (CachedActive == null)
				{
					Canvas canvas = FindObjectOfType<Canvas>();
					
					GameObject gameObject = new GameObject("Dialogs Container", typeof(RectTransform));
					RectTransform rectTransform = (RectTransform)gameObject.transform;
					
					rectTransform.SetParent(canvas.transform);
					rectTransform.anchorMin = Vector2.zero;
					rectTransform.anchorMax = Vector2.one;
					rectTransform.anchoredPosition = Vector2.zero;
					rectTransform.sizeDelta = Vector2.zero;
					rectTransform.localScale = Vector3.one;
					
					CachedActive = gameObject.AddComponent<DialogsContainer>();
				}

				return CachedActive;
			}
		}
		private static DialogsContainer CachedActive;
		
		public RectTransform Root { get; private set; }

		private void Awake()
		{
			CachedActive = this;
			Root = (RectTransform)transform;
		}
	}
}