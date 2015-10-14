using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MenuControl : MonoBehaviour {

    private GameObject mainMenuCanvas;
    private GameObject multiplayerMenuCanvas;


    public void Start() {
        mainMenuCanvas = GameObject.Find("CanvasMain");
        multiplayerMenuCanvas = GameObject.Find("CanvasMultiplayer");
    }

    public void OnClickedSinglePlayer() {
        Debug.Log("SinglePlayer button clicked!");
        GameManager.instance.CurrentGameMode = GameManager.GameMode.SinglePlayer;
        Application.LoadLevel("Level_01");
        NetworkManager networkManager = NetworkManagerCustom.instance;
        networkManager.StartHost();
        EnemyController.instance.Enabled = true;
        PowerUpController.instance.Enabled = true;
    }

    public void OnClickedLocalMultiPlayer() {
        mainMenuCanvas.SetActive(false);
        multiplayerMenuCanvas.SetActive(true);
    }

    void OnGUI() {
        NetworkManager networkManager = NetworkManagerCustom.instance;

        int xpos = 10;
        int ypos = 40;
        int spacing = 24;

        if (!NetworkClient.active && !NetworkServer.active && networkManager.matchMaker == null) {
            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Host(H)")) {
                networkManager.StartHost();
            }
            ypos += spacing;

            if (GUI.Button(new Rect(xpos, ypos, 105, 20), "LAN Client(C)")) {
                networkManager.StartClient();
            }
            networkManager.networkAddress = GUI.TextField(new Rect(xpos + 100, ypos, 95, 20), networkManager.networkAddress);
            ypos += spacing;

            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Server Only(S)")) {
                networkManager.StartServer();
            }
            ypos += spacing;
        }
        else {
            if (NetworkServer.active) {
                GUI.Label(new Rect(xpos, ypos, 300, 20), "Server: port=" + networkManager.networkPort);
                ypos += spacing;
            }
            if (NetworkClient.active) {
                GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + networkManager.networkAddress + " port=" + networkManager.networkPort);
                ypos += spacing;
            }
        }
    }

    public void OnClickedInGameQuit() {
        NetworkManager networkManager = NetworkManagerCustom.instance;
        if (NetworkServer.active) {
            networkManager.StopServer();
        }
        if (NetworkClient.active) {
            networkManager.StopClient();
        }
        GameManager.instance.CurrentGameMode = GameManager.GameMode.None;
        Application.LoadLevel("Menu");
    }

    public void OnClickedMultiplayerMenuBack() {
        mainMenuCanvas.SetActive(true);
        multiplayerMenuCanvas.SetActive(false);
    }

    public void OnClickedMultiplayerMenuClient() {
        
    }

    public void OnClickedMultiplayerMenuServer() {
        Debug.Log("MultiPlayer button clicked!");
        GameManager.instance.CurrentGameMode = GameManager.GameMode.MultiPlayerHost;
        Application.LoadLevel("Level_01");
        NetworkManager networkManager = NetworkManagerCustom.instance;
        networkManager.StartHost();
    }


}
