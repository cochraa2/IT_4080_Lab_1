using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BonusScript : NetworkBehaviour
{

    public NetworkVariable<float> increasedSpeed = new NetworkVariable<float>(45);
    public NetworkVariable<float> increasedTurnSpeed = new NetworkVariable<float>(150);
    public NetworkVariable<float> increasedBulletSpeed = new NetworkVariable<float>(70);

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DaCar"))
        {
            Destroy(gameObject);
        }
    }


}
