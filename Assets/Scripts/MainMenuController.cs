using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	#region Public attributes
	public float maxSpeed; // The max speed the menu player can fall
	public float offset; // The offset for the camera after the zoom animation
	public float extraTime; // Time between last pickup spawn and changing scene
	public Text levelName;
	public AnimationClip fadeOutMenu;
	public AnimationClip mainToSelector;
	public AnimationClip selectorToMain;
	public AnimationClip showLevelName;
	public GameObject player;
	#endregion
	
	#region Private attributes
	private Animation animationComponent;
	private Camera cam;
	private MenuPlayerController playerController;
	private MainMenuLevelPicker levelPicker;
	private MenuCameraController cameraController;
	private AudioSource audioSource;
	#endregion
	
	#region Auxiliar functions
	void DeactivateButton (string button)
	{
		Button startGame = GameObject.Find(button).GetComponent<Button>();
		startGame.interactable = false;	
	}
	#endregion
	
	/* To make sure the zoom out animation only plays once */
	bool animateGetBack;
	
	void Awake()
	{
		#region Components and objects
		levelPicker = this.GetComponent<MainMenuLevelPicker>();
		GameObject canvas = GameObject.FindWithTag("Canvas");
		animationComponent = canvas.GetComponent<Animation>();
		cameraController = this.GetComponent<MenuCameraController>();
		audioSource = this.GetComponent<AudioSource>();
		playerController = player.GetComponent<MenuPlayerController>();
		#endregion
	}
	
	void Start () 
	{
		animateGetBack = true;
		levelName.text += levelPicker.GetCurrentLevelName();
	}
	
	void Update ()
	{
		/* After the player hits the ground */
		if (playerController.GetBounceCount() == 1 && animateGetBack) 
		{
			levelPicker.SpawnObjects();
			cameraController.Play("Get Back");
			animateGetBack = false;
			StartCoroutine(levelPicker.SpawnPickups(cameraController.getBack.length, extraTime));
		}
	}
	
	public void ChangeScene()
	{
		SceneManager.LoadScene(levelPicker.GetCurrentLevel());
	}
	
	public void StartGame()
	{
		/* Deactivates buttons */
		DeactivateButton("Start Game");
		
		/* Set menu player active so it can start falling */
		player.SetActive(true);
		
		/* Play changing background color animation */
		cameraController.Play("Change Color");
		
		/* After the fade out animation, show the current level name */
		animationComponent.PlayQueued(fadeOutMenu.name, QueueMode.PlayNow);
		animationComponent.PlayQueued(showLevelName.name, QueueMode.CompleteOthers);
		
		/* Fade out main menu music */
		StartCoroutine(FadeoutMusic());
	}
	
	IEnumerator FadeoutMusic()
	{
		while(audioSource.volume > 0)
		{
			yield return new WaitForSeconds(0.1f);
			audioSource.volume -= 0.03f;
		}
		
		/* After the volume reaches zero, stop playing the music
		and set the volume back to 1 for other sound effects */
		audioSource.Stop();
		audioSource.volume = 1;
	}
	
	#region Canvas animations
	public void MainToSelector()
	{
		animationComponent.clip = mainToSelector;
		animationComponent.Play();
	}
	
	public void SelectorToMain()
	{
		animationComponent.clip = selectorToMain;
		animationComponent.Play();
	}
	#endregion
	
}
