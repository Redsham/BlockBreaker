using Voxels;

namespace Gameplay
{
	public class Session
	{
		public GamemodeBase Gamemode { get; }
		public ModelAsset   Model { get; }
		
		public Session(GamemodeBase gamemode, ModelAsset model)
		{
			Gamemode = gamemode;
			Model    = model;
		}
	}
}