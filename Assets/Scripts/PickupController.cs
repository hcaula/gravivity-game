using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour {
	
	#region Public attributes
	public float rotationSpeed;
	#endregion
	
	void FixedUpdate ()
	{
		/* Rotate the pickup */
		this.transform.Rotate(new Vector3(0,0,1) * rotationSpeed * Time.deltaTime);
	}
}
