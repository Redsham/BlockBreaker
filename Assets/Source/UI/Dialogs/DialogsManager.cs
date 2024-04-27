using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI.Dialogs
{
	public static class DialogsManager
	{
		private static readonly Dictionary<Type, DialogBox> Templates;
		
		static DialogsManager()
		{
			Templates = new Dictionary<Type, DialogBox>();
			foreach (DialogBox dialogBox in Resources.LoadAll<DialogBox>("UI/Dialogs"))
				Templates.Add(dialogBox.GetType(), dialogBox);
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
			T dialogBox = (T)Object.Instantiate(Templates[typeof(T)], DialogsContainer.Active.Root);
			dialogBox.Initialize();
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