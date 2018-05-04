using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour {
	public GameObject pairPortal;
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if ((col.tag).Equals("Player"))
		{	
			PlayerController pc = col.GetComponent<PlayerController>();
			
			/* If the player isn't already inside a portal */
			if(!pc.IsOnPortal())
			{
				col.gameObject.transform.position = pairPortal.transform.position;
				
				/* Now the player is inside a portal */
				pc.SetOnPortal(true);
			}
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if ((col.tag).Equals("Player"))
		{
			/* Wait to make sure this function is called
			after the trigger enter */
			StartCoroutine(WaitAndSetPortal(col.gameObject));
		}
	}

	IEnumerator WaitAndSetPortal(GameObject col)
	{
		yield return new WaitForSeconds(0.1f);
		PlayerController pc = col.GetComponent<PlayerController>();
		pc.SetOnPortal(false);
	}
}
