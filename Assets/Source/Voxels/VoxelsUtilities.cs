using System.Collections.Generic;
using UnityEngine;
using Voxels.Components;
using Voxels.Core;

namespace Voxels
{
	public static class VoxelsUtilities
	{
		public static Voxel Raycast(VoxelModelBehaviour voxelModelBehaviour, Ray ray)
		{
			Transform modelTransform = voxelModelBehaviour.transform;
			Model model = voxelModelBehaviour.Model;
			
			ray.origin = modelTransform.InverseTransformPoint(ray.origin);
			ray.direction = modelTransform.InverseTransformDirection(ray.direction);
			
			// Получение чанков, которые пересекает луч
			List<(Chunk chunk, float distance)> intersectChunksData = new();
			foreach (Chunk chunk in model.Chunks)
			{
				if (chunk == null)
					continue;
				
				Bounds bounds = GetChunkBounds(chunk, modelTransform);
				if (!bounds.IntersectRay(ray, out float distance))
					continue;
				
				intersectChunksData.Add((chunk, distance));
			}
			
			// Сортировка чанков по дистанции
			intersectChunksData.Sort((a, b) => a.distance.CompareTo(b.distance));
			
			// Перебор чанков
			foreach ((Chunk chunk, _) in intersectChunksData)
			{
				Voxel intersectedVoxel = null;
				float minDistance = float.MaxValue;
				
				for (uint x = 0; x < Constants.CHUNK_SIZE; x++)
				for (uint y = 0; y < Constants.CHUNK_SIZE; y++)
				for (uint z = 0; z < Constants.CHUNK_SIZE; z++)
				{
					Voxel voxel = chunk.GetVoxel(x, y, z);
					if (voxel.Type == VoxelType.Air)
						continue;
					
					Bounds bounds = GetVoxelBounds(chunk.GetGlobalVoxelCoordinates(x, y, z), modelTransform);
					if (!bounds.IntersectRay(ray, out float distance))
						continue;

					if (!(distance < minDistance))
						continue;

					minDistance = distance;
					intersectedVoxel = voxel;
				}

				if (intersectedVoxel != null)
					return intersectedVoxel;
			}

			return null;
		}
		private static Bounds GetChunkBounds(Chunk chunk, Transform transform)
		{
			float chunkUnitsSize = Constants.CHUNK_SIZE * Constants.VOXEL_SIZE;
			Vector3 localCenter = new Vector3(
				(chunk.Location.X + 0.5f) * chunkUnitsSize,
				(chunk.Location.Y + 0.5f) * chunkUnitsSize,
				(chunk.Location.Z + 0.5f) * chunkUnitsSize);
			return new Bounds(localCenter, new Vector3(chunkUnitsSize, chunkUnitsSize, chunkUnitsSize));
		}
		private static Bounds GetVoxelBounds(VoxelVector location, Transform transform)
		{
			float voxelUnitsSize = Constants.VOXEL_SIZE;
			Vector3 localCenter = new Vector3(
				(location.X + 0.5f) * voxelUnitsSize,
				(location.Y + 0.5f) * voxelUnitsSize,
				(location.Z + 0.5f) * voxelUnitsSize);
			return new Bounds(localCenter, new Vector3(voxelUnitsSize, voxelUnitsSize, voxelUnitsSize));
		}
		
		
		public static Vector3 GetVoxelWorldPosition(VoxelModelBehaviour voxelModelBehaviour, uint globalVoxelX, uint globalVoxelY, uint globalVoxelZ)
		{
			Transform modelTransform = voxelModelBehaviour.transform;
			Model model = voxelModelBehaviour.Model;
			
			return modelTransform.TransformPoint(new Vector3(
				(globalVoxelX + 0.5f) * Constants.VOXEL_SIZE,
				(globalVoxelY + 0.5f) * Constants.VOXEL_SIZE,
				(globalVoxelZ + 0.5f) * Constants.VOXEL_SIZE));
		}
		public static Vector3 GetVoxelWorldPosition(VoxelModelBehaviour voxelModelBehaviour, VoxelVector globalVoxelCoordinates) => GetVoxelWorldPosition(voxelModelBehaviour, globalVoxelCoordinates.X, globalVoxelCoordinates.Y, globalVoxelCoordinates.Z);
	}
}