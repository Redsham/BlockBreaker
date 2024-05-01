using System;
using UnityEngine;

namespace Voxels.Core
{
	public struct VoxelVector
	{
		public uint X;
		public uint Y;
		public uint Z;
		
		public VoxelVector(uint x, uint y, uint z)
		{
			X = x;
			Y = y;
			Z = z;
		}
		
		public static VoxelVector Zero => new(0, 0, 0);

		public override string ToString() => $"VoxelVector({X}, {Y}, {Z})";
		
		public static implicit operator Vector2(VoxelVector v) => new(v.X, v.Y);
		public static implicit operator Vector3(VoxelVector v) => new(v.X, v.Y, v.Z);
		
		public static VoxelVector operator +(VoxelVector a, VoxelVector b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		public static VoxelVector operator -(VoxelVector a, VoxelVector b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		public static VoxelVector operator *(VoxelVector a, uint b) => new(a.X * b, a.Y * b, a.Z * b);
		public static VoxelVector operator /(VoxelVector a, uint b) => new(a.X / b, a.Y / b, a.Z / b);
		public static bool operator ==(VoxelVector a, VoxelVector b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
		public static bool operator !=(VoxelVector a, VoxelVector b) => a.X != b.X || a.Y != b.Y || a.Z != b.Z;
		public override bool Equals(object obj) => obj is VoxelVector coordinate && this == coordinate;
		public override int GetHashCode() => HashCode.Combine(X, Y, Z);
	}
}