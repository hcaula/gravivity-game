using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	
	#region Audio Clips
	public AudioClip deathSound;
	public AudioClip[] pickups;
	public AudioClip pickupRespawn;
	public AudioClip playerRespawn;
	#endregion
	
	#region Public attributes
	public float deathVolume;
	public float pickupVolume;
	#endregion
	
	#region Private attributes
	private AudioSource audioSource;
	#endregion

	void Awake () {
		audioSource = this.GetComponent<AudioSource>();
	}
	
	public void AssignClipAndPlay(AudioClip clip)
	{
		audioSource.clip = clip;
		audioSource.Stop();
		audioSource.Play();
	}
	
	public void SinglePlay(AudioClip clip, float volume)
	{
		audioSource.PlayOneShot(clip, volume);
	}
}
