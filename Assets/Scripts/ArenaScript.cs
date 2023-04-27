using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ArenaScript : NetworkBehaviour
{
    public CarScript carPrefab;
    public NetworkVariable<int> playerCount = new NetworkVariable<int>();

    private Vector3 minPosition = new Vector3(-27, 5, -102);
    private Vector3 maxPosition = new Vector3(-50, 5, -130);

    private NetworkVariable<float> serverTimer = new NetworkVariable<float>(0);

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            SpawnAllPlayers();
            serverTimer.Value = 125f;
        }

    }

    private void Update()
    {
        if (IsServer)
        {
            serverTimer.Value -= Time.deltaTime;
            //UpdateTimerClientRpc(serverTimer.Value);
        }
    }

    //[ClientRpc]
    //void UpdateTimerClientRpc(float value)
    //{
    //    serverTimer.Value = value;
    //}

    private void SpawnAllPlayers()
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            SpawnPlayerForClient(clientId);
        }
    }

    private CarScript SpawnPlayerForClient(ulong clientId)
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(minPosition.x, maxPosition.x),
            Random.Range(minPosition.y, maxPosition.y),
            Random.Range(minPosition.z - clientId, maxPosition.z));

        // Pick a location to spawn the prefab at.  This is a simple
        // solution to illustrate setting the position.
        Vector3 spawnPosition = randomPosition;
        CarScript playerSpawn = Instantiate(
                                carPrefab,
                                spawnPosition,
                                Quaternion.identity);

        // This is what spawns the prefab over the network and assigns it as the
        // playerObject for clientId.
        playerSpawn.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        return playerSpawn;
    }


}
