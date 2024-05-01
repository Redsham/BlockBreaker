using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Dialogs.Core
{
	public abstract class DialogBox : MonoBehaviour
	{
		[Header("Events")]
		public UnityEvent OnShow = new();
		public UnityEvent OnHide = new();
		public UnityEvent OnClose = new();
		
		public GameObject Root => gameObject;
		public RectTransform RootTransform { get; private set; }
		public bool IsVisible { get; private set; }
		
		[Header("Components")]
		[SerializeField] protected Image Background;
		[SerializeField] protected CanvasGroup Content;
		
		
		public void Initialize()
		{
			RootTransform = (RectTransform)Root.transform;
			Root.SetActive(false);
			IsVisible = false;
		}
		
		public virtual void Show(object[] args)
		{
			Root.SetActive(true);
			LayoutRebuilder.ForceRebuildLayoutImmediate(RootTransform);

			#region Animation

			Background.color = Color.clear;
			LeanTween.alpha(Background.rectTransform, 0.9f, 0.1f);
			
			Content.alpha = 0.0f;
			LeanTween.alphaCanvas(Content, 1.0f, 0.1f)
				.setEaseInQuad()
				.setDelay(0.05f);
			
			RectTransform contentTransform = (RectTransform)Content.transform;
			contentTransform.anchoredPosition = new Vector2(0.0f, -100.0f);
			LeanTween.moveY(contentTransform, 0.0f, 0.1f)
				.setEaseInQuad()
				.setDelay(0.05f);

			#endregion
			
			IsVisible = true;
			OnShow.Invoke();
		}
		public virtual void Hide()
		{
			LeanTween.alpha(Background.rectTransform, 0.0f, 0.1f);
			LeanTween.alphaCanvas(Content, 0.0f, 0.1f).setEaseInQuad();
			LeanTween.moveY((RectTransform)Content.transform, -100.0f, 0.1f)
				.setEaseInQuad()
				.setOnComplete(() => Root.SetActive(false));
			
			IsVisible = false;
			OnHide.Invoke();
		}
		public virtual void Close()
		{
			LeanTween.alpha(Background.gameObject, 0.0f, 0.1f);
			LeanTween.alphaCanvas(Content, 0.0f, 0.1f).setEaseInQuad();
			LeanTween.moveY((RectTransform)Content.transform, -100.0f, 0.1f)
				.setEaseInQuad()
				.setOnComplete(() => Destroy(Root));
			
			IsVisible = false;
			OnClose.Invoke();
		}
	}
}