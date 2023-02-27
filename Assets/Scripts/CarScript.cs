using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CarScript : NetworkBehaviour
{
    public NetworkVariable<Vector3> PositionChange = new NetworkVariable<Vector3>();
    public NetworkVariable<Vector3> RotationChange = new NetworkVariable<Vector3>();


    private float speed = .02f;
    private float turnSpeed = .2f;
    private float horizontalInput;
    private float forwardInput;

    private Camera _camera;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _camera = transform.Find("Camera").GetComponent<Camera>();
        _camera.enabled = IsOwner;
    }

    void Update()
    {
        //Move the cars around

        if (IsOwner)
        {
            movementInputs();
            Vector3 goForward = new Vector3(forwardInput, 0, 0);
            goForward *= speed;
            Vector3 turnCar = new Vector3(0, horizontalInput, 0);
            turnCar *= turnSpeed;
            requestPositionToMoveServerRpc(goForward, turnCar);
        }

        if (!IsOwner || IsHost)
        {
            transform.Translate(PositionChange.Value);
            transform.Rotate(RotationChange.Value);
        }


    }


    private void movementInputs()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");
    }

    //------------------
    //RPC
    //------------------

    [ServerRpc]

    void requestPositionToMoveServerRpc(Vector3 posChange, Vector3 rotChange)
    {
        if (!IsServer && !IsHost)
            return;

        PositionChange.Value = posChange;
        RotationChange.Value = rotChange;
    }

}
