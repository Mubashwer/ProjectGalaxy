using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System;

public class Utilities : MonoBehaviour {

    public static string Base64Encode(byte[] bytes) {
        return Convert.ToBase64String(bytes);
    }

    public static byte[] Base64Decode(string base64EncodedData) {
        return Convert.FromBase64String(base64EncodedData);
    }


    //public static IPAddress GetIP() {
    //    var host = Dns.GetHostEntry(Dns.GetHostName());
    //    foreach (var ip in host.AddressList) {
    //        var ipBytes = ip.GetAddressBytes();
    //        if (ip.AddressFamily == AddressFamily.InterNetwork && ipBytes.Length ==  4 && ipBytes[3] != 1) {
    //            return ip;
    //        }
    //    }
    //    return IPAddress.None;
    //}


}
