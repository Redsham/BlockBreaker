using System;
using UnityEngine;
using Voxels;
using Voxels.Components;

namespace Gameplay
{
	public class GamemodeHandler : MonoBehaviour
	{
		public GamemodeBase Gamemode { get; private set; }
		public VoxelModelBehaviour Model => m_ModelBehaviour;
		
		[SerializeField] private VoxelModelBehaviour m_ModelBehaviour;
			
		
		public void Init(GamemodeBase gamemode)
		{
			Gamemode = gamemode;
			Gamemode.Init(this);
		}
		public void SetModel(ModelAsset asset) => Model.SetModel(Gamemode.PrepareModel(asset), asset.Palette);
	}
}