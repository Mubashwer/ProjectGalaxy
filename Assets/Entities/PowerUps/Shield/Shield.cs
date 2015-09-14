using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Shield : PowerUp {

	// Use this for initialization
	void Start () {
        activated = true;
        deactivated = false;
       
    }
	
	// Update is called once per frame
	void Update () {
        CountDown();
        if (player && player.GetComponent<PlayerController>().isAlive) transform.position = player.transform.position; //sync
	}


    // Destroy bullets which somehow manage to hit the player
    public override void Defend(Collider2D collider) {
        Projectile bullet = collider.gameObject.GetComponent<Projectile>();
        if (bullet) {
            Destroy(bullet);
        }
    }

    // Destroy bullets which touch the shield
    void OnTriggerEnter2D(Collider2D collider) {
        Projectile bullet = collider.gameObject.GetComponent<Projectile>();
        if (bullet) {
            Destroy(collider.gameObject); // destroy bullet
            // hit animation
            GameObject hitEffect = Instantiate(Resources.Load("YellowBulletHit"), collider.transform.position, Quaternion.identity) as GameObject;
            hitEffect.transform.parent = transform;
            if (NetworkServer.active) NetworkServer.Spawn(hitEffect);
            Destroy(hitEffect, 0.9f);
        }
    }

    // Initialization of the shield: make it follow player
    public override void Setup() {
        transform.position = player.transform.position;
        transform.SetParent(player.transform);
    }

}
