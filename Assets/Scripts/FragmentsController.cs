using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentsController : MonoBehaviour {

	#region Public attributes
	public float speed;
	public float destroyTime;
	public float fadeoutTime;
	public AnimationClip fadeOutFragment;
	#endregion

	#region Private attributes
	private float x;
	private float y;
	private Animation animationComponent;
	#endregion
	
	#region Auxiliar functions
	int RandomNegative () {
		float num = Random.value;
		if (num > 0.5) return 1;
		else return -1;
	}
	#endregion
	
	void Awake () {
		
		/* Get objects and componetns */
		PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
		SceneController sc = GameObject.FindWithTag("Scene Controller").GetComponent<SceneController>();
		animationComponent = this.GetComponent<Animation>();
		
		/* Get the enemy that killed the player */
		string killedBy = player.GetKilledBy();
		
		/* If killed by a kill floor, than we can splatter in all directions */
		if (killedBy.Equals("Kill Floor"))
		{
			x = Random.value * RandomNegative();
			y = Random.value * RandomNegative();
		}
		/* If killed by a blast zone, we want the fragments to appear
		on screen. So, calculate the direction opposite to gravity */
		else if (killedBy.Equals("BlastZone") || killedBy.Equals("SpecialExit"))
		{
			Vector3 gravity = sc.GetGravity();
			/* This means that the blast zone hitted was in a vertical position */
			if (gravity.x == 0)
			{
				/* Get a random value, but with direction inverted */
				y = Random.value * gravity.y * (-1);
				x = Random.value * RandomNegative();
			} 
			else {
				x = Random.value * gravity.x * (-1);
				y = Random.value * RandomNegative();
			}
		}
		
		StartCoroutine(RemoveFragments());
	}
	
	void FixedUpdate () {
		/* Move the fragments */
		Vector3 temp = this.transform.position;
		temp.x += speed * x/100;
		temp.y += speed * y/100;
		this.transform.position = temp;	
	}
	
	IEnumerator RemoveFragments()
	{
		/* Wait */
		yield return new WaitForSeconds(destroyTime);
		
		/* Play the fadeout animation */
		animationComponent.clip = fadeOutFragment;
		animationComponent.Play();
		
		/* Wait for the animation to end and destroy the object */
		yield return new WaitForSeconds(fadeOutFragment.length);
		Destroy(this.gameObject);
	}
}
