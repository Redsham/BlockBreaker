namespace Voxels.Core
{
	public class Model
	{
		private readonly Chunk[,,] m_Chunks;
		
		public readonly uint ChunksX;
		public readonly uint ChunksY;
		public readonly uint ChunksZ;

		public readonly uint SizeX;
		public readonly uint SizeY;
		public readonly uint SizeZ;

		private bool m_VoxelsNeighborsBuilded;

		public Model(uint sizeX, uint sizeY, uint sizeZ)
		{
			SizeX = sizeX + 1;
			SizeY = sizeY + 1;
			SizeZ = sizeZ + 1;
			
			ChunksX = (uint)(sizeX / Constants.CHUNK_SIZE + (sizeX % Constants.CHUNK_SIZE == 0 ? 0 : 1));
			ChunksY = (uint)(sizeY / Constants.CHUNK_SIZE + (sizeY % Constants.CHUNK_SIZE == 0 ? 0 : 1));
			ChunksZ = (uint)(sizeZ / Constants.CHUNK_SIZE + (sizeZ % Constants.CHUNK_SIZE == 0 ? 0 : 1));
			
			m_Chunks = new Chunk[ChunksX, ChunksY, ChunksZ];
			
			for (uint x = 0; x < ChunksX; x++)
				for (uint y = 0; y < ChunksY; y++)
					for (uint z = 0; z < ChunksZ; z++)
						m_Chunks[x, y, z] = new Chunk(x, y, z);
		}


		public void Apply()
		{
			CleanUpEmptyChunks();
			BuildVoxelsNeighbors();
		}
		public void CleanUpEmptyChunks()
		{
			bool isDirty = false;
			
			for (uint x = 0; x < ChunksX; x++)
				for (uint y = 0; y < ChunksY; y++)
					for (uint z = 0; z < ChunksZ; z++)
						if (m_Chunks[x, y, z].IsEmpty())
						{
							m_Chunks[x, y, z] = null;
							isDirty = true;
						}
			
			if(isDirty)
			{
				m_VoxelsNeighborsBuilded = false;
				BuildVoxelsNeighbors();
			}
		}
		public void BuildVoxelsNeighbors()
		{
			if(m_VoxelsNeighborsBuilded)
				return;
			
			for (uint i = 0; i < ChunksX; i++)
			for (uint j = 0; j < ChunksY; j++)
			for (uint k = 0; k < ChunksZ; k++)
			{
				Chunk chunk = m_Chunks[i, j, k];
				
				if(chunk == null) 
					continue;
				
				for (uint x = 0; x < Constants.CHUNK_SIZE; x++)
				for (uint y = 0; y < Constants.CHUNK_SIZE; y++)
				for (uint z = 0; z < Constants.CHUNK_SIZE; z++)
				{
					Voxel voxel = chunk.Voxels[x, y, z];
									
					uint globalX = i * Constants.CHUNK_SIZE + x;
					uint globalY = j * Constants.CHUNK_SIZE + y;
					uint globalZ = k * Constants.CHUNK_SIZE + z;
									
					voxel.Neighbors[0] = globalX > 0 ? GetVoxel(globalX - 1, globalY, globalZ) : null;
					voxel.Neighbors[1] = globalX < ChunksX * Constants.CHUNK_SIZE - 1 ? GetVoxel(globalX + 1, globalY, globalZ) : null;
					voxel.Neighbors[2] = globalY > 0 ? GetVoxel(globalX, globalY - 1, globalZ) : null;
					voxel.Neighbors[3] = globalY < ChunksY * Constants.CHUNK_SIZE - 1 ? GetVoxel(globalX, globalY + 1, globalZ) : null;
					voxel.Neighbors[4] = globalZ > 0 ? GetVoxel(globalX, globalY, globalZ - 1) : null;
					voxel.Neighbors[5] = globalZ < ChunksZ * Constants.CHUNK_SIZE - 1 ? GetVoxel(globalX, globalY, globalZ + 1) : null;
				}
			}

			m_VoxelsNeighborsBuilded = true;
		}
		public Chunk GetChunk(uint x, uint y, uint z)
		{
			if (x >= ChunksX || y >= ChunksY || z >= ChunksZ)
				return null;
			
			return m_Chunks[x, y, z];
		}
		public Voxel GetVoxel(uint globalX, uint globalY, uint globalZ)
		{
			uint chunkX = globalX / Constants.CHUNK_SIZE;
			uint chunkY = globalY / Constants.CHUNK_SIZE;
			uint chunkZ = globalZ / Constants.CHUNK_SIZE;
			
			if (chunkX >= ChunksX || chunkY >= ChunksY || chunkZ >= ChunksZ)
				return null;

			Chunk chunk = m_Chunks[chunkX, chunkY, chunkZ];
			if (chunk == null)
				return null;
			
			uint localX = globalX % Constants.CHUNK_SIZE;
			uint localY = globalY % Constants.CHUNK_SIZE;
			uint localZ = globalZ % Constants.CHUNK_SIZE;
			
			return chunk.Voxels[localX, localY, localZ];
		}
	}
}