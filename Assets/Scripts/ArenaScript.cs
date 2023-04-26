using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ArenaScript : NetworkBehaviour
{
    public CarScript carPrefab;
    private List<Transform> carTransformList = new List<Transform>();
    private List<CheckpointScript> checkpointSingleList;
   // private int nextCheckpointIndex;
    private List<int> nextCheckpointIndexList;

    public NetworkVariable<int> nextCheckpointNetworked = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            SpawnAllPlayers();
            GetTheClientsServerRpc();
        }

        Transform checkpointGroupTransform = transform.Find("CheckpointGroup");

        checkpointSingleList = new List<CheckpointScript>();

        foreach (Transform checkpointChildren in checkpointGroupTransform)
        {
            CheckpointScript checkpointSingle = checkpointChildren.GetComponent<CheckpointScript>();
            checkpointSingle.SetTrackCheckpoints(this);

            checkpointSingleList.Add(checkpointSingle);
        }

        nextCheckpointIndexList = new List<int>();
        foreach (Transform carTransform in carTransformList)
        {
            nextCheckpointIndexList.Add(0);
        }

        nextCheckpointNetworked.Value = 0;
    }

    private void SpawnAllPlayers()
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            SpawnPlayerForClient(clientId);
        }
    }

    private CarScript SpawnPlayerForClient(ulong clientId)
    {
        // Pick a location to spawn the prefab at.  This is a simple
        // solution to illustrate setting the position.
        Vector3 spawnPosition = new Vector3(0, 5, clientId * 5);
        CarScript playerSpawn = Instantiate(
                                carPrefab,
                                spawnPosition,
                                Quaternion.identity);

        // This is what spawns the prefab over the network and assigns it as the
        // playerObject for clientId.
        playerSpawn.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        return playerSpawn;
    }

    private void Start()
    {
        // Locate and name the checkpoints on the track

        

    }

    public void PlayerThroughCheckpoint(CheckpointScript checkpoint, Transform theTransform)
    {
        
        nextCheckpointNetworked.Value = nextCheckpointIndexList[carTransformList.IndexOf(theTransform)];
        if (checkpointSingleList.IndexOf(checkpoint) == nextCheckpointNetworked.Value)
        {
            //Correct Checkpoint
            Debug.Log("Correct");
            nextCheckpointIndexList[carTransformList.IndexOf(theTransform)]
                = (nextCheckpointNetworked.Value + 1) % checkpointSingleList.Count;
        }
        else
        {
            //Wrong Checkpoint
            Debug.Log("WRONG EEEEHHHHHHHH");
        }
    }

    [ServerRpc]
    public void GetTheClientsServerRpc()
    {

        if (!IsServer && !IsHost)
        {
            return;
        }
        foreach (KeyValuePair<ulong, NetworkClient> kvp in NetworkManager.Singleton.ConnectedClients)
            {
                // Get the NetworkObject component for the client
                NetworkObject networkObject = kvp.Value.PlayerObject;

                // Get the Transform component for the client's object
                Transform clientTransform = networkObject.transform;

                // Add the Transform to the list
                carTransformList.Add(clientTransform);
            }

    }
}
