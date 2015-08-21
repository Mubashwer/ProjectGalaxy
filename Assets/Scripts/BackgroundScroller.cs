using UnityEngine;
using System.Collections;

public class BackgroundScroller : MonoBehaviour {

	public float scrollingSpeed; //set it to different values for parallax effect
	
	// Divide asset resolution by pixels per unit and use editor
	public float imageHeight; 
	
	private Vector3 startPos;
	
	// Use this for initialization
	void Start () {
		
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		// One background image has to be joined with another for it to work
		// BG downwards at given speed until it reaches the same point in identical image
		// Then positions are reset and it repeats
		float newPos = Mathf.Repeat(Time.time * scrollingSpeed, imageHeight);
		transform.position = startPos + (Vector3.down * newPos);
	}
}
