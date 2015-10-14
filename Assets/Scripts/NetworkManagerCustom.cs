using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkManagerCustom : NetworkManager {

    // Private reference for this class only
    private static NetworkManagerCustom _instance;

    //Public reference that other classes will use
    public static NetworkManagerCustom instance {
        get {
            // Get instance from scene if it hasn't been set
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<NetworkManagerCustom>();
                // Reuse in other scenes	
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    void Awake() {
        if (_instance == null) {
            // Make the first instance the singleton
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else {
            // Destroy this if another exists
            if (this != _instance)
                Destroy(this.gameObject);
        }
    }

    // called when a new player is added for a client
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        base.OnServerAddPlayer(conn, playerControllerId);

        if (numPlayers < 2) return; // Beyond this point is only for non-host client player

        // When a client joines the game, unspawned bullets and powerUps needs to be spawned in the
        // client scene as only events involving these objects are synced, and the events which created 
        // thes objects happened before client joined the game.

        GameObject[] gameObjects = FindObjectsOfType<GameObject>();
        PlayerController player = GameManager.instance.FindLocalPlayer();

        // Add unspawned existing gameObjects to client
        print(numPlayers);
        foreach (GameObject g in gameObjects) {
            NetworkBehaviour spawnable = g.GetComponent<NetworkBehaviour>();
            if (NetworkServer.active && spawnable && !spawnable.isServer && spawnable.tag != "Unspawnable") {
                NetworkServer.Spawn(g);

                // reconfigure powerUp in host player in client scene so that they can be used
                PowerUp powerUp = g.GetComponent<PowerUp>();
                if (powerUp) {
                    player.RpcPowerUpReSetup();
                }
            }
        }
    }

    public override void OnStopServer() {
        base.OnStopServer();
        StopGame();
    }

    public void StopGame() {
        EnemyController.instance.Enabled = false;
        PowerUpController.instance.Enabled = false;
    }



}
