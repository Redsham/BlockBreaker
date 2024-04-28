using System;
using Voxels;
using Voxels.Core;

namespace Gameplay
{
	public abstract class GamemodeBase
	{
		public GamemodeHandler Handler { get; private set; }
		public float Progress
		{
			get => m_Progress;
			set
			{
				m_Progress = value;
				OnProgressChanged?.Invoke(value);
			}
		}
		public event Action<float> OnProgressChanged; 

		private float m_Progress;
		
		
		public virtual void Init(GamemodeHandler handler) => Handler = handler;
		public abstract Model PrepareModel(ModelAsset asset);
		public abstract void OnStart();
		public abstract void OnTap();
	}
}