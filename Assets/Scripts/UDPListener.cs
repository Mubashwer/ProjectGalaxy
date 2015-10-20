using UnityEngine;
using System.Collections;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using UnityEngine.UI;

public class UDPListener : MonoBehaviour {

    Thread receiveThread;

    private readonly object infoLock = new object();
    private string lastData = "";

    // start from unity3d
    public void Start() {
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    public void Update() {
        var text = GameObject.Find("UDPTestText").GetComponent<Text>();
        lock(infoLock) {
            text.text = lastData;
        }
    }

    public void ReceiveData() {
        UdpClient client = new UdpClient(7778);

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
            catch (Exception e) {
                print(e.ToString());
            }
        }

    }
}
