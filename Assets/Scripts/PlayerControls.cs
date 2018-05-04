using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {
	
	#region Constants
	Vector3 up = new Vector3(0,1,0);
	Vector3 down = new Vector3(0,-1,0);
	Vector3 left = new Vector3(-1,0,0);
	Vector3 right = new Vector3(1,0,0);
	#endregion
	
	#region Components
	private SwipeController sc;
	private SceneController sceneController;
	private PlayerController playerController;
	#endregion

	void Awake () 
	{
		/* Set components */
		sc = this.GetComponent<SwipeController>();
		
		GameObject sceneControllerObj = GameObject.FindWithTag("Scene Controller");
		sceneController = sceneControllerObj.GetComponent<SceneController>();
		
		playerController = this.GetComponent<PlayerController>();
	}
	
	void Update () 
	{
		
		/* If player is grounded, he can swipe to change the gravity direction */
		if(playerController.IsGrounded())
		{
			if(sc.SwipeLeft) sceneController.SetGravity(left);
			else if(sc.SwipeUp) sceneController.SetGravity(up);
			else if(sc.SwipeRight) sceneController.SetGravity(right);
			else if(sc.SwipeDown) sceneController.SetGravity(down);
		}
	}
}
