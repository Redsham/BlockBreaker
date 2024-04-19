using UnityEngine;

public static class DebugUtilities
{
	public static void DrawCube(Vector3 center, Vector3 size, Quaternion rotation, Color color, float time)
	{
		Vector3[] vertices = new[]
		{
			new Vector3(-0.5f, -0.5f, -0.5f),
			new Vector3(0.5f, -0.5f, -0.5f),
			new Vector3(0.5f, 0.5f, -0.5f),
			new Vector3(-0.5f, 0.5f, -0.5f),
			new Vector3(-0.5f, -0.5f, 0.5f),
			new Vector3(0.5f, -0.5f, 0.5f),
			new Vector3(0.5f, 0.5f, 0.5f),
			new Vector3(-0.5f, 0.5f, 0.5f)
		};

		for (int i = 0; i < vertices.Length; i++)
		{
			vertices[i].x *= size.x;
			vertices[i].y *= size.y;
			vertices[i].z *= size.z;
			vertices[i] = rotation * vertices[i] + center;
		}

		Debug.DrawLine(vertices[0], vertices[1], color, time);
		Debug.DrawLine(vertices[1], vertices[2], color, time);
		Debug.DrawLine(vertices[2], vertices[3], color, time);
		Debug.DrawLine(vertices[3], vertices[0], color, time);

		Debug.DrawLine(vertices[4], vertices[5], color, time);
		Debug.DrawLine(vertices[5], vertices[6], color, time);
		Debug.DrawLine(vertices[6], vertices[7], color, time);
		Debug.DrawLine(vertices[7], vertices[4], color, time);

		Debug.DrawLine(vertices[0], vertices[4], color, time);
		Debug.DrawLine(vertices[1], vertices[5], color, time);
		Debug.DrawLine(vertices[2], vertices[6], color, time);
		Debug.DrawLine(vertices[3], vertices[7], color, time);
	}
	public static void DrawBounds(Bounds bounds, Color color, float time) => DrawCube(bounds.center, bounds.size, Quaternion.identity, color, time);
}