using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CheckpointScript : NetworkBehaviour
{
    private CarScript carHit;
    

    public GameObject spawnSphere;
    private List<CarScript> standingsList = new List<CarScript>();

    private void OnTriggerEnter(Collider other)
    {
        Vector3 respawnWorldLoc = new Vector3();
        Vector3 respawnLocalLoc = new Vector3(0, 0, 0);
        
        if (other.gameObject.CompareTag("DaCar"))
        {
            ulong ownerClientId = other.GetComponent<NetworkObject>().OwnerClientId;
            carHit = NetworkManager.Singleton.ConnectedClients[ownerClientId].PlayerObject.GetComponent<CarScript>();
            foreach(ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if(carHit.Lap.Value > 1)
                {
                    Debug.Log("You Win!");
                    standingsList.Add(carHit);
                    carHit.txtLapDisplay.text = "Race Finish! Hit next Checkpoint!";

                    respawnWorldLoc = spawnSphere.gameObject.transform.position;

                    carHit.txtSpeedDisplay.text = "Welcome to Winners Island!";

                    carHit.HostHandleWrongCheckpoint(spawnSphere.GetComponent<CheckpointScript>());
                    carHit.transform.Translate(respawnWorldLoc);
                }
            }
        }
        
    }

    private void Update()
    {
        
    }
}
