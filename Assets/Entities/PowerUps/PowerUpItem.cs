using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class PowerUpItem : NetworkBehaviour {

    public GameObject powerUpPrefab;


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }


    void OnTriggerEnter2D(Collider2D collider) {

        if (!collider.gameObject || collider.gameObject.tag != "Player") return; //only players can collect
        PlayerController player = collider.gameObject.GetComponent<PlayerController>();

        // If player already has a powerup, replace it
        if (player.powerUp) {
            player.powerUp.WrapUp();
        }

        gameObject.GetComponent<Rigidbody2D>().isKinematic = true; // will stop moving
        gameObject.GetComponent<Collider2D>().enabled = false; // will no longer collide
        Extract(player); // apply powerUp to player
    }


    // Attach powerUpItem to player and exact powerUp from prefab and attach it to player as well
    public void Extract(PlayerController player) {
        player.item = this; 
        GameObject powerUpObject = Instantiate(powerUpPrefab) as GameObject; 
        if (NetworkServer.active && isServer && player.isLocalPlayer) NetworkServer.Spawn(powerUpObject);
        player.powerUp = powerUpObject.GetComponent<PowerUp>();
        player.powerUp.SetPlayer(player.gameObject);
        player.powerUp.Setup();
    }

    // Destroy powerUpItem
    public void Destroy() {
        if (isServer) NetworkServer.Destroy(gameObject);
        if (gameObject) Destroy(gameObject);
    }


}
