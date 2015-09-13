using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class PowerUpItem : NetworkBehaviour {

    public GameObject powerUpPrefab;
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
        if (player.GetComponent<PlayerController>().powerUp) {

            player.GetComponent<PlayerController>().CmdDestroyPowerUp();
            if (player.GetComponent<PlayerController>().powerUp.gameObject) Destroy(player.GetComponent<PlayerController>().powerUp.gameObject);
            replaced = true;

        }

        gameObject.GetComponent<Rigidbody2D>().isKinematic = true; // will stop moving
        gameObject.GetComponent<Collider2D>().enabled = false; // will no longer collide
        
        Setup(player); // apply powerUp to player

    }

    // create powerup object from item and assign 


    public void Setup(GameObject player) {
        player.GetComponent<PlayerController>().item = this; // assign to player
        GameObject powerUpObject = Instantiate(powerUpPrefab) as GameObject; // load powerUp object
        if (NetworkServer.active) NetworkServer.Spawn(powerUpObject);
        player.GetComponent<PlayerController>().powerUp = powerUpObject.GetComponent<PowerUp>();
        player.GetComponent<PlayerController>().powerUp.SetPlayer(player);
        player.GetComponent<PlayerController>().powerUp.Setup();
        

    }


    public void Destroy() {
        if (isServer) NetworkServer.Destroy(gameObject);
        if (gameObject) Destroy(gameObject);
    }


}
