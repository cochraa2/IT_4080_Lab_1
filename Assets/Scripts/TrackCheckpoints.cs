using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TrackCheckpoints : NetworkBehaviour
{
    private CheckpointScript singleCheckpoint;

    public void Awake()
    {
        Transform checkpointGroupTransform = transform.Find(tag = "CheckpointGroup");
        //Transform checkpointSingleTransform = transform.Find(tag = "Checkpoint");

        foreach(Transform checkpointChildren in checkpointGroupTransform)
        {
            CheckpointScript checkpoint = checkpointChildren.GetComponent<CheckpointScript>();
            Debug.Log($"Checkpoint found:{checkpointChildren} ");
        }
    }

    public void PlayerThroughCheckpoint(CheckpointScript checkpoint)
    {
        Debug.Log(checkpoint.transform.name);
    }

}
