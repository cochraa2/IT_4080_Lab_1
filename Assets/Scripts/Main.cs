using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class Main : NetworkBehaviour
{

    public It4080.NetworkSettings netSettings;

    // Start is called before the first frame update
    void Start()
    {
        netSettings.startServer += NetSettingsOnServerStart;
        netSettings.startHost += NetSettingsOnHostStart;
        netSettings.startClient += NetSettingsOnClientStart;
        Debug.Log("hello world");
   
    }

    private void startClient(IPAddress ip, ushort port)
    {
        var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.ConnectionData.Address = ip.ToString();
        utp.ConnectionData.Port = port;

        NetworkManager.Singleton.StartClient();

        Debug.Log("start client");
    }

    private void startHost(IPAddress ip, ushort port)
    {
        var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.ConnectionData.Address = ip.ToString();
        utp.ConnectionData.Port = port;

        NetworkManager.Singleton.StartHost();

        Debug.Log("start host");
    }

    private void startServer(IPAddress ip, ushort port)
    {
        var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.ConnectionData.Address = ip.ToString();
        utp.ConnectionData.Port = port;

        NetworkManager.Singleton.StartServer();

        Debug.Log("start server");
    }

    // -----------------------
    // Events 
    private void NetSettingsOnClientStart(IPAddress ip, ushort port)
    {
        startClient(ip, port);
    }

    private void NetSettingsOnServerStart(IPAddress ip, ushort port)
    {
        startServer(ip, port);
    }

    private void NetSettingsOnHostStart(IPAddress ip, ushort port)
    {
        startHost(ip, port);
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
