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
            trackCheckpoints.PlayerThroughCheckpoint(this);
        }
        
    }


    public void SetTrackCheckpoints(TrackCheckpoints trackCheckpoints)
    {
        this.trackCheckpoints = trackCheckpoints;
    }
}
