using System;
using System.Collections.Generic;
using System.IO;
using Bootstrapping;
using UnityEngine;

namespace Gameplay
{
	public static class UserStats
	{
		private static string FilePath => Path.Combine(Application.persistentDataPath, "UserStats.json");
		private static Data Current;

		
		/// <summary>
		/// Вызывается при изменении количества денег.
		/// </summary>
		public static event Action<uint> OnMoneysChanged;
		/// <summary>
		/// Вызывается при разблокировке модели.
		/// </summary>
		public static event Action<string> OnModelUnlocked;
		

		[BootstrapMethod]
		public static void Init()
		{
			if (File.Exists(FilePath))
			{
				try
				{
					Current = JsonUtility.FromJson<Data>(File.ReadAllText(FilePath));
					Debug.Log("[UserStats] User stats loaded.");
				}
				catch (Exception e)
				{
					Debug.LogError($"[UserStats] Failed to load user stats: {e}.");
					Current = new Data();
				}
			}
			else
			{
				Debug.Log("[UserStats] No user stats found, creating new.");
				Current = new Data();
			}
		}
		
		public static void UnlockModel(string id)
		{
			if (IsModelUnlocked(id))
			{
				Debug.LogWarning($"[UserStats] Model {id} is already unlocked.");
				return;
			}
			
			Current.UnlockedModels.Add(id.GetHashCode());
			OnModelUnlocked?.Invoke(id);
		}
		public static bool IsModelUnlocked(string id) => Current.UnlockedModels.Contains(id.GetHashCode());

		public static uint Moneys
		{
			get => Current.Moneys;
			set
			{
				Current.Moneys = value;
				OnMoneysChanged?.Invoke(value);
			}
		}

		
		private class Data
		{
			public uint Moneys = 1000;
			public HashSet<int> UnlockedModels = new HashSet<int>();
		}
	}
}