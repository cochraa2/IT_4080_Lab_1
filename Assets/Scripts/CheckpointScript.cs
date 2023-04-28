using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CheckpointScript : MonoBehaviour
{
    private CarScript carHit;
    Vector3 respawnWorldLoc = new Vector3();
    Vector3 respawnLocalLoc = new Vector3(0, 0, 0);

    public GameObject spawnSphere;
    private List<CarScript> standingsList = new List<CarScript>();

    float respawnY;

    private void OnTriggerEnter(Collider other)
    {
        ulong ownerClientId = other.GetComponent<NetworkObject>().OwnerClientId;
        if (other.gameObject.CompareTag("DaCar"))
        {
            carHit = NetworkManager.Singleton.ConnectedClients[ownerClientId].PlayerObject.GetComponent<CarScript>();
            foreach(ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if(carHit.Lap.Value >= 1)
                {
                    Debug.Log("You Win!");
                    standingsList.Add(carHit);

                    respawnWorldLoc = spawnSphere.gameObject.transform.position;

                    carHit.transform.Translate(respawnWorldLoc);
                }
            }
        }
        
    }

    private void Update()
    {
        
    }
}
