﻿using UnityEngine;
using System.Collections;

public class HideMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    transform.parent.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}