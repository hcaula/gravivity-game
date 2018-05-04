using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuLevelPicker : MonoBehaviour {
	
	#region Public attributes
	public float pickupSpawnTime;
	public GameObject objectsToSpawn;
	public GameObject[] pickups;
	public AudioClip pickupRespawn;
	#endregion
	
	#region Private attributes
	private string currentLevelName;
	private string currentLevel;
	#endregion
	
	#region Gets and Sets
	public string GetCurrentLevelName () {return currentLevelName;}
	public string GetCurrentLevel () {return currentLevel;}
	#endregion
	
	void Awake ()
	{
		/* Get from memory the last level the player has played */
		LoadCurrentLevel();
	}
	
	public void SpawnObjects ()
	{
		objectsToSpawn.SetActive(true);
	}
	
	public IEnumerator SpawnPickups(float length, float extraTime)
	{
		/* Get audio component */
		AudioSource audio = this.GetComponent<AudioSource>();
		
		/* Wait until the camera animation finishes */
		yield return new WaitForSeconds(length);
		
		/* Spawn each pickup */
		int i = 0;
		foreach(GameObject pickup in pickups)
		{
			yield return new WaitForSeconds(pickupSpawnTime);
			
			/* Don't play last audio */
			if(i < 2) audio.PlayOneShot(pickupRespawn, 1);
			
			pickup.SetActive(true);
			i++;
		}
		
		/* If there is extra time, wait and change scene */
		yield return new WaitForSeconds(extraTime);
		this.GetComponent<MainMenuController>().ChangeScene();		
	}
	
	void LoadCurrentLevel ()
	{
		currentLevelName = "gravity, gravity";
		currentLevel = "Level #1";
	}
}
