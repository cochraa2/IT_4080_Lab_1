using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CheckpointScript : MonoBehaviour
{
    private ArenaScript trackCheckpoints;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("DaCar"))
        {
            trackCheckpoints.PlayerThroughCheckpoint(this, other.gameObject.transform);
        }
        
    }

    public void SetTrackCheckpoints(ArenaScript trackCheckpoints)
    {
        this.trackCheckpoints = trackCheckpoints;
    }

}
