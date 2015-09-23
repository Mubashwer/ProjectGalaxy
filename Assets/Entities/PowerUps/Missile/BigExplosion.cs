using UnityEngine;
using System.Collections;

public class BigExplosion : Projectile {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Destroy(gameObject, 1);
    }

    public override void Hit() {
        Destroy(gameObject,1);
    }
}
