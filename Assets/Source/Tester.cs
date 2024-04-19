using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Voxels;
using Voxels.Core;
using Debug = UnityEngine.Debug;

public class Tester : MonoBehaviour
{
	[SerializeField] private Voxels.Components.VoxelModelBehaviour m_VoxelModel;
	[SerializeField] private string[] m_Paths;
	
	private void OnGUI()
	{
		GUILayout.BeginVertical();

		foreach (string path in m_Paths)
		{
			if (GUILayout.Button($"Load {path}"))
			{
				StopAllCoroutines();
				LoadModel(path);
			}
		}
		
		GUILayout.EndVertical();
	}

	private void LoadModel(string path)
	{
		string json = File.ReadAllText(path);
		ModelAsset asset = JsonUtility.FromJson<ModelAsset>(json);
		
		(uint width, uint height, uint depth) = asset.GetSize();
		Model model = new Model(width, height, depth);

		foreach (ModelAsset.Voxel assetVoxel in asset.Voxels)
		{
			Voxel modelVoxel = model.GetVoxel(assetVoxel.X, assetVoxel.Y, assetVoxel.Z);
			
			modelVoxel.Type = VoxelType.Unbreakable;
			modelVoxel.ColorIndex = assetVoxel.ColorIndex;
		}
		
		model.Apply();
		m_VoxelModel.SetModel(model, asset.Palette);

		StartCoroutine(Coroutine());
		IEnumerator Coroutine()
		{
			for(uint y = 0; y <= model.SizeY; y++)
			for(uint z = 0; z <= model.SizeZ; z++)
			for (uint x = 0; x <= model.SizeX; x++)
			{
				Voxel voxel = model.GetVoxel(x, model.SizeY - y, z);
				
				if(voxel == null || voxel.Type == VoxelType.Air)
					continue;

				voxel.Type = VoxelType.Air;
				List<Chunk> editedChunks = new(4);
				foreach(Voxel neighbor in voxel.Neighbors)
					if(neighbor != null && !editedChunks.Contains(neighbor.Chunk))
						editedChunks.Add(neighbor.Chunk);
				foreach (Chunk chunk in editedChunks)
					m_VoxelModel.GetRendererByChunk(chunk).Rebuild();
				
				yield return null;
			}
		}
	}
}