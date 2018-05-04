using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour {
	
	// void Start() {
// 		PlayerPrefs.DeleteAll();
// 	}
	
	#region Public functions
	public Color GetTransparent(Color color)
	{
		Color transp = color;
		transp.a = 0;
		return transp;
	}
	
	public int GetStringNumber(string s)
	{
		int hashPos = s.IndexOf("#");
		string numStr = s.Substring(hashPos + 1, s.Length - hashPos - 1);
		return int.Parse(numStr);
	}
	
	public string secondsToMinutes(float totalSeconds){
		int min = (int) Mathf.Floor(totalSeconds / 60);
		int sec = (int) totalSeconds - 60 * min;
		int milisec = (int) ((totalSeconds - Mathf.Floor(totalSeconds)) * 1000);

		string minutes = min.ToString();
		string seconds = sec.ToString();
		string miliseconds = milisec.ToString();
		if(min < 10) minutes = "0" + minutes;
		if(sec < 10) seconds = "0" + seconds;
		if(milisec < 10) miliseconds = "00" + miliseconds;
		else if(milisec < 100) miliseconds = "0" + miliseconds;
		
		return minutes + ":" + seconds + ":" + miliseconds;
	}
	#endregion
	
	#region Auxiliar functions
	int CharToInt(char c)
	{
		int ret = c - 48;
		return ret;
	}
	#endregion
	
	
}
