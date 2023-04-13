using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BonusScript : NetworkBehaviour
{

    public NetworkVariable<float> increasedSpeed = new NetworkVariable<float>(55);
    public NetworkVariable<float> increasedTurnSpeed = new NetworkVariable<float>(175);


    private void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            if(other.gameObject.tag == "DaCar")
            {
                Destroy(gameObject);

                if(gameObject == null)
                {
                    Debug.Log("yo, it's null!");
                }
            }
            
        }
    }
}
