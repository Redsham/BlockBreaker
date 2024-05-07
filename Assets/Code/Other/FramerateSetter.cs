using UnityEngine;

namespace Other
{
	public static class FramerateSetter
	{
		[Bootstrapping.BootstrapMethod]
		public static void Bootstrap() => Application.targetFrameRate = 60;
	}
}