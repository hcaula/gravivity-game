using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {
	
	#region Public attributes
	public double initLatency;
	public AudioClip wholeSong;
	public AudioClip intro;
	public AudioClip loop;
	#endregion
	
	#region Private attributes
	private AudioSource audioSource;
	private AudioSource introSource;
	private AudioSource loopSource;
	#endregion

	void Awake () {
		
		#region Components
		introSource = this.gameObject.AddComponent<AudioSource>();
		loopSource = this.gameObject.AddComponent<AudioSource>();
		
		if(this.GetComponent<GameController>() != null){
			this.GetComponent<GameController>().introAS = introSource;
			this.GetComponent<GameController>().loopAS = loopSource;
		}
		#endregion
	}
	
	void Start()
	{
		introSource.clip = intro;
		loopSource.clip = loop;
		loopSource.loop = true;
		
		/* Play intro after latency */
		introSource.PlayDelayed((float) initLatency); 
		
		/* Play loop after intro */
		loopSource.PlayDelayed((float) intro.length);
	}
	
}
