using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BonusScript : NetworkBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (IsServer)
        {
            Destroy(gameObject);
        }
    }
}
