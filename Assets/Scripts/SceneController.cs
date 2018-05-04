using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {
	
	#region Public attributes
	public Vector3 initialGravity;
	public Vector3 initialPosition;
	public float initialGravityVelocity;
	public Vector3 farDistance;
	public float successTime;
	#endregion
	
	#region Private attributes
	private GameObject[] players;
	private PlayerController playerController;
	private GameController gameController;
	private GameObject[] pickups = new GameObject[3];
	private bool[] pickupsCollected;
	private int pickupCounter;
	private float gravityVelocity;
	private Vector3 gravity;
	private int successfullPlayers;
	
	private SceneController sceneController;
	private SoundManager soundManager;
	#endregion
	
	#region Gets and Sets
	public float GetGravityVelocity() {return this.gravityVelocity;}
	public void SetGravityVelocity(float velocity) {this.gravityVelocity = velocity;}
	
	public Vector3 GetGravity() {return this.gravity;}
	public void SetGravity(Vector3 gravity) {this.gravity = gravity;}
	
	public GameObject GetPickup(int index) {return this.pickups[index];}
	public GameObject[] GetPickups() {return this.pickups;}
	public void SetPickup(int index, GameObject obj) {this.pickups[index] = obj;}
	
	public bool GetPickupsCollected(int index) {return this.pickupsCollected[index];}
	public void SetPickupsCollected(int index, bool value) {this.pickupsCollected[index] = value;}
	
	public int GetPickupCounter() {return this.pickupCounter;}
	public void SetPickupCounter(int value) {this.pickupCounter = value;}
	
	public int GetSuccessfullPlayers() {return this.successfullPlayers;}
	public void SetSuccessfullPlayers(int value) {this.successfullPlayers = value;}
	
	#endregion
	
	#region Restart functions
	public void RestartGravity() {this.gravity = this.initialGravity;}
	public void RestartGravityVelocity() {this.gravityVelocity = this.initialGravityVelocity;}
	#endregion
	
	#region Initiate functions
	void InitiatePickups()
	{
		pickups[0] = GameObject.FindWithTag("Pickup #1");
		pickups[1] = GameObject.FindWithTag("Pickup #2");
		pickups[2] = GameObject.FindWithTag("Pickup #3");
		pickupsCollected = new bool[] {false, false, false};
		pickupCounter = 0;
	}
	void InitiatePlayer()
	{
		players = GameObject.FindGameObjectsWithTag("Player");
		playerController = players[0].GetComponent<PlayerController>();
	}
	void InitiateGameController()
	{
		GameObject obj = GameObject.FindWithTag("GameController");
		gameController = obj.GetComponent<GameController>();
	}
	#endregion
	
	void Awake ()
	{
		InitiatePickups();
		InitiatePlayer();
		InitiateGameController();
		
		soundManager = this.GetComponent<SoundManager>();	
	}
	
	void Start ()
	{
		/* Set the gravity direction and velocity to their initial values */
		RestartGravity();
		RestartGravityVelocity();
	}
	
	public IEnumerator ResetPickups()
	{
		/* Activate for each pickup */
		int i = 0;
		foreach(GameObject pickup in pickups)
		{
			/* Wait for dramatic effect */
			yield return new WaitForSeconds(playerController.respawnTime/3);
			
			/* Play respawn sound only if the pickup has been collected */
			if (pickupsCollected[i]) soundManager.SinglePlay(soundManager.pickupRespawn, soundManager.pickupVolume);
			pickup.SetActive(true);
			pickupsCollected[i] = false;
			i++;
			pickupCounter--;
		}
	}
	
	public void NextScene()
	{
		gameController.LevelToTransition();
	}
	
	public IEnumerator WaitAndChangeScene(float time)
	{
		
		yield return new WaitForSeconds(time);
		
		/* Play the success sound */
		soundManager.SinglePlay(soundManager.pickupRespawn, 1);
		
		if(successfullPlayers == players.Length) 
		{
			/* Wait so that the success audio clip can play */
			yield return new WaitForSeconds(successTime);
	
			/* Change scene */
			NextScene();
		}
	}
		
}
