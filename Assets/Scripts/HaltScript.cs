using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HaltScript : NetworkBehaviour
{
    public NetworkVariable<bool> isPickedUp = new NetworkVariable<bool>(true);
    private CarScript theCar;

    private bool isColliding;

    private void Start()
    {
        theCar = GetComponent<CarScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsLocalPlayer && other.CompareTag("HaltBonus"))
        {
            isColliding = true;
            SetSpeeds();
        }
    }

    private void SetSpeeds()
    {
        if (IsLocalPlayer)
        {
            // Player colliding with item, maintain speed
            if (isColliding)
            {
                UpdateTheSpeedsClientRpc(theCar.carSpeed.Value);
            }
            // Player not colliding with item, set speed to 0
            else
            {
                UpdateTheSpeedsClientRpc(0f);
            }
        }
    }

    [ClientRpc]
    void UpdateTheSpeedsClientRpc(float speed)
    {
        theCar.carSpeed.Value = speed;
    }
}
