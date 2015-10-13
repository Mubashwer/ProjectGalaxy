using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MenuControl : MonoBehaviour {

    public void OnClickedSinglePlayer() {
        Debug.Log("SinglePlayer button clicked!");
        Application.LoadLevel("Level_01");
        NetworkManager networkManager = NetworkManagerCustom.instance;
        networkManager.StartHost();
    }

    public void OnClickedLocalMultiPlayer() {
        Debug.Log("MultiPlayer button clicked!");
        Application.LoadLevel("Level_01");
        NetworkManager networkManager = NetworkManagerCustom.instance;
        networkManager.StartClient();
    }

    public void OnClickedBack() {
        NetworkManager networkManager = NetworkManagerCustom.instance;
        if (NetworkServer.active) {
            networkManager.StopServer();
        }
        if (NetworkClient.active) {
            networkManager.StopClient();
        }
        Application.LoadLevel("Menu");
    }
}
