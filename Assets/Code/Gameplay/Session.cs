using Voxels;

namespace Gameplay
{
	public class Session
	{
		public readonly GamemodeBase Gamemode;
		public readonly ModelAsset   Model;
		
		public Session(GamemodeBase gamemode, ModelAsset model)
		{
			Gamemode = gamemode;
			Model    = model;
		}
	}
}