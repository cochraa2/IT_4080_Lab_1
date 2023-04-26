using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CheckpointScript : MonoBehaviour
{
    private ArenaScript trackCheckpoints;

        //public int checkpointCount;
        //private int lapCount = 1;



    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("DaCar"))
        {
            //trackCheckpoints.PlayerThroughCheckpoint(this, other.gameObject.transform);
        }
        
    }

    private void Update()
    {
        
    }

    //public void SetTrackCheckpoints(ArenaScript trackCheckpoints)
    //{
    //    this.trackCheckpoints = trackCheckpoints;
    //}


  
}
