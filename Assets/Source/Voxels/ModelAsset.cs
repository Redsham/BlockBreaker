using UnityEngine;

namespace Voxels
{
	public class ModelAsset
	{
		/// <summary>
		/// Воксели модели
		/// </summary>
		public Voxel[] Voxels { get; private set; }
		/// <summary>
		/// Палитра цветов модели
		/// </summary>
		public Color32[] Palette { get; private set; }
		
		/// <summary>
		/// Размер модели по оси X в вокселях
		/// </summary>
		public uint Width { get; private set; }
		/// <summary>
		/// Размер модели по оси Y в вокселях
		/// </summary>
		public uint Height { get; private set; }
		/// <summary>
		/// Размер модели по оси Z в вокселях
		/// </summary>
		public uint Depth { get; private set; }
		
		
		public override string ToString() => $"ModelAsset(Size: {Width}x{Height}x{Depth} | Voxels: {Voxels.Length} | Palette: {Palette.Length})";
		
		
		/// <summary>
		/// Десериализация модели из байтового массива
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
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