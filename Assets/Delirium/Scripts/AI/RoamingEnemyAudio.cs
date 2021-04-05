using Delirium.Audio;
using UnityEngine;

namespace Delirium.AI
{
	public class RoamingEnemyAudio : MonoBehaviour
	{
		private const float CHASE_SOUND_VOLUME_MAX = 0.5f;

		[SerializeField] private float chaseFadeSpeed;

		private bool hasCompletedPreviousHunt = true;
		private bool shouldFadeChase;
		private Sound chaseSound;

		private void Start() { GetComponent<RoamingEnemy>().StateChangedEvent += OnStateChangedEvent; }

		private void Update()
		{
			if (!shouldFadeChase) { return; }

			chaseSound.source.volume = Mathf.Lerp(chaseSound.source.volume, 0.0f, Time.deltaTime * chaseFadeSpeed);

			if (chaseSound.source.volume > 0.05f) { return; }

			chaseSound.source.volume = CHASE_SOUND_VOLUME_MAX;

			AudioManager.Instance.Stop("Enemy_Chase");

			shouldFadeChase = false;
		}

		private void OnStateChangedEvent(EnemyAIState state)
		{
			if (state == EnemyAIState.Roaming)
			{
				shouldFadeChase = true;
				hasCompletedPreviousHunt = true;
				return;
			}

			if (state != EnemyAIState.TargetLock || !hasCompletedPreviousHunt) { return; }

			chaseSound = AudioManager.Instance.Play("Enemy_Chase");
			shouldFadeChase = false;
			chaseSound.source.volume = CHASE_SOUND_VOLUME_MAX;


			AudioManager.Instance.Play("Jumpscare_01");
			hasCompletedPreviousHunt = false;
		}
	}
}