﻿using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	public float damage;
    public GameObject owner; //shooter

	public float GetDamage(){
		return damage;
	}

    public void Hit(){
		Destroy(gameObject,0.1f);
	}
}