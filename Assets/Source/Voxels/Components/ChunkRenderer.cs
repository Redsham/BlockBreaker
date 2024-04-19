using System.Collections.Generic;
using UnityEngine;
using Voxels.Core;

namespace Voxels.Components
{
	public class ChunkRenderer : MonoBehaviour
	{
		public MeshFilter MeshFilter { get; private set; }
		public MeshRenderer MeshRenderer { get; private set; }
		
		public Mesh Mesh { get; private set; }
		
		public Model Model { get; private set; }
		public Chunk Chunk { get; private set; }

		private readonly List<Vector3> m_Vertices = new();
		private readonly List<int> m_Triangles = new();
		private readonly List<Vector3> m_Normals = new();
		private readonly List<Vector2> m_Uvs = new();

		private float m_PaletteSize;

		
		
		private void Awake()
		{
			MeshFilter = gameObject.AddComponent<MeshFilter>();
			MeshRenderer = gameObject.AddComponent<MeshRenderer>();
		
			Mesh = new Mesh();
			MeshFilter.mesh = Mesh;
		}
		private void OnDestroy() => Destroy(Mesh);
		
		private void OnDrawGizmos()
		{
			Vector3 size = Vector3.one * Constants.CHUNK_SIZE * Constants.VOXEL_SIZE;
			Gizmos.DrawWireCube(transform.TransformPoint(size / 2.0f), size);
		}
		
		public void Bind(Model model, Chunk chunk, float palette)
		{
			Model = model;
			Chunk = chunk;
			m_PaletteSize = palette;
			
			Rebuild();
		}
		public void Rebuild()
		{
			Mesh.Clear();

			for (uint x = 0; x < Constants.CHUNK_SIZE; x++)
			for (uint y = 0; y < Constants.CHUNK_SIZE; y++)
			for (uint z = 0; z < Constants.CHUNK_SIZE; z++)
			{
				Voxel voxel = Chunk.GetVoxel(x, y, z);
				
				if (voxel.Type == VoxelType.Air)
					continue;
				
				if (voxel.Neighbors[0] == null || voxel.Neighbors[0].Type == VoxelType.Air)
					AddQuad(
						new Vector3(x, y, z + 1),
						new Vector3(x, y + 1, z + 1),
						new Vector3(x, y + 1, z),
						new Vector3(x, y, z),
						Vector3.left,
						voxel.ColorIndex,
						voxel.NoiseIndex
					);
				
				if (voxel.Neighbors[1] == null || voxel.Neighbors[1].Type == VoxelType.Air)
					AddQuad(
						new Vector3(x + 1, y, z),
						new Vector3(x + 1, y + 1, z),
						new Vector3(x + 1, y + 1, z + 1),
						new Vector3(x + 1, y, z + 1),
						Vector3.right,
						voxel.ColorIndex,
						voxel.NoiseIndex
					);
					
				
				if (voxel.Neighbors[2] == null || voxel.Neighbors[2].Type == VoxelType.Air)
					AddQuad(
						new Vector3(x, y, z),
						new Vector3(x + 1, y, z),
						new Vector3(x + 1, y, z + 1),
						new Vector3(x, y, z + 1),
						Vector3.down,
						voxel.ColorIndex,
						voxel.NoiseIndex
					);
					
				
				if (voxel.Neighbors[3] == null || voxel.Neighbors[3].Type == VoxelType.Air)
					AddQuad(
						new Vector3(x, y + 1, z + 1),
						new Vector3(x + 1, y + 1, z + 1),
						new Vector3(x + 1, y + 1, z),
						new Vector3(x, y + 1, z),
						Vector3.up,
						voxel.ColorIndex,
						voxel.NoiseIndex
					);
					
				
				if (voxel.Neighbors[4] == null || voxel.Neighbors[4].Type == VoxelType.Air)
					AddQuad(
						new Vector3(x, y + 1, z),
						new Vector3(x + 1, y + 1, z),
						new Vector3(x + 1, y, z),
						new Vector3(x, y, z),
						Vector3.back,
						voxel.ColorIndex,
						voxel.NoiseIndex
					);
					
				
				if (voxel.Neighbors[5] == null || voxel.Neighbors[5].Type == VoxelType.Air)
					AddQuad(
						new Vector3(x, y, z + 1),
						new Vector3(x + 1, y, z + 1),
						new Vector3(x + 1, y + 1, z + 1),
						new Vector3(x, y + 1, z + 1),
						Vector3.forward,
						voxel.ColorIndex,
						voxel.NoiseIndex
					);
					
			}
			
			Mesh.vertices = m_Vertices.ToArray();
			Mesh.triangles = m_Triangles.ToArray();
			Mesh.normals = m_Normals.ToArray();
			Mesh.uv = m_Uvs.ToArray();
			
			m_Vertices.Clear();
			m_Triangles.Clear();
			m_Normals.Clear();
			m_Uvs.Clear();
			return;

			void AddQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 normal, byte colorIndex, byte noiseIndex)
			{
				int index = m_Vertices.Count;
				
				m_Vertices.Add(a * Constants.VOXEL_SIZE);
				m_Vertices.Add(b * Constants.VOXEL_SIZE);
				m_Vertices.Add(c * Constants.VOXEL_SIZE);
				m_Vertices.Add(d * Constants.VOXEL_SIZE);
				
				m_Triangles.Add(index);
				m_Triangles.Add(index + 1);
				m_Triangles.Add(index + 2);
				m_Triangles.Add(index + 2);
				m_Triangles.Add(index + 3);
				m_Triangles.Add(index);

				m_Normals.Add(normal);
				m_Normals.Add(normal);
				m_Normals.Add(normal);
				m_Normals.Add(normal);
				
				m_Uvs.Add(new Vector2(colorIndex / m_PaletteSize, noiseIndex / (float)Constants.NOISE_DEPTH));
				m_Uvs.Add(new Vector2((colorIndex + 1) / m_PaletteSize, noiseIndex / (float)Constants.NOISE_DEPTH));
				m_Uvs.Add(new Vector2((colorIndex + 1) / m_PaletteSize, (noiseIndex + 1) / (float)Constants.NOISE_DEPTH));
				m_Uvs.Add(new Vector2(colorIndex / m_PaletteSize, (noiseIndex + 1) / (float)Constants.NOISE_DEPTH));
			}
		}
	}
}