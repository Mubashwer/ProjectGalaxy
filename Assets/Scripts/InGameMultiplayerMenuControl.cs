using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Net;


public class InGameMultiplayerMenuControl : MonoBehaviour {

    private GameObject multiplayerMenuCanvas;
    private GameObject ipAddressUIText;

    // Use this for initialization
    void Start () {
        multiplayerMenuCanvas = GameObject.Find("CanvasMultiplayerScreen");
        ipAddressUIText = GameObject.Find("IpAddressText");
        bool multiplayer = (GameManager.instance.CurrentGameMode == GameManager.GameMode.MultiPlayerHost);
        multiplayerMenuCanvas.SetActive(multiplayer);

        if (multiplayer) {
            ipAddressUIText.GetComponent<Text>().text = "Give this code to Player 2\n" +  Utilities.Base64Encode(Utilities.GetCurrentIPAddress().ToString());
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void CommenceMultiplayerGame() {
        multiplayerMenuCanvas.SetActive(false);
        EnemyController.instance.Enabled = true;
        PowerUpController.instance.Enabled = true;
    }


}
