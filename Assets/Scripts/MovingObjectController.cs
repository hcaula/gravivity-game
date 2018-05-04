using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectController : MonoBehaviour {
	
	#region Public attributes
	public float velocity; // The speed of the platform
	public float limitX; // The bounds of its movement in the x-axis
	public Vector3 directionVector;
	#endregion
	
	#region Private attributes
	private int direction; // The current direction of the platform
	#endregion
	
	public int GetDirection() {return this.direction;}

	void Start () {
		/* Starts positive */
		direction = 1;
	}
	
	void FixedUpdate () {
		Vector3 position = this.transform.position;
		if (position.x >= limitX || position.x <= -limitX) direction = direction * (-1); 		
		this.transform.position += transform.right * velocity * direction;
	}
}
