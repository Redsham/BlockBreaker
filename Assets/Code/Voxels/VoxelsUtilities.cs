using System.Collections.Generic;
using UnityEngine;
using Voxels.Components;
using Voxels.Core;

namespace Voxels
{
	public static class VoxelsUtilities
	{
		public static VoxelHit Raycast(VoxelModelBehaviour voxelModelBehaviour, Ray ray)
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
				if (!bounds.IntersectRay(ray, out float intersectionDistance))
					continue;
				
				intersectChunksData.Add((chunk, intersectionDistance));
			}
			
			// Сортировка чанков по дистанции
			intersectChunksData.Sort((a, b) => a.distance.CompareTo(b.distance));
			
			// Перебор чанков
			foreach ((Chunk chunk, _) in intersectChunksData)
			{
				Voxel intersectedVoxel = null;
				VoxelVector intersectedVoxelLocation = VoxelVector.Zero;
				float minDistance = float.MaxValue;
				
				for (uint x = 0; x < Constants.CHUNK_SIZE; x++)
				for (uint y = 0; y < Constants.CHUNK_SIZE; y++)
				for (uint z = 0; z < Constants.CHUNK_SIZE; z++)
				{
					Voxel voxel = chunk.GetVoxel(x, y, z);
					if (voxel.Type == VoxelType.Air)
						continue;
					
					Bounds bounds = GetVoxelBounds(chunk.GetGlobalVoxelCoordinates(x, y, z), modelTransform);
					if (!bounds.IntersectRay(ray, out float intersectionDistance))
						continue;

					if (!(intersectionDistance < minDistance))
						continue;

					minDistance = intersectionDistance;
					intersectedVoxel = voxel;
					intersectedVoxelLocation = new VoxelVector(x, y, z);
				}

				if (intersectedVoxel != null)
					return VoxelHit.Hit(intersectedVoxel, intersectedVoxelLocation, minDistance);
			}

			return VoxelHit.NoHit;
		}
		public static Vector3 GetRaycastNormal(VoxelModelBehaviour voxelModelBehaviour, Ray ray, VoxelHit hit)
		{
			if (!hit.IsHit)
				return Vector3.zero;
			
			Transform modelTransform = voxelModelBehaviour.transform;
			
			Vector3 direction = (ray.GetPoint(hit.Distance) - GetVoxelWorldLocation(voxelModelBehaviour, hit.GlobalLocation)).normalized;
			Vector3 localDirection = modelTransform.InverseTransformDirection(direction);
			
			float maxDot = 0.0f;
			Vector3 localNormal = Vector3.zero;
			Vector3[] normals = new[]
			{
				Vector3.left, Vector3.right,
				Vector3.down, Vector3.up,
				Vector3.back, Vector3.forward
			};
			
			foreach (Vector3 n in normals)
			{
				float dot = Vector3.Dot(localDirection, n);
				if (!(dot > maxDot))
					continue;

				maxDot = dot;
				localNormal = n;
			}

			return modelTransform.TransformDirection(localNormal);
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
		
		public static Vector3 GetVoxelWorldLocation(VoxelModelBehaviour voxelModelBehaviour, uint globalVoxelX, uint globalVoxelY, uint globalVoxelZ)
		{
			Transform modelTransform = voxelModelBehaviour.transform;
			Model model = voxelModelBehaviour.Model;
			
			return modelTransform.TransformPoint(new Vector3(
				(globalVoxelX + 0.5f) * Constants.VOXEL_SIZE,
				(globalVoxelY + 0.5f) * Constants.VOXEL_SIZE,
				(globalVoxelZ + 0.5f) * Constants.VOXEL_SIZE));
		}
		public static Vector3 GetVoxelWorldLocation(VoxelModelBehaviour voxelModelBehaviour, VoxelVector globalVoxelCoordinates) => GetVoxelWorldLocation(voxelModelBehaviour, globalVoxelCoordinates.X, globalVoxelCoordinates.Y, globalVoxelCoordinates.Z);
		
		
		public struct VoxelHit
		{
			public bool IsHit { get; private set; }
			public Voxel Voxel { get; private set; }
			public VoxelVector LocalLocation { get; private set; }
			public float Distance { get; private set; }
			public VoxelVector GlobalLocation => Voxel.Chunk.GetGlobalVoxelCoordinates(LocalLocation.X, LocalLocation.Y, LocalLocation.Z);
			
			
			
			public static VoxelHit NoHit => new() { IsHit = false };
			public static VoxelHit Hit(Voxel voxel, VoxelVector location, float distance) => new() { IsHit = true, Voxel = voxel, LocalLocation = location, Distance = distance };
		}
	}
}