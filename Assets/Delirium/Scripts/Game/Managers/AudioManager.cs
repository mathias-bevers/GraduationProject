using System;
using Delirium.Tools;
using UnityEngine;
using UnityEngine.Audio;

namespace Delirium.Audio
{
	public class AudioManager : Singleton<AudioManager>
	{
		[SerializeField] private AudioMixer audioMixer;

		public Sound[] sounds;

		public AudioMixer AudioMixer => audioMixer;

		protected override void Awake()
		{
			base.Awake();

			foreach (Sound sound in sounds)
			{
				sound.source = gameObject.AddComponent<AudioSource>();
				sound.source.clip = sound.clip;

				sound.source.volume = sound.volume;
				sound.source.pitch = sound.pitch;
				sound.source.loop = sound.loop;
				sound.source.outputAudioMixerGroup = AudioMixer.FindMatchingGroups("Master")[0];
			}
		}

		private void Start()
		{
			Play("Theme");
			Play("Ambience_01");
			Play("Ambience_02");
			Play("Ambience_03");
		}

		public Sound Play(string clipName)
		{
			Sound sound = Array.Find(sounds, s => s.name == clipName);

			if (sound == null) { throw new NullReferenceException($"Could not find a sound clip with the name {clipName}."); }


			if (!sound.source.isPlaying) { sound.source.Play(); }

			return sound;
		}

		public Sound Stop(string clipName)
		{
			Sound sound = Array.Find(sounds, s => s.name == clipName);

			if (sound == null) { throw new NullReferenceException($"Could not find a sound clip with the name {clipName}."); }
			
			sound.source.Stop();
			return sound;
		}
	}
}