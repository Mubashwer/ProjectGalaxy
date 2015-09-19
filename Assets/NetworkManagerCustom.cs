using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkManagerCustom : NetworkManager {

    // called when a new player is added for a client
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        base.OnServerAddPlayer(conn, playerControllerId);
        print("CONNECTED");
        GameObject[] gameObjects = FindObjectsOfType<GameObject>();
        foreach(GameObject g in gameObjects) {
            NetworkBehaviour spawnable = g.GetComponent<NetworkBehaviour>();
            if (spawnable && spawnable.tag != "Unspawnable") {
                if (NetworkServer.active) NetworkServer.Spawn(g);
            }
        }

    }

}
