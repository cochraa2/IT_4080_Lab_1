using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HaltScript : NetworkBehaviour
{
    public NetworkVariable<bool> isSpeedPickedUp = new NetworkVariable<bool>(true);
    public NetworkVariable<bool> isHaltPickedUp = new NetworkVariable<bool>(true);
    private CarScript carPickedUp;
    private CarScript otherCars;

    public void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            if (other.gameObject.CompareTag("DaCar"))
            {
                

                ulong ownerClientId = other.GetComponent<NetworkObject>().OwnerClientId; 
                foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
                {   
                    if (ownerClientId == clientId)
                    {
                        carPickedUp = other.gameObject.GetComponent<CarScript>();
                       
                        Debug.Log("YOU SHOULD NOT STOP");

                    }
                    else
                    {
                        otherCars = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.GetComponent<CarScript>();
                        otherCars.carSpeed.Value = 0f;                       
                    }

                }
                Destroy(gameObject);
            }
        }
        
    }
}
