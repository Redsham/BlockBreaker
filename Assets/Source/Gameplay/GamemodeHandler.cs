﻿using UnityEngine;
using Voxels.Components;

namespace Gameplay
{
	public class GamemodeHandler : MonoBehaviour
	{
		public static Session Session { get; set; }
		
		public GamemodeBase Gamemode { get; private set; }
		public VoxelModelBehaviour Model => m_ModelBehaviour;
		public PlayerController PlayerController => m_PlayerController;
		
		[SerializeField] private VoxelModelBehaviour m_ModelBehaviour;
		[SerializeField] private PlayerController m_PlayerController;


		private void Start()
		{
			InitGamemode(Session);
			m_PlayerController.OnTap.AddListener(() => Gamemode.OnTap());
		}
		private void InitGamemode(Session session)
		{
			Gamemode = session.Gamemode;
			Gamemode.Init(this);
			
			Model.SetModel(Gamemode.PrepareModel(session.Model), session.Model.Palette);
		}
	}
}