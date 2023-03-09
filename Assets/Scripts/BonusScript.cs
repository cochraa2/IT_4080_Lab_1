using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BonusScript : NetworkBehaviour
{

    public GameObject bonusPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (IsServer)
        {
            if(collision.gameObject.tag == "DaCar")
            {
                Destroy(bonusPrefab);

                if(bonusPrefab == null)
                {
                    Debug.Log("yo, it's null!");
                }
            }
            
        }
    }
}
