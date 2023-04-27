using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HaltScript : NetworkBehaviour
{
    public NetworkVariable<bool> isPickedUp = new NetworkVariable<bool>(true);

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DaCar"))
        {
            Destroy(gameObject);
        }
    }
}
