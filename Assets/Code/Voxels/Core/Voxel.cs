using Random = UnityEngine.Random;

namespace Voxels.Core
{
	public class Voxel
	{
		/// <summary>
		/// Тип вокселя
		/// </summary>
		public VoxelType Type;
		/// <summary>
		/// Индекс цвета вокселя в палитре
		/// </summary>
		public byte ColorIndex;
		/// <summary>
		/// Индекс шума вокселя в палитре
		/// </summary>
		public readonly byte NoiseIndex = 0;

		/// <summary>
		/// Чанк, в котором находится воксель
		/// </summary>
		public readonly Chunk Chunk;
		/// <summary>
		/// Соседние воксели
		/// </summary>
		public readonly Voxel[] Neighbors = new Voxel[6];


		public Voxel(Chunk chunk)
		{
			Chunk = chunk;
			NoiseIndex = (byte)Random.Range(0, Constants.NOISE_DEPTH);
		}

		public override string ToString() => $"Voxel({Type}, {ColorIndex})";
	}
}