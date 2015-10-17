using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Shield : PowerUp {

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (timerStarted) CountDownTime();
        if (player && player.GetComponent<PlayerController>().IsAlive) transform.position = player.transform.position; //sync
	}


    // Destroy bullets which touch the shield
    void OnTriggerEnter2D(Collider2D collider) {
        Projectile bullet = collider.gameObject.GetComponent<Projectile>();
        if (bullet) {
            Destroy(collider.gameObject); // destroy bullet
            // hit animation
            GameObject hitEffect = Instantiate(Resources.Load("YellowBulletHit"), collider.transform.position, Quaternion.identity) as GameObject;
            hitEffect.transform.parent = transform;
            Destroy(hitEffect, 0.9f);
        }
    }

    // Initialization of the shield: make it follow player
    public override void Setup(GameObject player, int id) {
        base.Setup(player, id);
        transform.position = player.transform.position;
        transform.SetParent(player.transform);
    }

}
