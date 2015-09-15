using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Projectile : NetworkBehaviour {
	
	public float damage;
    public float speed;
    public GameObject owner; //shooter


	public float GetDamage(){
		return damage;
	}

    public virtual void Hit(){
		Destroy(gameObject,0.05f);
	}
}