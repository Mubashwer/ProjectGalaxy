﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	//  Variables for restricting movement
	private float xMin, xMax, yMin, yMax, padding = 0.5f;
	
	public GameObject shortBullet;
	public float shortBulletSpeed = 10;
	public float shortBulletShootRate = 0.15f;
	
	// Use this for initialization
	void Start () {
		
		float distFromCam = transform.position.z - Camera.main.transform.position.z;
		xMin = Camera.main.ViewportToWorldPoint (new Vector3(0,0,distFromCam)).x+padding;
		xMax = Camera.main.ViewportToWorldPoint (new Vector3(1,0,distFromCam)).x-padding;
		yMin = Camera.main.ViewportToWorldPoint (new Vector3(0,0,distFromCam)).y+padding;
		yMax = Camera.main.ViewportToWorldPoint (new Vector3(1,1,distFromCam)).y-padding;
	}
	
	
	// Update is called once per frame
	void Update() {
		
		// Fire bullet at a fixed rate if screen is touched or space is pressed
		if((Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) ||
			Input.GetKeyDown(KeyCode.Space)) {
			InvokeRepeating("ShootShortBullet", 0.00001f, shortBulletShootRate);
		}
		if(((Input.touchCount > 0 && (Input.touches[0].phase == TouchPhase.Ended ||
			Input.touches[0].phase == TouchPhase.Canceled) )) || 
			Input.GetKeyUp(KeyCode.Space)) {
			CancelInvoke("ShootShortBullet");
		}
		// Follow touch swipe or mouse left-click

		FollowSwipe ();
	}
	
	
	
	// Just shoot a bullet
	void ShootShortBullet() {
		Vector3 bulletPos = transform.position;
		bulletPos.y += 0.5f;
		GameObject laserBeam = Instantiate(shortBullet, bulletPos, Quaternion.identity) as GameObject;
		laserBeam.GetComponent<Rigidbody2D>().velocity = Vector3.up * shortBulletSpeed;		
	}
	
	// Move with same velocity as touch swipe
	private void FollowSwipe() {
		if(Input.touchCount > 0) { 
			// Check for any touch swipe
			for(int i = 0; i < Input.touchCount && Input.touches[i].phase == TouchPhase.Moved; i++) {
				Vector2 delta = Input.touches[i].deltaPosition; // touch displacement
				Vector3 delta3 = new Vector3(delta.x, delta.y, transform.position.z);
				// move in the direction and distance of touch displacement in world space
				transform.Translate(delta3 * Time.deltaTime, Space.World);
			}
		} 
		// otherwise move to left-click position of mouse
		else if (Input.touchCount <= 0 && Input.GetMouseButton(0)){
			Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
			transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
		}
		
		// Clamp/Restrict the player to the play space
		float newX = Mathf.Clamp (transform.position.x, xMin, xMax);
		float newY = Mathf.Clamp (transform.position.y, yMin, yMax);
		transform.position = new Vector3(newX, newY, transform.position.z);

	}

}
