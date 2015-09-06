using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	public float damage;
    public GameObject owner; //shooter

	public float GetDamage(){
		return damage;
	}

    public void Hit(){
		Destroy(gameObject,0.05f);
	}
}