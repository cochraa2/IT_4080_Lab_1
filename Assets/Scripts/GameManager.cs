using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    private ulong clientId;
    public NetworkList<PlayerInfo> allPlayers = new NetworkList<PlayerInfo>();
    public NetworkVariable<int> gameLaps = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        gameLaps.Value = 1;
    }

    public void GameOver()
    {
        if (IsServer)
        {
            foreach (PlayerInfo player in allPlayers)
            {
                var client = NetworkManager.Singleton.ConnectedClients[player.clientId].PlayerObject.GetComponent<CarScript>();

                clientId = player.clientId;
                if (client.Lap.Value >= gameLaps.Value)
                {

                    Debug.Log("Hooray! Game Completed!");

                }
                else
                {
                    //otherCars = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.GetComponent<CarScript>();
                    //otherCars.carSpeed.Value = 0f;
                }

            }
        }

    }
}

