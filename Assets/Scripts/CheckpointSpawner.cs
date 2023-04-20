using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class CheckpointSpawner : NetworkBehaviour
{
    public bool spawnOnLoad = true;

    public GameObject checkpointPrefab;
    private GameObject serverCheckpoint = null;
    private Transform spawnPointTransform;
    private GameObject instantiatedCheckpoint;

    private float spawnDelay = 2f;
    private float timeAfterDestroyed = 0f;

    private BonusScript _bonusBoost;


    //[ServerRpc]
    //public void SpawnCheckpointServerRpc(ServerRpcParams rpcParams = default)
    //{
    //    Vector3 spawnPosition = transform.position;
    //    spawnPosition.y -= 2;
    //    instantiatedCheckpoint = Instantiate(checkpointPrefab, spawnPosition, Quaternion.identity);
    //    //instantiatedCheckpoint.GetComponent<NetworkObject>();
    //    serverCheckpoint = instantiatedCheckpoint;
    //}

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            HostOnNetworkSpawn();
        }
    }

    private void HostOnNetworkSpawn()
    {
        if (checkpointPrefab != null && spawnOnLoad)
        {
            DoSomethingServerSide(NetworkManager.Singleton.LocalClientId);
        }
    }

    private void DoSomethingServerSide(ulong clientId)
    {
        // If isn't the Server/Host then we should early return here!
        if (!IsServer) return;


        // NOTE! In case you know a list of ClientId's ahead of time, that does not need change,
        // Then please consider caching this (as a member variable), to avoid Allocating Memory every time you run this function
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };

        // Let's imagine that you need to compute a Random integer and want to send that to a client
        Vector3 spawnPosition = transform.position;
        spawnPosition.y -= 2;
        instantiatedCheckpoint = Instantiate(checkpointPrefab, spawnPosition, Quaternion.identity);
        instantiatedCheckpoint.gameObject.GetComponent<NetworkObject>().Spawn();




        DoSomethingClientRpc(instantiatedCheckpoint, clientRpcParams);
    }

    [ClientRpc]
    private void DoSomethingClientRpc(NetworkObjectReference instantiatedCheck, ClientRpcParams rpcParams = default)
    {
        if (IsOwner)
        { return; }

        // Run your client-side logic here!!
        serverCheckpoint = instantiatedCheckpoint;
    }
}