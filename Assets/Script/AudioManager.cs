using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	// Audio players components.
	public AudioSource EffectsSource;
	// Random pitch adjustment range.
	public float LowPitchRange = .95f;
	public float HighPitchRange = 1.05f;
	// Singleton instance.
	public static AudioManager Instance = null;

	[Header("Clips")]
	public AudioClip hitEnemyFX;
	public AudioClip hitPlayerFX;
	public AudioClip syringeFX;

	// Initialize the singleton instance.
	private void Awake()
	{
		// If there is not already an instance of SoundManager, set it to this.
		if (Instance == null)
		{
			Instance = this;
		}
		//If an instance already exists, destroy whatever this object is to enforce the singleton.
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
	}

	// Play a single clip through the sound effects source.
	public void PlayWithoutPitch(AudioClip clip, float vol)
	{
		EffectsSource.clip = clip;
		EffectsSource.pitch = 1;
		EffectsSource.PlayOneShot(EffectsSource.clip, vol);
	}
	public void Play(AudioClip clip, float vol)
	{
		EffectsSource.clip = clip;
		float randomPitch = Random.Range(LowPitchRange, HighPitchRange);
		EffectsSource.pitch = randomPitch;
		EffectsSource.PlayOneShot(EffectsSource.clip, vol);
	}

	// Play a random clip from an array, and randomize the pitch slightly.
	public void RandomSoundEffect(params AudioClip[] clips)
	{
		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(LowPitchRange, HighPitchRange);
		EffectsSource.pitch = randomPitch;
		EffectsSource.clip = clips[randomIndex];
		EffectsSource.Play();
	}
}