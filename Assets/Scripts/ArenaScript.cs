using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ArenaScript : NetworkBehaviour
{
    public CarScript carPrefab;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            SpawnAllPlayers();
        }
    }

    private void SpawnAllPlayers()
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            //SpawnPlayerForClient(clientId);
        }
    }

    private CarScript SpawnPlayerForClient(ulong clientId)
    {
        // Pick a location to spawn the prefab at.  This is a simple
        // solution to illustrate setting the position.
        Vector3 spawnPosition = new Vector3(0, 1, clientId * 5);
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
