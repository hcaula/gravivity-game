using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour {
	
	#region Public attributes
	public int qtdFragments;
	public GameObject fragmentPrefab;
	#endregion
	
	#region Private attributes
	private SpriteRenderer playerSR;
	private GameObject player;
	#endregion
	
	void Awake () 
	{
		#region Objects and components
		player = GameObject.FindWithTag("Player");
		playerSR = player.GetComponent<SpriteRenderer>();
		#endregion
	}
	
	public void InstatiateFragments(Vector3 position)
	{
		/* Iterate over the desired quantity of fragments */
		for(int i = 0; i < qtdFragments; i++)
		{
			/* Set the fragment color to the same as player */
			fragmentPrefab.GetComponent<SpriteRenderer>().color = playerSR.color;
			
			/* Instantiate fragment */
			Instantiate(fragmentPrefab, position, Quaternion.identity);
		}
	}
	
	public void AnimateSuccess()
	{
		/* Make player transparent */
		playerSR.color = player.GetComponent<GlobalVariables>().GetTransparent(playerSR.color);
	}
}
