using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class MainScript : NetworkBehaviour
{

    public It4080.NetworkSettings netSettings;
    public ChatServer chatServer;
    public It4080.Chat chat;

    // Start is called before the first frame update
    void Start()
    {
        chat.SystemMessage("Hello World");
        It4080.Chat.ChatMessage msg = new It4080.Chat.ChatMessage();
        msg.message = "foobar";

        netSettings.startServer += NetSettingsOnServerStart;
        netSettings.startHost += NetSettingsOnHostStart;
        netSettings.startClient += NetSettingsOnClientStart;

        netSettings.setStatusText("Not Connected");
        Debug.Log("hello world");
   
    }

    private void startClient(IPAddress ip, ushort port)
    {
        var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.ConnectionData.Address = ip.ToString();
        utp.ConnectionData.Port = port;

        NetworkManager.Singleton.StartClient();
        NetworkManager.Singleton.OnClientConnectedCallback += ClientOnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += ClientOnClientDisconnected;
        Debug.Log("start client");
    }

    private void startHost(IPAddress ip, ushort port)
    {
        var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.ConnectionData.Address = ip.ToString();
        utp.ConnectionData.Port = port;

        NetworkManager.Singleton.StartHost();
   

        NetworkManager.Singleton.OnClientConnectedCallback += HostOnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HostOnClientDisconnected;
        netSettings.setStatusText("You are the HOST, your ID is 0");

        Debug.Log("start host");
    }

    private void startServer(IPAddress ip, ushort port)
    {
        var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.ConnectionData.Address = ip.ToString();
        utp.ConnectionData.Port = port;

        NetworkManager.Singleton.StartServer();
        netSettings.setStatusText("You have started the SERVER");

        Debug.Log("start server");
    }

    private void printIs(string msg)
    {
        Debug.Log($"[{msg}] server: {IsServer} host: {IsHost} client: {IsClient} owner: {IsClient}");
    }

    // -----------------------
    // Events
    //------------------------

    private void HostOnClientConnected(ulong clientId)
    {
        Debug.Log($"Client Connected: {clientId}");
    }

    private void HostOnClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client Disconnected: {clientId}");
    }

    private void ClientOnClientConnected(ulong clientId)
    {
        netSettings.setStatusText($"Connected as Client Number: {clientId}");
    }

    private void ClientOnClientDisconnected(ulong clientId)
    {
        netSettings.setStatusText($"You have DISCONNECTED");
    }

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
