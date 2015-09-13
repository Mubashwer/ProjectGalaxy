using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class PowerUpItem : NetworkBehaviour {

    public GameObject powerUpPrefab;
    [SyncVar]
    public bool replaced = false;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }


    void OnTriggerEnter2D(Collider2D collider) {
       
        GameObject player = collider.gameObject;
         
        if (!player || player.tag != "Player") return; //only players can collect

        // If player already has a powerup, replace it
        if (!replaced && player.GetComponent<PlayerController>().powerUp) {

            player.GetComponent<PlayerController>().powerUp.WrapUp();
            replaced = true;

        }

        gameObject.GetComponent<Rigidbody2D>().isKinematic = true; // will stop moving
        gameObject.GetComponent<Collider2D>().enabled = false; // will no longer collide
        
        Setup(player.GetComponent<PlayerController>()); // apply powerUp to player

    }

    // create powerup object from item and assign 


    public void Setup(PlayerController player) {
        player.item = this; // assign to player
        GameObject powerUpObject = Instantiate(powerUpPrefab) as GameObject; // load powerUp object
        if (NetworkServer.active && player.isServer && powerUpObject.GetComponent<PowerUp>().isServer) NetworkServer.Spawn(powerUpObject);
        player.powerUp = powerUpObject.GetComponent<PowerUp>();
        player.powerUp.SetPlayer(player.gameObject);
        player.GetComponent<PlayerController>().powerUp.Setup();
        replaced = false;
    }


    public void Destroy() {
        if (isServer) NetworkServer.Destroy(gameObject);
        if (gameObject) Destroy(gameObject);
    }


}
