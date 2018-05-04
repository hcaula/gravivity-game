using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	
	#region Private attributes
	private GlobalVariables global;
	private float startTime;
	private float thisTime;
	private float fastestTime;
	#endregion
	
	#region Public attributes
	public int transitionTime;
	public int currentLevel;
	public AudioSource introAS;
	public AudioSource loopAS;
	public bool newRecord;
	#endregion
	
	#region Auxiliar functions
	string GetNextLevel()
	{
		return "Level #" + (currentLevel+1).ToString();
	}
	void DestroyIfExists()
	{
		GameObject[] controllers = GameObject.FindGameObjectsWithTag("GameController");
		/* If there's more than one, destroy the second one.
		This doesn't treat the case for more than two Game Controllers. */
		if (controllers.Length > 1) Destroy(controllers[1]);
	}
	#endregion
	
	#region Gets & Sets
	public float GetThisTime() {return this.thisTime;}
	public float GetFastestTime() {return this.fastestTime;}
	#endregion
	
	void Awake() 
	{
		/* So that this object persists between scenes */
		DontDestroyOnLoad(this.transform.gameObject);
		
		/* This assures that there can only be one Game Controller 
		per scene  We do this so we can test a level without having 
		to play from the beginning */
		DestroyIfExists();
		
		this.startTime = Time.time;
		
		#region Get components
		global = this.GetComponent<GlobalVariables>();
		#endregion
	}
	
  void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		/* If we are in a scene transition, wait and go to next level */
		if ((scene.name).Equals("Scene Transition"))
		{
			StartCoroutine(WaitAndChangeScene());
		}
		
		/* If we are in a level, get the current level number */
		else if((scene.name).Contains("Level"))
		{
			currentLevel = global.GetStringNumber(scene.name);
			/* To loop after the last level */
			
		} 
	}
	
	public void LevelToTransition()
	{
		if (currentLevel == 15) {
			TransitionToGoodbye();
		}
			
		
		else SceneManager.LoadScene("Scene Transition");
	}
	
	void TransitionToGoodbye()
	{	
		/* Calculate total time */
		float endTime = Time.time;
		this.thisTime = endTime - startTime;
		
		this.fastestTime = PlayerPrefs.GetFloat("fastestTime", -1.0f);
		if(this.thisTime < fastestTime || fastestTime < 0) {
			this.newRecord = true;
			PlayerPrefs.SetFloat("fastestTime", this.thisTime);
			PlayerPrefs.Save();
			} else this.newRecord = false;
			
		fastestTime = PlayerPrefs.GetFloat("fastestTime");
		
		SceneManager.LoadScene("Goodbye");
		StartCoroutine(FadeOutMusic());
	}
	
	IEnumerator WaitAndChangeScene()
	{
		
		yield return new WaitForSeconds(transitionTime);
		SceneManager.LoadScene(GetNextLevel());
	}
	
	IEnumerator FadeOutMusic()
	{
		while(introAS.volume > 0 || loopAS.volume > 0)
		{
			yield return new WaitForSeconds(0.1f);
			introAS.volume -= 0.1f;
			loopAS.volume -= 0.1f;
		}
		
		introAS.Stop();
		loopAS.Stop();
		
		introAS.volume = 1;
		loopAS.volume = 1;
	}
}
