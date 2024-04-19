namespace Voxels.Core
{
	public class Chunk
	{
		public readonly uint X;
		public readonly uint Y;
		public readonly uint Z;

		
		public readonly Voxel[,,] Voxels = new Voxel[Constants.CHUNK_SIZE, Constants.CHUNK_SIZE, Constants.CHUNK_SIZE];
		
		public Chunk(uint x, uint y, uint z)
		{
			X = x;
			Y = y;
			Z = z;
			
			for (uint i = 0; i < Constants.CHUNK_SIZE; i++)
				for (uint j = 0; j < Constants.CHUNK_SIZE; j++)
					for (uint k = 0; k < Constants.CHUNK_SIZE; k++)
						Voxels[i, j, k] = new Voxel(this);
		}
		
		public bool IsEmpty()
		{
			for (uint i = 0; i < Constants.CHUNK_SIZE; i++)
				for (uint j = 0; j < Constants.CHUNK_SIZE; j++)
					for (uint k = 0; k < Constants.CHUNK_SIZE; k++)
						if (Voxels[i, j, k].Type != VoxelType.Air)
							return false;
			
			return true;
		}
	}
}