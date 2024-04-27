using UnityEngine;
using UnityEngine.Events;
using Voxels.Components;

namespace Gameplay
{
	public class GamemodeHandler : MonoBehaviour
	{
		/// <summary>
		/// Текущая сессия игры
		/// </summary>
		public static Session Session { get; set; }
		
		public GamemodeBase Gamemode { get; private set; }
		public VoxelModelBehaviour ModelBehaviour => m_ModelBehaviour;
		public PlayerController PlayerController => m_PlayerController;
		
		public UnityEvent<float> OnProgressChanged;
		
		[SerializeField] private VoxelModelBehaviour m_ModelBehaviour;
		[SerializeField] private PlayerController m_PlayerController;


		private void Start()
		{
			InitGamemode(Session);
			m_PlayerController.OnTap.AddListener(() => Gamemode.OnTap());
		}
		private void InitGamemode(Session session)
		{
			Gamemode = session.Gamemode;
			Gamemode.Init(this);
			
			ModelBehaviour.SetModel(Gamemode.PrepareModel(session.Model), session.Model.Palette);
			Gamemode.OnProgressChanged += OnProgressChanged.Invoke;
			
			Gamemode.OnStart();
		}
	}
}