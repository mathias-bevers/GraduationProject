using Delirium.Audio;
using UnityEngine;

namespace Delirium.AI
{
	/// <summary>
	/// This class is used to play the correct enemy sounds.
	/// <para>Made by Mathias Bevers</para>
	/// </summary>
	public class RoamingEnemyAudio : MonoBehaviour
	{
		private const float CHASE_SOUND_VOLUME_MAX = 0.5f;

		[SerializeField] private float chaseFadeSpeed;

		private bool hasCompletedPreviousHunt = true;
		private bool shouldFadeChase;
		private Sound chaseSound;

		private void Start() { GetComponent<RoamingEnemy>().StateChangedEvent += OnStateChangedEvent; }

		/// <summary>
		///     Fades the chase music if the attached <see cref="RoamingEnemy" /> is back in the roaming state.
		///     When faded out the sound is stopped and the volume is reset to it's original volume.
		/// </summary>
		private void Update()
		{
			if (!shouldFadeChase) { return; }

			chaseSound.source.volume = Mathf.Lerp(chaseSound.source.volume, 0.0f, Time.deltaTime * chaseFadeSpeed);

			if (chaseSound.source.volume > 0.05f) { return; }

			shouldFadeChase = false;

			chaseSound.source.volume = CHASE_SOUND_VOLUME_MAX;

			AudioManager.Instance.Stop("Enemy_Chase");
		}

		/// <summary>
		///     Play or reset sounds based on the state of the attached <see cref="RoamingEnemy" />.
		/// </summary>
		/// <param name="state">The new state of the attached <see cref="RoamingEnemy" />.</param>
		private void OnStateChangedEvent(RoamingEnemyState state)
		{
			if (state == RoamingEnemyState.Roaming)
			{
				shouldFadeChase = true;
				hasCompletedPreviousHunt = true;
				return;
			}

			if (state != RoamingEnemyState.TargetLock || !hasCompletedPreviousHunt) { return; }

			chaseSound = AudioManager.Instance.Play("Enemy_Chase");
			shouldFadeChase = false;
			chaseSound.source.volume = CHASE_SOUND_VOLUME_MAX;


			AudioManager.Instance.Play("Jumpscare_01");
			hasCompletedPreviousHunt = false;
		}
	}
}