using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TrackCheckpoints : NetworkBehaviour
{
    //private List<CheckpointScript> checkpointSingleList;
    //private int nextCheckpointIndex;
    //private List<int> nextCheckpointIndexList;
    //private List<Transform> carTransforms = new List<Transform>();

    //public void Awake()
    //{
    //    NetworkManager networkManager = NetworkManager.Singleton;

    //    //Locate and name the checkpoints on the track

    //    Transform checkpointGroupTransform = transform.Find("CheckpointGroup");

    //    checkpointSingleList = new List<CheckpointScript>();

    //    foreach (Transform checkpointChildren in checkpointGroupTransform)
    //    {
    //        CheckpointScript checkpointSingle = checkpointChildren.GetComponent<CheckpointScript>();
    //        checkpointSingle.SetTrackCheckpoints(this);

    //        checkpointSingleList.Add(checkpointSingle);
    //    }

    //    nextCheckpointIndex = 0;
    //}

    //public void PlayerThroughCheckpoint(CheckpointScript checkpoint)
    //{
    //    Debug.Log(checkpoint.transform.name);
    //    if (checkpointSingleList.IndexOf(checkpoint) == nextCheckpointIndex)
    //    {
    //        //Correct Checkpoint
    //        Debug.Log("Correct");
    //        nextCheckpointIndex = (nextCheckpointIndex + 1) % checkpointSingleList.Count;
    //    }
    //    else
    //    {
    //        //Wrong Checkpoint
    //        Debug.Log("WRONG EEEEHHHHHHHH");
    //    }
    //}

}
