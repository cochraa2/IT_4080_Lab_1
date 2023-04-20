using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CheckpointScript : NetworkBehaviour
{
    private TrackCheckpoints trackCheckpoints;

    private void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            if (other.gameObject.tag == "DaCar")
            {
                ;
            }
        }
        
    }


    public void SetTrackCheckpoints(TrackCheckpoints theCheckpoints)
    {
        this.trackCheckpoints = theCheckpoints;
    }
}