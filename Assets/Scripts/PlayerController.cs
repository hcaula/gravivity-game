using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	#region Public attributes
	public float respawnTime;
	public float angularSpeedSuccess;
	public int oppositeX;
	public int letGoMultiplier;
	public AnimationClip outlineGlow;
	public AnimationClip outlineReverse;
	public AnimationClip exitAnimation;
	public AnimationClip spawnPlayer;
	public Vector3 initialPosition;
	#endregion

	#region Private attributes
	private bool grounded;
	private string killedBy;
	private bool isOnPortal;
	private bool isOnMovingFloor;
	private bool followExit;
	private Rigidbody2D rb2d;
	private SceneController sceneController;
	private SoundManager soundManager;
	private PlayerAnimations playerAnimations;
	private Animation animationComponent;
	private GlobalVariables global;
	#endregion
	
	#region Gets and Sets
	public bool IsGrounded() {return this.grounded;}
	public string GetKilledBy() {return this.killedBy;}
	public bool IsOnPortal() {return this.isOnPortal;}
	public void SetOnPortal(bool value) {this.isOnPortal = value;}
	#endregion
	
	#region Auxiliar Functions
	void StopPlayerInMotion()
	{
		/* Stop physics momentum */
		rb2d.velocity = Vector2.zero;
		rb2d.angularVelocity = 0f;
		sceneController.SetGravity(new Vector3(0,0,0));
		sceneController.SetGravityVelocity(0);
	}
	
	void DeadAnimation ()
	{
		playerAnimations.InstatiateFragments(this.transform.position);
		soundManager.SinglePlay(soundManager.deathSound, soundManager.deathVolume);
	}
	#endregion
	
	void Awake ()
	{
		/* The player's Rigidbody2D component */
		rb2d = this.GetComponent<Rigidbody2D>();
		
		/* The scene controller */
		GameObject sceneControllerObj = GameObject.FindWithTag("Scene Controller");
		sceneController = sceneControllerObj.GetComponent<SceneController>();
		
		/* Gets the sound manager and animation from the scene controller */
		soundManager = sceneController.GetComponent<SoundManager>();
		
		/* Get animation component and manual animation script */
		animationComponent = this.GetComponent<Animation>();
		playerAnimations = this.GetComponent<PlayerAnimations>();
		
		/* Get the global variables component */
		global = this.GetComponent<GlobalVariables>();
		
		/* Initiate to avoid bugs on fragments */
		killedBy = "Kill Floor";
	}
	
	void Start () 
	{	
		this.initialPosition = this.transform.position;
		
		isOnPortal = false;
		followExit = false;
		isOnMovingFloor = false;
		
		/* Forces the position to the one we choose on the Scene Controller */
		// this.transform.position = initialPosition;
		
	}
	
	void FixedUpdate ()
	{
		/* Init local variables (it's better to control the gravity attributes
		on the scene controller, we may want to apply a universal gravity) */
		Vector3 gravity = sceneController.GetGravity();
		float velocity = sceneController.GetGravityVelocity();
		
		/* If it's a level that there are two players */
		gravity.x = gravity.x * oppositeX;
		
		/* Apply current gravity force on object */
		rb2d.AddForce(gravity * velocity);
	}
	
	void Update()
	{
		/* If the exit is moving, make the player follow it on success */
		if(followExit) this.transform.position = GameObject.FindWithTag("Exit").transform.position;
		
		/* Force the rotation to zero if the player is on a moving floor */
		if(isOnMovingFloor) this.transform.rotation = new Quaternion(0,0,0,0);
	}
	
	void OnCollisionEnter2D(Collision2D col) 
	{
		/* Check if player is grounded */
		if (col.gameObject.tag == "Floor" || col.gameObject.tag == "Moving Floor" || col.gameObject.tag == "Exit") 
		{
			grounded = true;
			animationComponent.clip = outlineGlow;
			animationComponent.Play();
		}
		
		if (col.gameObject.tag == "Moving Floor")
		{
			/* Make the player follow the moving floor by seting
			it as its gameobject parent */
			this.transform.SetParent(col.gameObject.transform);
			isOnMovingFloor = true;
			this.transform.rotation = new Quaternion(0,0,0,0);
		}
		
		/* Check if player died and set its respective killer */
		if (col.gameObject.tag == "BlastZone" || col.gameObject.tag == "Kill Floor") 
		{
			this.killedBy = col.gameObject.tag;
			
			/* Just in case something goes wrong */
			this.isOnPortal = false;
			Dead();
		}
	}
	
	void OnCollisionExit2D(Collision2D col) 
	{
		/* Check if player isn't grounded anymore */
		if (col.gameObject.tag == "Floor" || (col.gameObject.tag == "Exit" && grounded) || col.gameObject.tag == "Moving Floor") 
		{
			grounded = false;
			if(!animationComponent.IsPlaying(exitAnimation.name))
			{
				animationComponent.clip = outlineReverse;
				animationComponent.Play();
			}
		}
		
		if (col.gameObject.tag == "Moving Floor")
		{
			/* "Unglue" the player from the moving floor */
			this.transform.SetParent(null);
			isOnMovingFloor = false;
			
			MovingObjectController moc = col.gameObject.GetComponent<MovingObjectController>();
			Vector3 directionVector = moc.directionVector;
			float letGoSpeed = moc.velocity * letGoMultiplier;
			int direction = moc.GetDirection();
			
			rb2d.AddForce(directionVector * letGoSpeed * direction);
		}
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		/* If the collision happened with a pickup */
		if ((col.name).Contains("Pickup")) 
		{
			int pickupNum = global.GetStringNumber(col.name) - 1;
			sceneController.SetPickupsCollected(pickupNum, true);
			soundManager.AssignClipAndPlay(soundManager.pickups[pickupNum]);
			col.gameObject.SetActive(false);
		}
		
		/* If player hits the exit door */
		else if ((col.tag).Equals("Exit"))
		{
			Succeess();
		}
		
		/* For the joke level */
		else if ((col.name).Equals("SpecialExit"))
		{
			this.killedBy = "SpecialExit";
			StartCoroutine(SpecialExit());
		}
	}
	
	void Dead()
	{
		
		int deaths = PlayerPrefs.GetInt("deaths", 0) + 1;
		PlayerPrefs.SetInt("deaths", deaths);
		
		/* Play death sound and instantiate fragments */
		DeadAnimation();
		
		/* Wait two seconds and set position and gravity back to initial */
		StartCoroutine(Respawn());

	}
	
	void Succeess()
	{
		StopPlayerInMotion();
		this.GetComponent<BoxCollider2D>().enabled = false;
		followExit = true;
		
		animationComponent.Stop();
		animationComponent.clip = exitAnimation;
		animationComponent.Play();
		rb2d.angularVelocity = angularSpeedSuccess;
		
		/* Wait a few seconds and change scene */
		int successfull = sceneController.GetSuccessfullPlayers() + 1;
		sceneController.SetSuccessfullPlayers(successfull);
		StartCoroutine(sceneController.WaitAndChangeScene(exitAnimation.length));
	}

	IEnumerator Respawn()
	{
		/* Teleport the player offscreen */
		this.transform.position = sceneController.farDistance;
		
		/* Change rotation to normal position */
		this.transform.rotation = Quaternion.identity;
		
		StopPlayerInMotion();
		
		/* Reset pickups */
		StartCoroutine(sceneController.ResetPickups());
		
		/* Wait a little bit */
		yield return new WaitForSeconds(respawnTime);
		
		/* Teleport back to initial position */
		this.transform.position = this.initialPosition;
		soundManager.SinglePlay(soundManager.playerRespawn, soundManager.pickupVolume);
		
		animationComponent.clip = spawnPlayer;
		animationComponent.Play();
		
		/* Set every variable to their initial stetes */
		sceneController.RestartGravity();
		sceneController.RestartGravityVelocity();
	}	
	
	/* For the joke level */
	IEnumerator SpecialExit()
	{
		DeadAnimation();
		yield return new WaitForSeconds(2);
		sceneController.NextScene();
	}
	
}
