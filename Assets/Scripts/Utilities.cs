using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

public class Utilities : MonoBehaviour {

    public static string Base64Encode(string plainText) {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    public static string Base64Decode(string base64EncodedData) {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }

    public static IPAddress GetCurrentIPAddress() {
        try {
            return GetLocalIPv4(NetworkInterfaceType.Wireless80211);
        }
        catch {
            return new IPAddress(0);
        }
    }

    public static IPAddress GetLocalIPv4(NetworkInterfaceType _type) {
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

}
