using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    private TrackCheckpoints trackCheckpoints;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("DaCar"))
        {
            ;
        }
        
    }


    public void SetTrackCheckpoints(TrackCheckpoints theCheckpoints)
    {
        this.trackCheckpoints = theCheckpoints;
    }
}
