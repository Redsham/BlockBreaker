using UnityEngine;
using Voxels;
using Voxels.Components;
using Voxels.Core;

namespace Gameplay.VFX
{
	public class VoxelBreakEffect : MonoBehaviour
	{
		public static VoxelBreakEffect Active { get; private set; }

		[Header("Hummer")]
		[SerializeField] private Transform m_Hummer;
		[SerializeField] private Vector2   m_HummerHitPoint;
		
		[Header("Voxel")]
		[SerializeField] private Transform m_Voxel;
		[SerializeField] private Material  m_VoxelMaterial;
		
		[Header("Other")]
		[SerializeField] private ParticleSystem m_HitParticles;
		[SerializeField] private AudioSource	m_HitSoundSource;
		[SerializeField] private AudioClip[]    m_HitSounds;
		
		private int m_HummerTweenId;
		private int m_VoxelTweenId;

		private void Awake() => Active = this;
		private void Start()
		{
			m_Hummer.gameObject.SetActive(false);
			m_Voxel.gameObject.SetActive(false);
		}

		public void Play(Voxel voxel, VoxelVector voxelGlobalLocation, VoxelModelBehaviour voxelModelBehaviour, Vector3 normal, Vector3 up)
		{
			// Отображение объектов
			m_Hummer.gameObject.SetActive(true);
			m_Voxel.gameObject.SetActive(true);
			
			// Отмена предыдущих анимаций
			LeanTween.cancel(m_HummerTweenId, false);
			LeanTween.cancel(m_VoxelTweenId, false);

			#region Hummer

			up = Vector3.ProjectOnPlane(up, normal).normalized;
			
			Vector3 voxelLocation = VoxelsUtilities.GetVoxelWorldLocation(voxelModelBehaviour, voxelGlobalLocation);
			Vector3 hitPoint = voxelLocation + normal * Constants.VOXEL_SIZE / 2.0f;
			
			Vector3 hummerPosition = hitPoint + -up * m_HummerHitPoint.y + normal * m_HummerHitPoint.x;
			m_Hummer.SetPositionAndRotation(hummerPosition, Quaternion.LookRotation(up, normal));
			m_Hummer.localScale = Vector3.zero;

			#region Animation

			LTSeq seq = LeanTween.sequence();
			m_HummerTweenId = seq.id;
			
			seq.append(
				LeanTween.scale(m_Hummer.gameObject, Vector3.one, 0.05f)
					.setEaseOutSine()
			);
			seq.append(
				LeanTween.value(0.0f, 1.0f, 0.1f)
					.setOnUpdate((float value) =>
					{
						m_Hummer.rotation = Quaternion.Slerp(Quaternion.LookRotation(-up, normal),
							Quaternion.LookRotation(normal, up),
							value);
					})
					.setEaseInCirc()
			);
			seq.append(() =>
			{
				m_HitParticles.Play();
				m_HitSoundSource.PlayOneShot(m_HitSounds[Random.Range(0, m_HitSounds.Length)]);
			});
			seq.append(0.05f);
			seq.append(
				LeanTween.scale(m_Hummer.gameObject, Vector3.zero, 0.1f)
					.setEaseInSine()
			).append(() => m_Hummer.gameObject.SetActive(false));

			#endregion

			#endregion

			#region Voxel

			m_VoxelMaterial.color = voxelModelBehaviour.Palette[voxel.ColorIndex];
			m_Voxel.SetPositionAndRotation(voxelLocation, voxelModelBehaviour.transform.rotation);
			m_Voxel.localScale = Vector3.one * Constants.VOXEL_SIZE;

			// Анимация
			m_VoxelTweenId = LeanTween.scale(m_Voxel.gameObject, Vector3.zero, 0.1f)
				.setDelay(0.15f)
				.setEaseOutSine()
				.setOnComplete(() => m_Voxel.gameObject.SetActive(false)).id;

			#endregion

			#region Other
			
			// Перемещение источника звука
			m_HitSoundSource.transform.position = hitPoint;
			m_HitSoundSource.pitch = Random.Range(0.9f, 1.1f);
			m_HitSoundSource.volume = Random.Range(0.9f, 1.1f);
			
			// Перемещение частиц
			Transform particlesTransform = m_HitParticles.transform;
			particlesTransform.position = hitPoint;
			particlesTransform.forward = normal;
			
			ParticleSystem.MainModule mainModule = m_HitParticles.main;
			mainModule.startColor = m_VoxelMaterial.color;

			#endregion
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(m_Hummer.TransformPoint(new Vector3(0.0f, m_HummerHitPoint.y, m_HummerHitPoint.x)), 0.1f);
		}
	}
}