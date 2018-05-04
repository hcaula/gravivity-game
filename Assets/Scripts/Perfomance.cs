using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perfomance : MonoBehaviour {
	
	public int framerate;
	private float deltaTime = 0.0f;

	void Start() 
	{
		Application.targetFrameRate = framerate;
	}
	
	void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
	}
 
	void OnGUI()
	{
		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, 0, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 4 / 100;
		style.normal.textColor = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string text = string.Format("{1:0.} fps", msec, fps);
		GUI.Label(rect, text, style);
	}
}
