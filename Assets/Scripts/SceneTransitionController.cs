using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionController : MonoBehaviour {
	
	#region Public attributes
	public Text levelName;
	#endregion
	
	#region Private attributes
	private GameController gameController;
	public string[] levelNames; // Ideally, you want to read a JSON or something here
	#endregion
	
	void Awake ()
	{
		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
	}
	
	void Start()
	{
		int currentLevel = gameController.currentLevel;
		levelName.text = levelNames[currentLevel];
	}
}
