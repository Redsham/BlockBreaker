namespace Voxels.Core
{
	public class Model
	{
		public readonly Chunk[] Chunks;
		
		/// <summary>
		/// Количество чанков по осям
		/// </summary>
		public readonly VoxelVector ChunksSize;
		/// <summary>
		/// Размер модели по осям в вокселях
		/// </summary>
		public readonly VoxelVector VoxelsSize;

		private bool m_VoxelsNeighborsBuilt;

		
		public Model(uint sizeX, uint sizeY, uint sizeZ)
		{
			VoxelsSize = new VoxelVector(
				sizeX + 1, 
				sizeY + 1, 
				sizeZ + 1
			);

			ChunksSize = new VoxelVector(
				(uint)(sizeX / Constants.CHUNK_SIZE + (sizeX % Constants.CHUNK_SIZE == 0 ? 0 : 1)),
				(uint)(sizeY / Constants.CHUNK_SIZE + (sizeY % Constants.CHUNK_SIZE == 0 ? 0 : 1)),
				(uint)(sizeZ / Constants.CHUNK_SIZE + (sizeZ % Constants.CHUNK_SIZE == 0 ? 0 : 1))
			);
			
			Chunks = new Chunk[ChunksSize.X * ChunksSize.Y * ChunksSize.Z];
			
			for (uint x = 0; x < ChunksSize.X; x++)
				for (uint y = 0; y < ChunksSize.Y; y++)
					for (uint z = 0; z < ChunksSize.Z; z++)
						Chunks[GetChunkIndex(x, y, z)] = new Chunk(x, y, z);
		}

		
		/// <summary>
		/// Удаление пустых чанков
		/// </summary>
		public void CleanUpEmptyChunks()
		{
			bool isDirty = false;
			
			for (uint chunkX = 0; chunkX < ChunksSize.X; chunkX++)
				for (uint chunkY = 0; chunkY < ChunksSize.Y; chunkY++)
				for (uint chunkZ = 0; chunkZ < ChunksSize.Z; chunkZ++)
				{
					uint index = GetChunkIndex(chunkX, chunkY, chunkZ);
					if (!Chunks[index].IsEmpty())
						continue;

					Chunks[index] = null;
					isDirty = true;
				}

			if (!isDirty)
				return;

			m_VoxelsNeighborsBuilt = false;
			BuildVoxelsNeighbors();
		}
		/// <summary>
		/// Построение связей между вокселями
		/// </summary>
		public void BuildVoxelsNeighbors()
		{
			if(m_VoxelsNeighborsBuilt)
				return;
			
			for (uint chunkX = 0; chunkX < ChunksSize.X; chunkX++)
			for (uint chunkY = 0; chunkY < ChunksSize.Y; chunkY++)
			for (uint chunkZ = 0; chunkZ < ChunksSize.Z; chunkZ++)
			{
				Chunk chunk = Chunks[GetChunkIndex(chunkX, chunkY, chunkZ)];
				
				if(chunk == null) 
					continue;
				
				for (uint voxelX = 0; voxelX < Constants.CHUNK_SIZE; voxelX++)
				for (uint voxelY = 0; voxelY < Constants.CHUNK_SIZE; voxelY++)
				for (uint voxelZ = 0; voxelZ < Constants.CHUNK_SIZE; voxelZ++)
				{
					Voxel voxel = chunk.GetVoxel(voxelX, voxelY, voxelZ);
									
					uint globalX = chunkX * Constants.CHUNK_SIZE + voxelX;
					uint globalY = chunkY * Constants.CHUNK_SIZE + voxelY;
					uint globalZ = chunkZ * Constants.CHUNK_SIZE + voxelZ;
									
					voxel.Neighbors[0] = globalX > 0 ? GetVoxel(globalX - 1, globalY, globalZ) : null;
					voxel.Neighbors[1] = globalX < ChunksSize.X * Constants.CHUNK_SIZE - 1 ? GetVoxel(globalX + 1, globalY, globalZ) : null;
					voxel.Neighbors[2] = globalY > 0 ? GetVoxel(globalX, globalY - 1, globalZ) : null;
					voxel.Neighbors[3] = globalY < ChunksSize.Y * Constants.CHUNK_SIZE - 1 ? GetVoxel(globalX, globalY + 1, globalZ) : null;
					voxel.Neighbors[4] = globalZ > 0 ? GetVoxel(globalX, globalY, globalZ - 1) : null;
					voxel.Neighbors[5] = globalZ < ChunksSize.Z * Constants.CHUNK_SIZE - 1 ? GetVoxel(globalX, globalY, globalZ + 1) : null;
				}
			}

			m_VoxelsNeighborsBuilt = true;
		}
		
		
		/// <summary>
		/// Получение индекса чанка по его координатам
		/// </summary>
		public uint GetChunkIndex(uint x, uint y, uint z) => x + y * ChunksSize.X + z * ChunksSize.X * ChunksSize.Y;
		/// <summary>
		/// Получение индекса чанка по его координатам
		/// </summary>
		public uint GetChunkIndex(VoxelVector voxelVector) => GetChunkIndex(voxelVector.X, voxelVector.Y, voxelVector.Z);
		
		
		/// <summary>
		/// Получение чанка по его координатам
		/// </summary>
		public Chunk GetChunk(uint x, uint y, uint z) => Chunks[GetChunkIndex(x, y, z)];
		/// <summary>
		/// Получение чанка по его координатам
		/// </summary>
		public Chunk GetChunk(VoxelVector voxelVector) => GetChunk(voxelVector.X, voxelVector.Y, voxelVector.Z);
		
		
		/// <summary>
		/// Получение вокселя по его глобальным координатам
		/// </summary>
		public Voxel GetVoxel(uint globalX, uint globalY, uint globalZ)
		{
			uint chunkX = globalX / Constants.CHUNK_SIZE;
			uint chunkY = globalY / Constants.CHUNK_SIZE;
			uint chunkZ = globalZ / Constants.CHUNK_SIZE;
			
			uint chunkIndex = GetChunkIndex(chunkX, chunkY, chunkZ);
			if (chunkIndex >= Chunks.Length)
				return null;

			Chunk chunk = Chunks[chunkIndex];
			if (chunk == null)
				return null;
			
			uint localX = globalX % Constants.CHUNK_SIZE;
			uint localY = globalY % Constants.CHUNK_SIZE;
			uint localZ = globalZ % Constants.CHUNK_SIZE;
			
			return chunk.GetVoxel(localX, localY, localZ);
		}
		/// <summary>
		/// Получение вокселя по его глобальным координатам
		/// </summary>
		public Voxel GetVoxel(VoxelVector voxelVector) => GetVoxel(voxelVector.X, voxelVector.Y, voxelVector.Z);
	}
}