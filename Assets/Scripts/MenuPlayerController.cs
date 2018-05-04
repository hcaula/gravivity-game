using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayerController : MonoBehaviour {
	
	#region Public attributes
	public bool grounded; // To know when to play camera animations
	public float maxSpeed; // Max falling speed
	public float angularSpeed; // The rotation after the player bounces
	public AudioClip crash;
	public AudioClip tom1;
	public AudioClip tom2;
	#endregion
	
	#region Private attributes
	private int bounceCount; // How many times the player has bounced
	private AudioSource audioSource;
	private Vector3 gravity;
	private Rigidbody2D rb2d;
	#endregion
	
	#region Gets and Sets
	public int GetBounceCount() {return this.bounceCount;}
	#endregion
	
	#region Auxiliar functions
	void LockX()
	{
		Vector3 temp = this.transform.position;
		temp.x = 0;
		this.transform.position = temp;
	}
	#endregion
	
	void Awake ()
	{
		#region Get components
		audioSource = this.GetComponent<AudioSource>();
		rb2d = this.GetComponent<Rigidbody2D>();
		#endregion
	}
	
	void Start()
	{
		grounded = false;
		gravity = new Vector3(0,-1,0);
		bounceCount = 0;
		
		/* Play rufem sound */
		audioSource.Play();	
	}
	
	void FixedUpdate ()
	{
		if(rb2d.velocity.magnitude <= maxSpeed) rb2d.AddForce(gravity * 10);
		
		/* So that when the player is spinning, it never leaves the x position (x = 0) */
		if (grounded) LockX();
	}

	void OnCollisionEnter2D(Collision2D col) {
		if(col.gameObject.tag == "Floor")
		{
			grounded = true;
			bounceCount++;
			
			if (bounceCount == 1) 
			{
				/* After the first bounce, stop the rufem sound */
				audioSource.Stop();
				
				audioSource.PlayOneShot(crash, 1);
				
				/* Spin the player */
				rb2d.angularVelocity = angularSpeed;
			}
			else if (bounceCount == 2) audioSource.PlayOneShot(tom1, 1);
			else if (bounceCount == 3) audioSource.PlayOneShot(tom2, 1);
		}
	}
}
