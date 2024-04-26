using Voxels;
using Voxels.Core;

namespace Gameplay.Gamemodes
{
	public class Antiquities : GamemodeBase
	{
		public override Model PrepareModel(ModelAsset asset)
		{
			Model model = new Model(asset.Width, asset.Height, asset.Depth);
			
			foreach (ModelAsset.Voxel assetVoxel in asset.Voxels)
			{
				Voxel voxel = model.GetVoxel(assetVoxel.X, assetVoxel.Y, assetVoxel.Z);
				
				voxel.Type = VoxelType.Unbreakable;
				voxel.ColorIndex = assetVoxel.ColorIndex;
			}
			
			model.Apply();

			return model;
		}
		public override void OnTap()
		{
			
		}
	}
}