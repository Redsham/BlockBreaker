using System.Collections.Generic;
using UnityEngine;
using Voxels.Core;
using Random = UnityEngine.Random;

namespace Voxels.Components
{
	public class VoxelModelBehaviour : MonoBehaviour
	{
		private static readonly int Shader_MainTex = Shader.PropertyToID("_BaseMap");

		private readonly Dictionary<Chunk, ChunkRenderer> m_Renderers = new();
		private Texture2D m_Texture;

		public Model Model { get; private set; }
		public Material Material { get; private set; }

		
		public void SetModel(Model model, Color32[] palette)
		{
			Model = model;
			SetPalette(palette);
			
			ClearChunkRenderers();
			CreateChunkRenderers();
		}
		private void SetPalette(Color32[] palette)
		{
			if(m_Texture != null)
				Destroy(m_Texture);
			
			m_Texture = new Texture2D(palette.Length, Constants.NOISE_DEPTH, TextureFormat.RGBA32, false, false)
			{
				filterMode = FilterMode.Point
			};

			for (int i = 0; i < palette.Length; i++)
				for (int j = 0; j < Constants.NOISE_DEPTH; j++)
					m_Texture.SetPixel(i, j, (Color)palette[i] * Random.Range(1.0f - Constants.NOISE_RANGE, 1.0f + Constants.NOISE_RANGE));
			
			m_Texture.Apply();


			if (Material == null)
			{
				Material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
				
				foreach (ChunkRenderer chunkRenderer in m_Renderers.Values)
					chunkRenderer.MeshRenderer.sharedMaterial = Material;
			}
			
			Material.SetTexture(Shader_MainTex, m_Texture);
		}
		
		private void ClearChunkRenderers()
		{
			foreach (ChunkRenderer chunkRenderer in m_Renderers.Values)
				Destroy(chunkRenderer.gameObject);

			m_Renderers.Clear();
		}
		private void CreateChunkRenderers()
		{
			for (uint x = 0; x < Model.ChunksX; x++)
			for (uint y = 0; y < Model.ChunksY; y++)
			for (uint z = 0; z < Model.ChunksZ; z++)
			{
				Chunk chunk = Model.Chunks[Model.GetChunkIndex(x, y, z)];
				if (chunk == null)
					continue;
				
				GameObject chunkGameObject = new GameObject($"Chunk ({x}, {y}, {z})");
				
				Transform chunkTransform = chunkGameObject.transform;
				chunkTransform.SetParent(transform);
				chunkTransform.localRotation = Quaternion.identity;;
				chunkTransform.localPosition = new Vector3(
					chunk.X * Constants.CHUNK_SIZE * Constants.VOXEL_SIZE,
					chunk.Y * Constants.CHUNK_SIZE * Constants.VOXEL_SIZE,
					chunk.Z * Constants.CHUNK_SIZE * Constants.VOXEL_SIZE);
				
				ChunkRenderer chunkRenderer = chunkGameObject.AddComponent<ChunkRenderer>();
				chunkRenderer.Bind(Model, chunk, m_Texture.width);
				chunkRenderer.MeshRenderer.sharedMaterial = Material;
				
				m_Renderers.Add(chunk, chunkRenderer);
			}
		}
		public ChunkRenderer GetRendererByChunk(Chunk chunk) => m_Renderers.TryGetValue(chunk, out ChunkRenderer chunkRenderer) ? chunkRenderer : null;
	}
}