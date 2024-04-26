using UnityEngine;
using Utilities;

namespace Voxels
{
	public class ModelAsset
	{
		public Voxel[] Voxels { get; private set; }
		public Color32[] Palette { get; private set; }
		
		public uint Width { get; private set; }
		public uint Height { get; private set; }
		public uint Depth { get; private set; }
		
		
		
		public static ModelAsset Deserialize(byte[] data)
		{
			ModelAsset asset = new();
			int offset = 0;
			
			#region Voxels

			uint voxelCount = BinaryHelper.ReadUInt32(data, ref offset);
			asset.Voxels = new Voxel[voxelCount];
			
			for (int i = 0; i < voxelCount; i++)
			{
				Voxel voxel = new()
				{
					X = BinaryHelper.ReadUInt32(data, ref offset),
					Y = BinaryHelper.ReadUInt32(data, ref offset),
					Z = BinaryHelper.ReadUInt32(data, ref offset),
					ColorIndex = BinaryHelper.ReadByte(data, ref offset)
				};

				asset.Voxels[i] = voxel;
			}

			#endregion

			#region Palette

			uint colorCount = BinaryHelper.ReadUInt32(data, ref offset);
			asset.Palette = new Color32[colorCount];
			
			for (int i = 0; i < colorCount; i++)
			{
				Color color = new()
				{
					r = BinaryHelper.ReadByte(data, ref offset) / 255.0f,
					g = BinaryHelper.ReadByte(data, ref offset) / 255.0f,
					b = BinaryHelper.ReadByte(data, ref offset) / 255.0f
				};
				
				asset.Palette[i] = color;
			}

			#endregion

			#region Dimensions

			asset.Width = BinaryHelper.ReadUInt32(data, ref offset);
			asset.Height = BinaryHelper.ReadUInt32(data, ref offset);
			asset.Depth = BinaryHelper.ReadUInt32(data, ref offset);

			#endregion
			
			return asset;
		}
		public override string ToString() => $"ModelAsset(Size: {Width}x{Height}x{Depth} | Voxels: {Voxels.Length} | Palette: {Palette.Length})";

		[System.Serializable]
		public struct Voxel
		{
			public uint X;
			public uint Y;
			public uint Z;
			
			public byte ColorIndex;
		}
	}
}