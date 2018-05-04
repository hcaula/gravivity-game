using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoodbyeController : MonoBehaviour {
	
	public Text yourTime;
	public Text bestTime;
	public Text newRecord;
	public Text deaths;
	
	private GlobalVariables global;

	// Use this for initialization
	void Start () {
		global = this.GetComponent<GlobalVariables>();
		GameController gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		/* Convert to minutes display */
		string thisTime = global.secondsToMinutes(gameController.GetThisTime());
		string fastestTime = global.secondsToMinutes(gameController.GetFastestTime());
		
		yourTime.text += thisTime;
		bestTime.text += fastestTime;
		
		if(gameController.newRecord) newRecord.text = "new record!";
		else newRecord.text = "";
		
		deaths.text += PlayerPrefs.GetInt("deaths");
		
		PlayerPrefs.SetInt("deaths", 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void BackToMainMenu()
	{
		SceneManager.LoadScene("Main Menu");
	}
}
