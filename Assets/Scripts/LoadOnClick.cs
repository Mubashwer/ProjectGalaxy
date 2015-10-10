using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LoadOnClick : MonoBehaviour {
    public void LoadScene(int index) {
        Application.LoadLevel(index);
    }

    public void StartSinglePlayerGame() {
        // setup parameters to enable single player only
        Debug.Log("Starting single player game");
        GameManager.instance.SinglePlayer = true;
        NetworkManager netman = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        netman.ServerChangeScene("Level_01");
        if (netman == null) {
            Debug.LogError("Network Manager is null");
            return;
        }

        netman.StopHost();
        var client = netman.StartHost();
        

    }

    public void StartMultiplayerGame() {
        // setup parameters to enable multiplayer menu
        GameManager.instance.SinglePlayer = false;
        Application.LoadLevel("Level_01");
    }

    public void OpenLeaderboard() {
        Application.LoadLevel("Leaderboard");
    }

    public void OpenMainMenu() {
        Application.LoadLevel("MainMenu");
    }
}
