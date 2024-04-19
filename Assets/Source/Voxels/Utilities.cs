using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Voxels.Components;
using Voxels.Core;
using Debug = UnityEngine.Debug;

namespace Voxels
{
	public static class Utilities
	{
		public static Voxel Raycast(VoxelModelBehaviour voxelModelBehaviour, Ray ray)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			Debug.DrawLine(ray.origin, ray.GetPoint(100.0f), Color.yellow);
			
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
				
				DebugUtilities.DrawBounds(bounds, Color.red, 5.0f);
				
				intersectChunksData.Add((chunk, distance));
			}
			
			// Сортировка чанков по дистанции
			intersectChunksData.Sort((a, b) => a.distance.CompareTo(b.distance));
			
			// Перебор чанков
			foreach ((Chunk chunk, _) in intersectChunksData)
			{
				for (uint x = 0; x < Constants.CHUNK_SIZE; x++)
				for (uint y = 0; y < Constants.CHUNK_SIZE; y++)
				for (uint z = 0; z < Constants.CHUNK_SIZE; z++)
				{
					Voxel voxel = chunk.GetVoxel(x, y, z);
					if (voxel.Type == VoxelType.Air)
						continue;
					
					Bounds bounds = GetVoxelBounds(x, y, z, modelTransform);
					if (!bounds.IntersectRay(ray, out _))
						continue;
					
					Debug.Log($"Raycast took {stopwatch.ElapsedMilliseconds} ms");
					return voxel;
				}
			}

			Debug.Log($"Raycast took {stopwatch.ElapsedMilliseconds} ms");
			return null;
		}
		
		private static Bounds GetChunkBounds(Chunk chunk, Transform transform)
		{
			float chunkUnitsSize = Constants.CHUNK_SIZE * Constants.VOXEL_SIZE;
			Vector3 localCenter = new Vector3(
				(chunk.X + 0.5f) * chunkUnitsSize,
				(chunk.Y + 0.5f) * chunkUnitsSize,
				(chunk.Z + 0.5f) * chunkUnitsSize);
			return new Bounds(localCenter, new Vector3(chunkUnitsSize, chunkUnitsSize, chunkUnitsSize));
		}
		private static Bounds GetVoxelBounds(uint x, uint y, uint z, Transform transform)
		{
			float voxelUnitsSize = Constants.VOXEL_SIZE;
			Vector3 localCenter = new Vector3(
				(x + 0.5f) * voxelUnitsSize,
				(y + 0.5f) * voxelUnitsSize,
				(z + 0.5f) * voxelUnitsSize);
			return new Bounds(localCenter, new Vector3(voxelUnitsSize, voxelUnitsSize, voxelUnitsSize));
		}
	}
}