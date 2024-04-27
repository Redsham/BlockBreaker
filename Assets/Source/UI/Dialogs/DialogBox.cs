using UnityEngine;
using UnityEngine.Events;

namespace UI.Dialogs
{
	public abstract class DialogBox : MonoBehaviour
	{
		public UnityEvent OnShow = new();
		public UnityEvent OnHide = new();
		public UnityEvent OnClose = new();
		
		public GameObject Root => gameObject;
		public RectTransform RootTransform { get; private set; }
		public bool IsVisible { get; private set; }
		
		
		public void Initialize()
		{
			RootTransform = (RectTransform)Root.transform;
			Root.SetActive(false);
			IsVisible = false;
		}
		
		public virtual void Show(object[] args)
		{
			// TODO: Анимация появления
			Root.SetActive(true);
			IsVisible = true;
			OnShow.Invoke();
		}
		public virtual void Hide()
		{
			// TODO: Анимация скрытия
			Root.SetActive(false);
			IsVisible = false;
			OnHide.Invoke();
		}
		public virtual void Close()
		{
			// TODO: Анимация закрытия
			IsVisible = false;
			OnClose.Invoke();
			Destroy(Root);
		}
	}
}