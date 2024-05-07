using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Voxels.Components;

namespace Gameplay
{
	public class GamemodeHandler : MonoBehaviour
	{
		/// <summary>
		/// Текущая сессия игры
		/// </summary>
		public static Session Session { get; set; }
		/// <summary>
		/// Текущий режим игры
		/// </summary>
		public GamemodeBase Gamemode { get; private set; }
		
		
		
		public VoxelModelBehaviour ModelBehaviour => m_ModelBehaviour;
		public PlayerController PlayerController => m_PlayerController;
		public UnityEvent<float> OnProgressChanged => m_OnProgressChanged;
		
		
		[Header("Events")]
		[SerializeField] private UnityEvent<float> m_OnProgressChanged;
		
		[Header("Components")]
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
			Gamemode.OnProgressChanged += m_OnProgressChanged.Invoke;
			
			Gamemode.OnStart();
		}
		private void Update() => Session.Tick(Time.deltaTime);
	}
}