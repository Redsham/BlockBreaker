namespace Voxels.Core
{
	public class Chunk
	{
		public readonly uint X;
		public readonly uint Y;
		public readonly uint Z;

		
		public readonly Voxel[] Voxels = new Voxel[Constants.CHUNK_SIZE * Constants.CHUNK_SIZE * Constants.CHUNK_SIZE];
		
		public Chunk(uint x, uint y, uint z)
		{
			X = x;
			Y = y;
			Z = z;
			
			for(uint i = 0; i < Constants.CHUNK_SIZE * Constants.CHUNK_SIZE * Constants.CHUNK_SIZE; i++)
				Voxels[i] = new Voxel(this);
		}
		
		public uint GetVoxelIndex(uint x, uint y, uint z) => x + y * Constants.CHUNK_SIZE + z * Constants.CHUNK_SIZE * Constants.CHUNK_SIZE;
		public Voxel GetVoxel(uint x, uint y, uint z) => Voxels[GetVoxelIndex(x, y, z)];
		public bool IsEmpty()
		{
			for (uint x = 0; x < Constants.CHUNK_SIZE; x++)
				for (uint y = 0; y < Constants.CHUNK_SIZE; y++)
					for (uint z = 0; z < Constants.CHUNK_SIZE; z++)
						if (GetVoxel(x, y, z).Type != VoxelType.Air)
							return false;
			
			return true;
		}

		public override string ToString() => $"Chunk({X}, {Y}, {Z})";
	}
}