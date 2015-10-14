using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

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
            ipAddressUIText.GetComponent<Text>().text = GetCurrentIPAddress().ToString();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private static IPAddress GetCurrentIPAddress() {
        try {
            return GetLocalIPv4(NetworkInterfaceType.Wireless80211);
        }
        catch {
            return new IPAddress(0);
        }
    }

    private static IPAddress GetLocalIPv4(NetworkInterfaceType _type) {
        IPAddress output = null;
        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces()) {
            if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up) {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses) {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork) {
                        output = ip.Address;
                    }
                }
            }
        }
        return output;
    }

    public void CommenceMultiplayerGame() {
        multiplayerMenuCanvas.SetActive(false);
        EnemyController.instance.Enabled = true;
        PowerUpController.instance.Enabled = true;
    }


}
