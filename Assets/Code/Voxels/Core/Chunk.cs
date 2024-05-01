namespace Voxels.Core
{
	public class Chunk
	{
		public readonly VoxelVector Location;
		public readonly Voxel[] Voxels = new Voxel[Constants.CHUNK_SIZE * Constants.CHUNK_SIZE * Constants.CHUNK_SIZE];
		
		public Chunk(uint x, uint y, uint z)
		{
			Location = new VoxelVector(x, y, z);
			
			for(uint i = 0; i < Constants.CHUNK_SIZE * Constants.CHUNK_SIZE * Constants.CHUNK_SIZE; i++)
				Voxels[i] = new Voxel(this);
		}
		public Chunk(VoxelVector location) : this(location.X, location.Y, location.Z) { }
		
		
		/// <summary>
		/// Возвращает индекс вокселя по координатам в чанке
		/// </summary>
		/// <param name="localX"></param>
		/// <param name="localY"></param>
		/// <param name="localZ"></param>
		/// <returns></returns>
		public uint GetVoxelIndex(uint localX, uint localY, uint localZ) => localX + localY * Constants.CHUNK_SIZE + localZ * Constants.CHUNK_SIZE * Constants.CHUNK_SIZE;
		/// <summary>
		/// Возвращает воксель по координатам в чанке
		/// </summary>
		/// <param name="localX"></param>
		/// <param name="localY"></param>
		/// <param name="localZ"></param>
		/// <returns></returns>
		public Voxel GetVoxel(uint localX, uint localY, uint localZ) => Voxels[GetVoxelIndex(localX, localY, localZ)];
		
		/// <summary>
		/// Возвращает глобальные координаты вокселя по локальным
		/// </summary>
		/// <param name="localX"></param>
		/// <param name="localY"></param>
		/// <param name="localZ"></param>
		/// <returns></returns>
		public VoxelVector GetGlobalVoxelCoordinates(uint localX, uint localY, uint localZ) => Location * Constants.CHUNK_SIZE + new VoxelVector(localX, localY, localZ);
		
		/// <summary>
		/// Возвращает true, если все воксели в чанке пустые
		/// </summary>
		/// <returns></returns>
		public bool IsEmpty()
		{
			for (uint x = 0; x < Constants.CHUNK_SIZE; x++)
				for (uint y = 0; y < Constants.CHUNK_SIZE; y++)
					for (uint z = 0; z < Constants.CHUNK_SIZE; z++)
						if (GetVoxel(x, y, z).Type != VoxelType.Air)
							return false;
			
			return true;
		}

		public override string ToString() => $"Chunk({Location.X}, {Location.Y}, {Location.Z})";
	}
}