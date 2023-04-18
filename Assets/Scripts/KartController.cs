using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartController : MonoBehaviour
{
    [SerializeField] WheelCollider FR;
    [SerializeField] WheelCollider FL;
    [SerializeField] WheelCollider BR;
    [SerializeField] WheelCollider BL;

    public float acceleration = 500f;
    public float brakingForce = 300f;

    private float currentAcceleration = 0f;
    private float currentBrakeForce = 0f;

    private void FixedUpdate()
    {
        //Get Forward/Reverse with W and S
        currentAcceleration = acceleration * Input.GetAxis("Vertical");


        //Hit Space to Brake
        if (Input.GetKey(KeyCode.Space))
        {
            currentBrakeForce = brakingForce;
        }
        else
        {
            currentBrakeForce = 0f;
        }

        //Apply Acceleration to Front Wheels

        FR.motorTorque = currentAcceleration;
        FL.motorTorque = currentAcceleration;

        FR.brakeTorque = currentBrakeForce;
        FL.brakeTorque = currentBrakeForce;
        BR.brakeTorque = currentBrakeForce;
        BL.brakeTorque = currentBrakeForce;
    }

}
