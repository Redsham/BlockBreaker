using System;

namespace Utilities
{
	public static class BinaryHelper
	{
		public static byte ReadByte(byte[] data, ref int offset)
		{
			byte value = data[offset];
			offset++;
			return value;
		}
		public static uint ReadUInt32(byte[] data, ref int offset)
		{
			uint value = BitConverter.ToUInt32(data, offset);
			offset += 4;
			return value;
		}
	}
}