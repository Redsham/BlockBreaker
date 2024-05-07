using Voxels;

namespace Gameplay
{
	public class Session
	{
		public readonly GamemodeBase Gamemode;
		public readonly ModelAsset   Model;
		
		public float Time { get; set; }
		
		public Session(GamemodeBase gamemode, ModelAsset model)
		{
			Gamemode = gamemode;
			Model    = model;
		}
		
		public void Tick(float deltaTime)
		{
			Time += deltaTime;
		}
	}
}