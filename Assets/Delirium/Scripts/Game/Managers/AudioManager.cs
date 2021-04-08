using System;
using Delirium.Tools;
using UnityEngine;
using UnityEngine.Audio;

namespace Delirium.Audio
{
	/// <summary>
	///     This class is used to Keep track of all of the game-sounds. It can <see cref="Play" /> and <see cref="Stop" /> sounds.
	///     <para>Made by: Amber Eissink</para>
	/// </summary>
	public class AudioManager : Singleton<AudioManager>
	{
		[SerializeField] private AudioMixer audioMixer;

		public Sound[] sounds;

		/// <summary>
		///     Get the audio mixer, which is set in the inspector.
		/// </summary>
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

		/// <summary>
		///     Try to play a sound by its name, when it is play leave it as is and only return the sound. If it isn't playing start playing the sound and return it.
		/// </summary>
		/// <param name="clipName">The name of the sound you want to play.</param>
		/// <returns>Returns the sound that is attempted to play.</returns>
		/// <exception cref="NullReferenceException">This exception is thrown when there is no sound with the given clipName.</exception>
		public Sound Play(string clipName)
		{
			Sound sound = Array.Find(sounds, s => s.name == clipName);

			if (sound == null) { throw new NullReferenceException($"Could not find a sound clip with the name {clipName}."); }


			if (!sound.source.isPlaying) { sound.source.Play(); }

			return sound;
		}

		/// <summary>
		///     Stop a sound by its name and return it.
		/// </summary>
		/// <param name="clipName">The name of the sound you want to stop.</param>
		/// <returns>Returns the sound that is stopped.</returns>
		/// <exception cref="NullReferenceException">This exception is thrown when there is no sound with the given clipName.</exception>
		public Sound Stop(string clipName)
		{
			Sound sound = Array.Find(sounds, s => s.name == clipName);

			if (sound == null) { throw new NullReferenceException($"Could not find a sound clip with the name {clipName}."); }

			sound.source.Stop();
			return sound;
		}
	}
}