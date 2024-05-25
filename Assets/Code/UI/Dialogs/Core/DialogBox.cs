using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Dialogs.Core
{
	public abstract class DialogBox : MonoBehaviour
	{
		[Header("Events")]
		public UnityEvent OnShow = new UnityEvent();
		public UnityEvent OnHide = new UnityEvent();
		public UnityEvent OnClose = new UnityEvent();
		
		public GameObject Root => gameObject;
		public RectTransform RootTransform { get; private set; }
		public bool IsVisible { get; private set; }
		
		[Header("Components")]
		[SerializeField] protected Image Background;
		[SerializeField] protected CanvasGroup Content;

		private bool m_IsInitialized;
		
		
		public void Initialize(bool hideAfter)
		{
			if (m_IsInitialized)
				throw new Exception();
			
			RootTransform = (RectTransform)Root.transform;

			if (hideAfter)
			{
				Root.SetActive(false);
				IsVisible = false;
			}

			m_IsInitialized = true;
		}
		
		public virtual void Show(object[] args)
		{
			if(!m_IsInitialized)
				Initialize(false);
			
			Root.SetActive(true);
			LayoutRebuilder.ForceRebuildLayoutImmediate(RootTransform);

			#region Animation

			Color backgroundColor = Background.color;
			backgroundColor.a = 0.0f;
			Background.color = backgroundColor;
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