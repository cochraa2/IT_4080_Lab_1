using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ChatServer : NetworkBehaviour
{
    public It4080.Chat chat;

    //private ulong[] singleClientId = new ulong[1];
    //private ulong[] dmClientIds = new ulong[2];

    //[ServerRpc]
    //public void SendSystemMessageServerRpc(string message, ulong to, ServerRpcParams serverRpcParams)
    //{
    //    ClientRpcParams rpcParams = default;
    //    rpcParams.Send.TargetClientIds = singleClientId;
    //    singleClientId[0] = to;
    //}

    //[ClientRpc]
    //public void ReceiveMessageClientRpc(string message, string from, ClientRpcParams clientRpcParams)
    //{

    //}

    // Update is called once per frame
    void Update()
    {

    }
}
