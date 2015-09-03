using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Projectile : NetworkBehaviour {
	
	public float damage;
    public GameObject owner; //shooter

	public float GetDamage(){
		return damage;
	}

    [ClientCallback]
    public void Hit(){
		Destroy(gameObject,0.1f);
	}
}