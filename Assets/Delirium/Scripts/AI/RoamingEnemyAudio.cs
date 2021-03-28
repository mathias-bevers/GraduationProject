using System.Collections;
using Delirium.Audio;
using UnityEngine;

namespace Delirium.AI
{
	public class 
		RoamingEnemyAudio : MonoBehaviour
	{
		[SerializeField] private float chaseFadeSpeed;
		private bool shouldStopClip = true;
		private bool canPlayJumpScare = true;
		private bool shouldFadeChase;

		private EnemyAIState previousState;
		private Sound chaseSound;

		private void Start() { GetComponent<RoamingEnemy>().StateChangedEvent += OnStateChangedEvent; }

		private void Update()
		{
			if (!shouldFadeChase) { return; }

			chaseSound.source.volume = Mathf.Lerp(chaseSound.source.volume, 0.0f, Time.deltaTime * chaseFadeSpeed);

			if (chaseSound.source.volume > 0.05f) { return; }

			chaseSound.source.volume = 0.5f;

			AudioManager.Instance.Stop("Enemy_Chase");
			shouldFadeChase = false;
		}

		private void OnStateChangedEvent(EnemyAIState state)
		{
			if (previousState == EnemyAIState.Attack)
			{
				previousState = state;
				return;
			}

			previousState = state;

			if (state == EnemyAIState.Roaming)
			{
				shouldFadeChase = true;
				canPlayJumpScare = true;
			}

			if (state != EnemyAIState.TargetLock) { return; }

			chaseSound = AudioManager.Instance.Play("Enemy_Chase");

			if (!canPlayJumpScare) { return; }

			AudioManager.Instance.Play("Jumpscare_01");
			canPlayJumpScare = false;
		}

		private IEnumerator ChaseStopCooldown()
		{
			shouldStopClip = false;
			yield return new WaitForSeconds(5.0f);
			shouldStopClip = true;
		}
	}
}