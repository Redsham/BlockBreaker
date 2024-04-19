using UnityEngine;

namespace Voxels
{
	public class ModelAsset
	{
		public Voxel[] Voxels;
		public Color[] Palette;
		
		
		public (uint width, uint height, uint depth) GetSize()
		{
			uint width = 0;
			uint height = 0;
			uint depth = 0;
			
			foreach (Voxel voxel in Voxels)
			{
				if (voxel.X > width)
					width = voxel.X;
				
				if (voxel.Y > height)
					height = voxel.Y;
				
				if (voxel.Z > depth)
					depth = voxel.Z;
			}
			
			return (width, height, depth);
		}
		
		
		[System.Serializable]
		public struct Voxel
		{
			public uint X;
			public uint Y;
			public uint Z;
			
			public byte ColorIndex;
		}
		
		[System.Serializable]
		public struct Color
		{
			public byte R;
			public byte G;
			public byte B;
		}
	}
}