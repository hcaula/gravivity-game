using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour {

	#region Private attributes
	private bool swipeUp, swipeDown, swipeLeft, swipeRight;
	private bool isDraging = false;
	private Vector2 startTouch, swipeDelta;
	#endregion
	
	#region Public attributes
	public int deadzone;
	#endregion
	
	#region Gets and Sets
 	public Vector2 SwipeDelta { get { return swipeDelta; } }
	public bool SwipeLeft { get { return swipeLeft; } }
	public bool SwipeUp { get { return swipeUp; } }
	public bool SwipeRight { get { return swipeRight; } }
	public bool SwipeDown { get { return swipeDown; } }
	#endregion
	
	void Update() 
	{
		/* Reset every bool after every frame */
		swipeUp = swipeDown = swipeLeft = swipeRight = false;
		
		/* Standalone Inputs */
		if(Input.GetMouseButtonDown(0)) 
		{
			isDraging = true;
			startTouch = Input.mousePosition;
		} 
		else if (Input.GetMouseButtonUp(0)) 
		{
			Reset();
		}
		
		/* Mobile Inputs */
		if (Input.touches.Length > 0) 
		{
			if(Input.touches[0].phase == TouchPhase.Began) 
			{
				isDraging = true;
				startTouch = Input.touches[0].position;
			} 
			else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled) 
			{
				Reset();
			}
		}
		
		/* Calculate distance */
		swipeDelta = Vector2.zero;
		if (isDraging)
		{
			if(Input.touches.Length > 0) swipeDelta = Input.touches[0].position - startTouch;
			else if (Input.GetMouseButton(0)) swipeDelta = (Vector2)Input.mousePosition - startTouch;
		}
		
		/* Check if crossed the deadzone */
		if(swipeDelta.magnitude > deadzone)
		{
			/* Check direction */
			float x = swipeDelta.x;
			float y = swipeDelta.y;
			if(Mathf.Abs(x) > Mathf.Abs(y)) 
			{
				/* If we're here, then the swipe is horizontal */
				if (x < 0) swipeLeft = true;
				else swipeRight = true;
			}
			else 
			{
				/* If we're here, then the swipe is vertical */
				if (y < 0) swipeDown = true;
				else swipeUp = true;
			}
			
			Reset();
		}
			
	}
	
	void Reset() 
	{
		startTouch = swipeDelta = Vector2.zero;
		isDraging = false;
	}
}
