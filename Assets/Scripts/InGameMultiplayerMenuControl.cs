using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Net;
using UnityEngine.Networking;

public class InGameMultiplayerMenuControl : NetworkBehaviour {

    private GameObject multiplayerMenuCanvas;
    private GameObject ipAddressUIText;

    private GameObject multiplayerJoinMenuCanvas;
    private GameObject clientInputCode;
    // Use this for initialization
    void Start () {
        // For Host
        multiplayerMenuCanvas = GameObject.Find("CanvasMultiplayerScreen");
        ipAddressUIText = GameObject.Find("IpAddressText");
        bool multiplayer = (GameManager.instance.CurrentGameMode == GameManager.GameMode.MultiPlayerHost);
        multiplayerMenuCanvas.SetActive(multiplayer);

        try {
            if (multiplayer) {
                ipAddressUIText.GetComponent<Text>().text = "Give this code to Player 2:\n" +  Utilities.Base64Encode(Utilities.GetIP().GetAddressBytes());
               //Debug.Log(Utilities.Base64Decode(Utilities.Base64Encode(Utilities.GetIP())));
            }
        }
        catch{}
        finally {
    
            // For Client
            multiplayerJoinMenuCanvas = GameObject.Find("CanvasMultiplayerJoin");
            clientInputCode = GameObject.Find("JoinKey");
            multiplayer = (GameManager.instance.CurrentGameMode == GameManager.GameMode.MultiPlayerClient);
            multiplayerJoinMenuCanvas.SetActive(multiplayer);
            }
    }
    
    // Update is called once per frame
    void Update () {
    
    }


    public void CommenceMultiplayerGame() {
        multiplayerMenuCanvas.SetActive(false);
        LevelManager.instance.FindLocalPlayer().CmdSetSpawners(true);
    }

    public void OnClickedJoinGame() {
        NetworkManager networkManager = NetworkManagerCustom.instance;
        var code = clientInputCode.GetComponent<InputField>().text;
        networkManager.networkAddress = (new IPAddress(Utilities.Base64Decode(code))).ToString();
        networkManager.StartClient();
        multiplayerJoinMenuCanvas.SetActive(false);
    }
}
