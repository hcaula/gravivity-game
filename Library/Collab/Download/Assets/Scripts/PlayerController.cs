using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	/* Public attributes */
	public Vector3 initialGravity;
	public Vector3 initialPosition;
	public Vector3 farDistance;
	public int initialVelocity;
	public float respawnTime;
	public int qtdFragments;
	public GameObject fragmentPrefab;
	public AudioClip deathSound;
	public AudioClip pickup1;
	public AudioClip pickup2;
	public AudioClip pickup3;
	public AudioClip pickupRespawn;
	public AudioClip playerRespawn;
	public float deathVolume;
	public float pickupVolume;

	
	/* Private attributes */
	private bool grounded;
	private Vector3 up = new Vector3(0,1,0);
	private Vector3 down = new Vector3(0,-1,0);
	private Vector3 left = new Vector3(-1,0,0);
	private Vector3 right = new Vector3(1,0,0);
	private Rigidbody2D rb2d;
	private SwipeController sc;
	private SpriteRenderer sr;
	private AudioSource audioSource;
	private Vector3 gravity;
	private int velocity;
	private GameObject outline;
	private GameObject[] pickups = new GameObject[3];
	private bool[] pickupsCollected;
	private int pickupCounter;
	
	
	void Start () 
	{
		/* Get components and objects prefabs */
		rb2d = this.GetComponent<Rigidbody2D>();
		sc = this.GetComponent<SwipeController>();
		sr = this.GetComponent<SpriteRenderer>();
		audioSource = this.GetComponent<AudioSource>();
		outline = GameObject.FindWithTag("Outline");
		// pickups = GameObject.FindGameObjectsWithTag("Pickup");
		pickups[0] = GameObject.FindWithTag("Pickup #1");
		pickups[1] = GameObject.FindWithTag("Pickup #2");
		pickups[2] = GameObject.FindWithTag("Pickup #3");
		
		/* Set pickups collected to false */
		pickupsCollected = new bool[] {false, false, false};
		pickupCounter = 0;
				
		/* Set gravity and initial position */
		gravity = initialGravity;
		this.transform.position = initialPosition;
		velocity = initialVelocity;
	}
	
	void Update () 
	{
		/* Apply current gravity force on object */
		rb2d.AddForce(gravity * velocity);
		
		/* 
		 * Check swipe and change gravity if player is on the floor 
		 * Also, check if grounded to activate outline
		*/
		if(grounded)
		{
			outline.SetActive(true);
			if(sc.SwipeLeft) gravity = left;
			else if(sc.SwipeUp) gravity = up;
			else if(sc.SwipeRight) gravity = right;
			else if(sc.SwipeDown) gravity = down;
		}
		else outline.SetActive(false);
	}
	
	void OnCollisionEnter2D(Collision2D col) 
	{
		/* Check if player is grounded */
		if (col.gameObject.tag == "Floor") grounded = true;
		
		/* Check if player exited the screen */
		if (col.gameObject.tag == "BlastZone") Died();
		
		/* Check if player collided with a kill floor */
		if (col.gameObject.tag == "Kill Floor") Died();
	}
	
	void OnCollisionExit2D(Collision2D col) 
	{
		/* Check if player isn't grounded anymore */
		if (col.gameObject.tag == "Floor") grounded = false;
	}
	
	/* Check for pickup collision */
	void OnTriggerEnter2D(Collider2D col)
	{
		/* Check witch pickup it is, so it can be correctly displayed */
		if ((col.name).Equals("Pickup #1")) 
		{
			pickupsCollected[0] = true;
			AssignClipAndPlay(pickup1);
		}
		else if ((col.name).Equals("Pickup #2")) 
		{
			pickupsCollected[1] = true;
			AssignClipAndPlay(pickup2);
		}
		if ((col.name).Equals("Pickup #3")) 
		{
			pickupsCollected[2] = true;
			AssignClipAndPlay(pickup3);
		}
		
		
		
		/* Deactivate the pickup */
		col.gameObject.SetActive(false);
	}
	
	void Died()
	{
		/* Activate fragments */
		AnimateFragments();
		
		/* Play death sound */
		audioSource.PlayOneShot(deathSound, deathVolume);
		
		/* Wait two seconds and set position and gravity back to initial */
		StartCoroutine(Respawn());

	}
	
	IEnumerator Respawn()
	{
		/* Teleport the player offscreen */
		this.transform.position = farDistance;
		
		/* Stop physics momentum */
		this.transform.rotation = Quaternion.identity;
		rb2d.velocity = Vector2.zero;
		rb2d.angularVelocity = 0f; 
		gravity = new Vector3(0,0,0);
		velocity = 0;
		
		/* Reset pickups */
		StartCoroutine(ResetPickups());
		
		/* Wait a little bit */
		yield return new WaitForSeconds(respawnTime);
		
		/* Teleport back to initial position */
		this.transform.position = initialPosition;
		audioSource.PlayOneShot(playerRespawn, pickupVolume);
		
		/* Set every variable to their initial stetes */
		gravity = initialGravity;
		velocity = initialVelocity;
		
		
	}
	
	void AnimateFragments()
	{
		for(int i = 0; i < qtdFragments; i++)
		{
			fragmentPrefab.GetComponent<SpriteRenderer>().color = sr.color;
			GameObject fragment = Instantiate(fragmentPrefab, this.transform.position, Quaternion.identity);
			fragment.SetActive(true);
		}
	}
	
	IEnumerator ResetPickups()
	{
		/* Activate for each pickup */
		int i = 0;
		foreach(GameObject pickup in pickups)
		{
			/* Wait for dramatic effect */
			yield return new WaitForSeconds(respawnTime/3);
			
			/* Play respawn sound only if the pickup has been collected */
			if (pickupsCollected[i]) audioSource.PlayOneShot(pickupRespawn, deathVolume);
			print(pickupsCollected[i]);
			
			pickup.SetActive(true);
			pickupsCollected[i] = false;
			i++;
			pickupCounter--;
			
		}
	}
	
	void AssignClipAndPlay(AudioClip clip)
	{
		audioSource.clip = clip;
		audioSource.Stop();
		audioSource.Play();
	}
}
