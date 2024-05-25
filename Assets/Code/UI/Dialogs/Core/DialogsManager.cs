using System;
using System.Collections.Generic;
using Bootstrapping;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI.Dialogs.Core
{
	public static class DialogsManager
	{
		private static readonly Dictionary<Type, DialogBox> Templates = new();
		
		[BootstrapMethod]
		public static void Bootstrap()
		{
			foreach (DialogBox dialogBox in Resources.LoadAll<DialogBox>("UI/Dialogs"))
				Templates.Add(dialogBox.GetType(), dialogBox);
			
			Debug.Log($"[DialogsManager] DialogsManager initialized with {Templates.Count} templates.");
		}

		/// <summary>
		/// Получает шаблон диалогового окна указанного типа.
		/// </summary>
		/// <exception cref="Exception">Шаблон диалогового окна указанного типа не найден.</exception>
		public static DialogBox GetTemplate<T>() where T : DialogBox
		{
			Type type = typeof(T);
			
			if(Templates.TryGetValue(type, out DialogBox dialogBox))
				return dialogBox;
			
			throw new Exception($"DialogBox template for type {type} not found.");
		}
		/// <summary>
		/// Создает диалоговое окно указанного типа.
		/// </summary>
		public static T CreateDialog<T>() where T : DialogBox
		{
			T dialogBox = (T)Object.Instantiate(GetTemplate<T>(), DialogsContainer.Active.Root);
			dialogBox.Initialize(true);
			return dialogBox;
		}
		/// <summary>
		/// Создает и открывает диалоговое окно указанного типа.
		/// </summary>
		/// <param name="args">Аргументы открытия диалогового окна.</param>
		public static T ShowDialog<T>(params object[] args) where T : DialogBox
		{
			T dialogBox = CreateDialog<T>();
			dialogBox.Show(args);
			return dialogBox;
		}
		/// <summary>
		/// Создает и открывает диалоговое окно указанного типа.
		/// </summary>
		public static T ShowDialog<T>() where T : DialogBox
		{
			T dialogBox = CreateDialog<T>();
			dialogBox.Show(null);
			return dialogBox;
		}
	}
}