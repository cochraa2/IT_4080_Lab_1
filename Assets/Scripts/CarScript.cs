using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CarScript : NetworkBehaviour
{
    //private static Color[] availColors = new Color[] {
    //        Color.black, Color.blue, Color.cyan,
    //        Color.gray, Color.green, Color.yellow };
    //private int hostColorIndex = 0;
    //public NetworkVariable netPlayerColor = new NetworkVariable();


    //public override void OnNetworkSpawn()
    //{
    //    netPlayerColor.OnValueChanged += OnPlayerColorChanged;
    //}


    //public void ApplyPlayerColor()
    //{
    //    GetComponent().material.color = netPlayerColor.Value;
    //}


    //public void OnPlayerColorChanged(Color previous, Color current)
    //{
    //    ApplyPlayerColor();
    //}


    //public void Update()
    //{
    //    if (Input.GetButtonDown("Fire1"))
    //    {
    //        RequestNextColorServerRpc();
    //    }
    //}

    //[ServerRpc]
    //void RequestNextColorServerRpc(ServerRpcParams serverRpcParams = default)
    //{
    //    hostColorIndex += 1;
    //    if (hostColorIndex > availColors.Length - 1)
    //    {
    //        hostColorIndex = 0;
    //    }

    //    Debug.Log($"host color index = {hostColorIndex} for {serverRpcParams.Receive.SenderClientId}");
    //    netPlayerColor.Value = availColors[hostColorIndex];
    //}
}
