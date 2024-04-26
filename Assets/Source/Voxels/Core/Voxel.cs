using Random = UnityEngine.Random;

namespace Voxels.Core
{
	public class Voxel
	{
		public VoxelType Type;
		public byte ColorIndex;
		public readonly byte NoiseIndex = 0;

		public readonly Chunk Chunk;
		public readonly Voxel[] Neighbors = new Voxel[6];


		public Voxel(Chunk chunk)
		{
			Chunk = chunk;
			NoiseIndex = (byte)Random.Range(0, Constants.NOISE_DEPTH);
		}

		public override string ToString() => $"Voxel({Type}, {ColorIndex})";
	}
}