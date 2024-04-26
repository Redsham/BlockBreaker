using Voxels;
using Voxels.Core;

namespace Gameplay
{
	public abstract class GamemodeBase
	{
		public GamemodeHandler Handler { get; private set; }
		
		
		public virtual void Init(GamemodeHandler handler) => Handler = handler;
		public abstract Model PrepareModel(ModelAsset asset);
		public abstract void OnTap();
	}
}