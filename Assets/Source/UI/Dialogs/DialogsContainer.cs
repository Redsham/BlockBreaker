using UnityEngine;

namespace UI.Dialogs
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
					
					GameObject gameObject = new GameObject("DialogsContainer", typeof(RectTransform));
					RectTransform rectTransform = (RectTransform)gameObject.transform;
					
					rectTransform.SetParent(canvas.transform);
					rectTransform.anchorMin = Vector2.zero;
					rectTransform.anchorMax = Vector2.one;
					rectTransform.sizeDelta = Vector2.zero;
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