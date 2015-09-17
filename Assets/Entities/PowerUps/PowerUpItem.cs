using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class PowerUpItem : NetworkBehaviour {

    public GameObject powerUpPrefab;
    public PlayerController player;
    public int id;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter2D(Collider2D collider) {
        
        if (!collider.gameObject || collider.gameObject.tag != "Player") return; //only players can collect
        player = collider.gameObject.GetComponent<PlayerController>();
        
        // If player gets a new powerUp, destroy older one
        if (player.item && player.item.id != id) {
            player.item.Destroy();
        }
        if(player.powerUp && player.powerUp.GetId() != id) {
            player.powerUp.WrapUp();
        }
        
        gameObject.GetComponent<Rigidbody2D>().isKinematic = true; // will stop moving
        gameObject.GetComponent<Collider2D>().enabled = false; // will no longer collide
        Setup();
    }


    // Attach powerUpItem to player and exact powerUp from prefab and attach it to player as well
    public void Setup() {
        GameObject powerUpObject = Instantiate(powerUpPrefab) as GameObject;
        player.powerUp = powerUpObject.GetComponent<PowerUp>();
        player.powerUp.Setup(player.gameObject, id);

        // Only the local copy is associated with the player, the rest are destroyed
        if (player.isLocalPlayer) {
            player.item = this;
        }
        else {
            Destroy();
        }

    }

    public void Destroy() {
        Destroy(gameObject);
    }


}
