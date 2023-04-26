using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CarScript : NetworkBehaviour
{
    public NetworkVariable<Vector3> PositionChange = new NetworkVariable<Vector3>();
    public NetworkVariable<Vector3> RotationChange = new NetworkVariable<Vector3>();
    public NetworkVariable<Color> VehicleColor = new NetworkVariable<Color>();
    public NetworkVariable<int> Score = new NetworkVariable<int>();
    public NetworkVariable<int> Lap = new NetworkVariable<int>();

    public NetworkVariable<float> carSpeed = new NetworkVariable<float>(75);
    public NetworkVariable<float> carTurnSpeed = new NetworkVariable<float>(175);

    private int lastCheckpoint;


    //private float speed = 50f;
    //private float turnSpeed = 175f;

    private float serverTimeLeft = 0f;
    private float serverBonusDuration = 3f;

    public TMPro.TMP_Text txtLapDisplay;
    public TMPro.TMP_Text txtSpeedDisplay;


    private float horizontalInput;
    private float forwardInput;

    private Camera _camera;
    private BulletSpawner _bulletSpawner;
    private PowerUpScript _bonusBoost;
    private BonusScript bonusTings;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _camera = transform.Find("Camera").GetComponent<Camera>();
        _camera.enabled = IsOwner;
        Lap.OnValueChanged += ClientOnLapChange;
        carSpeed.OnValueChanged += ClientOnSpeedChange;

        _bulletSpawner = transform
            .Find("BarrelTip")
            .transform.Find("BulletSpawner")
            .GetComponent<BulletSpawner>();
     
        netPlayerColor.OnValueChanged += OnPlayerColorChanged;
        DisplayLap();
        DisplaySpeed();
    }


    void Update()
    {
        if (!IsOwner)
        {
            return;   
        }
        ClickToChangeColor();
        ShootBullets();
        FlipYourCar();

        DecreaseMySpeed();
        

    }

    private void FixedUpdate()
    {
        if (IsOwner)
        {
            movementInputs();
            Vector3 goForward = new Vector3(forwardInput * Time.smoothDeltaTime, 0, 0);
            goForward *= carSpeed.Value;

            Vector3 turnCar = new Vector3(0, horizontalInput * Time.smoothDeltaTime, 0);
            turnCar *= carTurnSpeed.Value;


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

    private void FlipYourCar()
    {
        if (Input.GetKeyDown(KeyCode.F) && IsOwner)
        {
            GetComponent<Transform>().eulerAngles = new Vector3(0, 4, 0);

            transform.Rotate(0, 0, 0);
        }
    }

    private void ShootBullets()
    {
        if (IsOwner)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                _bulletSpawner.FireServerRpc();
            }
        }
        
    }

    // ------------
    // Score stuff
    // ------------

    public void OnCollisionEnter(Collision collision)
    {
        if (IsHost)
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                HostHandleBulletCollision(collision.gameObject);
            }
        }
    }
    private void HostHandleBulletCollision(GameObject carBullet)
    {
        BulletScript Bullet = carBullet.GetComponent<BulletScript>();
        Score.Value -= Bullet.Damage.Value;
        ulong ownerClientId = carBullet.GetComponent<NetworkObject>().OwnerClientId;
        CarScript otherPlayer = NetworkManager.Singleton.ConnectedClients[
            ownerClientId].PlayerObject.GetComponent<CarScript>();
        otherPlayer.Score.Value += Bullet.Damage.Value;
        Destroy(carBullet);
    }
    public void DisplayLap()
    {
        txtLapDisplay.text = "Lap: " + Lap.Value.ToString();
    }

    private void ClientOnLapChange(int previous, int current)
    {
        DisplayLap();
    }
    //-------------------------------------------
    // Speed Boost & PowerUp Things
    //-------------------------------------------

    private void ServerHandleSpeedPowerUp(GameObject pickupBall)
    {
        BonusScript Pickup = pickupBall.GetComponent<BonusScript>();
        carSpeed.Value += Pickup.increasedSpeed.Value;
        carTurnSpeed.Value += Pickup.increasedTurnSpeed.Value;
        ulong ownerClientId = gameObject.GetComponent<NetworkObject>().OwnerClientId;
        Debug.Log($"Powerup owner: {ownerClientId}");
        CarScript playerPickedUp = NetworkManager.Singleton.ConnectedClients[
            ownerClientId].PlayerObject.GetComponent<CarScript>();

        Destroy(pickupBall);

        
        //playerPickedUp._bulletSpawner.bulletSpeed += Pickup.increasedBulletSpeed.Value;
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            if (other.gameObject.CompareTag("BonusBoost"))
            {
                ServerHandleSpeedPowerUp(other.gameObject);
                serverTimeLeft += serverBonusDuration;

            }

            if (other.gameObject.CompareTag("Checkpoint"))
            {
                ulong ownerClientId = gameObject.GetComponent<NetworkObject>().OwnerClientId;
                CarScript playerhit = NetworkManager.Singleton.ConnectedClients[
                                        ownerClientId].PlayerObject.GetComponent<CarScript>();

                Debug.Log($"Checkpoint hit by ID: {ownerClientId}");
                HandleCheckpoints(other.gameObject.GetComponent<CheckpointScript>());
                Debug.Log("Last Checkpoint value: " + lastCheckpoint);


            }
        }
    }

    private void DecreaseMySpeed()
    {
        if (IsServer && serverTimeLeft > 0f)
        {
            serverTimeLeft -= Time.deltaTime;

            if (serverTimeLeft <= 0f && carSpeed.Value > 50f)
            {
                carSpeed.Value = 75f;
                carTurnSpeed.Value = 175;
                _bulletSpawner.bulletSpeed = 70f;
                serverTimeLeft = 0f;
            }
        }
    }

    private void DisplaySpeed()
    {
        txtSpeedDisplay.text = carSpeed.Value.ToString() + " MPH";
    }

    private void ClientOnSpeedChange(float previous, float current)
    {
        DisplaySpeed();
    }

    //---------------------
    // Checkpoint Handling
    //---------------------

    public void HandleCheckpoints(CheckpointScript checkpoint)
    {
        if(checkpoint.name == "CheckpointSingle" && lastCheckpoint == 0)
        {
            lastCheckpoint++;
        }
        if (checkpoint.name == "CheckpointSingle 2" && lastCheckpoint == 1)
        {
            lastCheckpoint++;
        }
        if (checkpoint.name == "CheckpointSingle 3" && lastCheckpoint == 2)
        {
            lastCheckpoint++;
        }
        if (checkpoint.name == "CheckpointSingle 4" && lastCheckpoint == 3)
        {
            lastCheckpoint++;
        }
        if (checkpoint.name == "CheckpointSingle 5" && lastCheckpoint == 4)
        {
            lastCheckpoint++;
        }
        if (checkpoint.name == "CheckpointSingle LAST" && lastCheckpoint == 5)
        {
            lastCheckpoint = 0;
            Lap.Value++;
        }
    }

    //------------------
    // Color Things
    //------------------

    private static Color[] availColors = new Color[] {
            Color.black, Color.blue, Color.cyan,
            Color.gray, Color.green, Color.yellow, Color.red, Color.magenta, Color.white, Color.clear };
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
        if (Input.GetKeyDown(KeyCode.C))
        {
            requestNextColorServerRpc();
        }
    }

    //---------------------------------------------------------

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
    void requestNextColorServerRpc(ServerRpcParams serverRpcParams = default)
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
