using UnityEngine;
using System.Collections;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using UnityEngine.UI;

public class UDPListener : MonoBehaviour {
    private readonly object infoLock = new object();
    private string lastData = "";

    UdpClient client;

    // start from unity3d
    public void Start() {
        client = new UdpClient(7778);
        client.Client.ReceiveTimeout = 100;
        StartCoroutine(ReceiveDataCoRoutine());
    }

    public void Update() {
        var text = GameObject.Find("UDPTestText").GetComponent<Text>();
        lock (infoLock) {
            text.text = lastData;
        }
    }

    IEnumerator ReceiveDataCoRoutine() {
        while (true) {
            try {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);

                string text = Encoding.Unicode.GetString(data);

                print("UDP Received: " + text);
                lock (infoLock) {
                    lastData = text;
                }
            }
            catch (SocketException) {
            }
            catch (Exception e) {
                print(e.ToString());
            }
            yield return new WaitForSeconds(1);
        }
    }
}
