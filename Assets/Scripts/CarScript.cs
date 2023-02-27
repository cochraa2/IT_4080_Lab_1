using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CarScript : NetworkBehaviour
{
    public float speed = 20f;
    private float turnSpeed = 100f;
    public float horizontalInput;
    public float forwardInput;
    private Camera _camera;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _camera = transform.Find("Camera").GetComponent<Camera>();
        _camera.enabled = IsOwner;
    }


    void Update()
    {
        //Move the car around
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.left * Time.deltaTime * speed * forwardInput);
        transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime * horizontalInput);


    }
}
