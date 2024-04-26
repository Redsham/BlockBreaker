namespace Voxels
{
	public static class Constants
	{
		/// <summary>
		/// Размер чанка в блоках
		/// </summary>
		public const uint  CHUNK_SIZE = 16;
		/// <summary>
		/// Размер блока в юнитах
		/// </summary>
		public const float VOXEL_SIZE = 0.1f;
		
		/// <summary>
		/// Количество вариантов шума на цвет
		/// </summary>
		public const int   NOISE_DEPTH = 8;
		/// <summary>
		/// Диапазон шума на цвет
		/// </summary>
		public const float NOISE_RANGE = 0.05f;
	}
}