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
	            ipAddressUIText.GetComponent<Text>().text = "Give your IP to Player 2\n" + Utilities.GetCurrentIPAddress().ToString();  //Utilities.Base64Encode(Utilities.GetCurrentIPAddress().ToString());
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
        //var hack = Utilities.Base64Encode("192.168.1.1");
        //Debug.Log(code);
        networkManager.networkAddress = code;
        networkManager.StartClient();
        multiplayerJoinMenuCanvas.SetActive(false);
    }
}
