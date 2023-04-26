using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CheckpointScript : NetworkBehaviour
{
    private TrackCheckpoints trackCheckpoints;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("DaCar"))
        {
            //NetworkObject netObject = GetComponent<NetworkObject>();
            //NetworkObject.NetworkHide(netObject);
        }
        
    }


    public void SetTrackCheckpoints(TrackCheckpoints theCheckpoints)
    {
        this.trackCheckpoints = theCheckpoints;
    }
}
