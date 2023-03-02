using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CarScript : NetworkBehaviour
{
    public NetworkVariable<Vector3> PositionChange = new NetworkVariable<Vector3>();
    public NetworkVariable<Vector3> RotationChange = new NetworkVariable<Vector3>();
    public NetworkVariable<Color> VehicleColor = new NetworkVariable<Color>();


    private float speed = 50f;
    private float turnSpeed = 175f;
    private float horizontalInput;
    private float forwardInput;

    private Camera _camera;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _camera = transform.Find("Camera").GetComponent<Camera>();
        _camera.enabled = IsOwner;

        netPlayerColor.OnValueChanged += OnPlayerColorChanged;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (IsHost)
    //    {
    //        if (collision.gameObject.CompareTag("Bullet"))
    //        {
    //            HostHandleBulletCollision(collision.gameObject);
    //        }
    //    }
    //}

    //private void HostHandleBulletCollision(GameObject player)
    //{
    //    BulletScript bulletBoy = bulletBoy.getComponent
    //}

    void Update()
    {
        //Move the cars around

        //if (IsOwner)
        //{
        //    movementInputs();
        //    Vector3 goForward = new Vector3(forwardInput * Time.deltaTime, 0, 0);
        //    goForward *= speed;

        //    Vector3 turnCar = new Vector3(0, horizontalInput * Time.deltaTime, 0);
        //    turnCar *= turnSpeed;

        //    requestPositionToMoveServerRpc(goForward, turnCar);
        //}

        //if (!IsOwner || IsHost)
        //{
        //    transform.Translate(PositionChange.Value);
        //    transform.Rotate(RotationChange.Value);
        //}

        //ClickToChangeColor();


    }

    private void FixedUpdate()
    {
        if (IsOwner)
        {
            movementInputs();
            Vector3 goForward = new Vector3(forwardInput * Time.deltaTime, 0, 0);
            goForward *= speed;

            Vector3 turnCar = new Vector3(0, horizontalInput * Time.deltaTime, 0);
            turnCar *= turnSpeed;

            requestPositionToMoveServerRpc(goForward, turnCar);
        }

        if (!IsOwner || IsHost)
        {
            transform.Translate(PositionChange.Value);
            transform.Rotate(RotationChange.Value);
        }

        ClickToChangeColor();
        FlipYourCar();
    }


    private void movementInputs()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");
    }

    private void FlipYourCar()
    {
        if (Input.GetKeyDown("i"))
        {
            transform.Rotate(0, 0, 0);
        }
    }

    //------------------
    // Color Things
    //------------------

    private static Color[] availColors = new Color[] {
            Color.black, Color.blue, Color.cyan,
            Color.gray, Color.green, Color.yellow };
    private int hostColorIndex = 0;
    public NetworkVariable<Color> netPlayerColor = new NetworkVariable<Color>();



    public void ApplyPlayerColor()
    { 
        GetComponent<MeshRenderer>().material.color = netPlayerColor.Value;
        transform.Find("CarBody").GetComponent<MeshRenderer>().material.color = netPlayerColor.Value;
        transform.Find("RoofBack").GetComponent<MeshRenderer>().material.color = netPlayerColor.Value;
        transform.Find("RoofTop").GetComponent<MeshRenderer>().material.color = netPlayerColor.Value;

    }


    public void OnPlayerColorChanged(Color previous, Color current)
    {
        ApplyPlayerColor();
    }


    public void ClickToChangeColor()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RequestNextColorServerRpc();
        }
    }

    

    //------------------
    // RPC
    //------------------

    [ServerRpc]

    void requestPositionToMoveServerRpc(Vector3 posChange, Vector3 rotChange)
    {
        if (!IsServer && !IsHost)
            return;

        PositionChange.Value = posChange;
        RotationChange.Value = rotChange;
    }

    [ServerRpc]
    void RequestNextColorServerRpc(ServerRpcParams serverRpcParams = default)
    {
        if (!IsServer && !IsHost)
            return;

        hostColorIndex += 1;
        if (hostColorIndex > availColors.Length - 1)
        {
            hostColorIndex = 0;
        }

        Debug.Log($"host color index = {hostColorIndex} for {serverRpcParams.Receive.SenderClientId}");
        netPlayerColor.Value = availColors[hostColorIndex];
    }

}
