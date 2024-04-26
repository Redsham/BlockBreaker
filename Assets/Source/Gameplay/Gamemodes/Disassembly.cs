using System.Collections.Generic;
using UnityEngine;
using Voxels;
using Voxels.Core;

namespace Gameplay.Gamemodes
{
	public class Disassembly : GamemodeBase
	{
		public override Model PrepareModel(ModelAsset asset)
		{
			// Создание модели
			Model model = new Model(asset.Width, asset.Height, asset.Depth);
			
			// Заполнение модели вокселями
			foreach (ModelAsset.Voxel assetVoxel in asset.Voxels)
			{
				Voxel voxel = model.GetVoxel(assetVoxel.X, assetVoxel.Y, assetVoxel.Z);
				
				voxel.Type = VoxelType.Breakable;
				voxel.ColorIndex = assetVoxel.ColorIndex;
			}
			
			// Применение изменений
			model.CleanUpEmptyChunks();
			model.BuildVoxelsNeighbors();

			return model;
		}
		public override void OnTap()
		{
			// Создание луча
			Ray ray = Handler.PlayerController.Camera.ScreenPointToRay(Input.GetTouch(0).position);
			Voxel voxel = VoxelsUtilities.Raycast(Handler.Model, ray);
			
			// Проверка на попадание в воксель
			if (voxel == null)
				return;
			
			// Удаление вокселя
			voxel.Type = VoxelType.Air;
			
			// Получение затронутых чанков
			HashSet<Chunk> chunks = new(6);
			foreach(Voxel neighbour in voxel.Neighbors)
				if(neighbour != null)
					chunks.Add(neighbour.Chunk);
			
			// Обновление чанков
			foreach (Chunk chunk in chunks)
				Handler.Model.GetRendererByChunk(chunk).Rebuild();
		}
	}
}