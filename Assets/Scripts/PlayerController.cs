using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update() {
		FollowSwipe ();
	}
	
	void FollowSwipe() {
		if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved) { // Check for swipe
			Vector2 delta = Input.touches[0].deltaPosition; // touch displacement
			Vector3 delta3 = new Vector3(delta.x, delta.y, transform.position.z);
			// move in the direction and distance of touch displacement in world space
			transform.Translate(delta3 * Time.deltaTime, Space.World);
		} // otherwise move to left-click position of mouse
		else if (Input.touchCount <= 0 && Input.GetMouseButton(0)){
			Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
			transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
		}
	}
}
