using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraController : MonoBehaviour {

	#region Public attributes
	public float getBackTime; // Time between the last bounce and zoom out
	public GameObject player;
	public AnimationClip changeColor;
	public AnimationClip getBack;
	#endregion
	
	#region Private attributes
	private Camera cam;
	private bool followPlayer; // If true, camera follows the player.
	private Animation camAnimation;
	#endregion
	
	#region Gets and Sets
	public void SetFollowPlayer(bool value) {this.followPlayer = false;}
	#endregion
	
	void Start () {
		
		#region Components and objects
		GameObject camObj = GameObject.FindWithTag("MainCamera");
		cam = camObj.GetComponent<Camera>();
		camAnimation = camObj.GetComponent<Animation>();
		#endregion
		
		followPlayer = true;
	}
	
	void Update () 
	{
		/* When the player hits the middle of the screen */
		if (player.transform.position.y <= 0 && followPlayer)
		{
			Vector3 pos = cam.transform.position;
			pos.y = player.transform.position.y;
			cam.transform.position = pos;
		}
	}
	
	public void Play(string animation) 
	{
		if (animation.Equals("Change Color")) 
		{
			camAnimation.clip = changeColor;
			camAnimation.Play();
		}
		if (animation.Equals("Get Back")) StartCoroutine(PlayGetBack());
	}
	
	IEnumerator PlayGetBack()
	{
		camAnimation.clip = getBack;
		yield return new WaitForSeconds(getBackTime);
		camAnimation.Play();
		followPlayer = false;
	}
}
