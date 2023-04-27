using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CarScript : NetworkBehaviour
{
    public NetworkVariable<Vector3> PositionChange = new NetworkVariable<Vector3>();
    public NetworkVariable<Vector3> RotationChange = new NetworkVariable<Vector3>();
    public NetworkVariable<Color> VehicleColor = new NetworkVariable<Color>();
    public NetworkVariable<int> Score = new NetworkVariable<int>();
    public NetworkVariable<int> Lap = new NetworkVariable<int>();
    private NetworkVariable<bool> pickedUpHalt = new NetworkVariable<bool>(false);

    public NetworkVariable<float> carSpeed = new NetworkVariable<float>(75f);
    public NetworkVariable<float> carTurnSpeed = new NetworkVariable<float>(175f);

    public List<CheckpointScript> passedCheckpoints = new List<CheckpointScript>();

//    private int lastCheckpoint;


    //private float speed = 50f;
    //private float turnSpeed = 175f;

    private float serverTimeLeftSpeed = 0f;
    private float serverSpeedDuration = 3f;

    private float serverTimeLeftHalt = 0f;
    private float serverHaltDuration = 2f;

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

        carSpeed.Value = 75f;
        carTurnSpeed.Value = 175f;

        pickedUpHalt.Value = false;
    }


    void Update()
    {
        if (!IsOwner)
        {
            return;   
        }
        ClickToChangeColor();
        ShootBullets();

        

        if(pickedUpHalt.Value == true)
        {
            Debug.Log(pickedUpHalt.Value);
        }        

        if (serverTimeLeftHalt > 0)
        {
            Debug.Log(serverTimeLeftHalt);
        }
        

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
            DecreaseMySpeed();

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
                serverTimeLeftSpeed += serverSpeedDuration;

            }

            if (other.gameObject.CompareTag("Checkpoint"))
            {
                HandleCheckpoints(other.gameObject.GetComponent<CheckpointScript>());
            }

            if (other.gameObject.CompareTag("HaltBonus"))
            {
                serverTimeLeftHalt += serverHaltDuration;
                ServerHandleHalt(other.gameObject);
                
                
            }
        }
    }

    private void DecreaseMySpeed()
    {
        if (IsServer && serverTimeLeftSpeed > 0f)
        {
            serverTimeLeftSpeed -= Time.deltaTime;

            if (serverTimeLeftSpeed <= 0f && carSpeed.Value > 80f)
            {
                carSpeed.Value = 75f;
                carTurnSpeed.Value = 175f;
                _bulletSpawner.bulletSpeed = 70f;
                serverTimeLeftSpeed = 0f;
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
    // HALT BONUS
    //---------------------

    private void ServerHandleHalt(GameObject haltPickup)
    {
        HaltScript Pickup = haltPickup.GetComponent<HaltScript>();
        pickedUpHalt.Value = Pickup.isPickedUp.Value;

        Destroy(haltPickup);

    }

    public void ClientStopsAll()
    {
        if(IsServer && serverTimeLeftHalt > 0f)
        {
            serverTimeLeftHalt -= Time.deltaTime;

            if (serverTimeLeftHalt > 0f && pickedUpHalt.Value == false)
            {
                Debug.Log("ayo my speed should be halted, or 100");
                carSpeed.Value = 100f;
            }
            if (serverTimeLeftHalt <= 0f && carSpeed.Value == 0f)
            {
                carSpeed.Value = 75f;
                serverTimeLeftSpeed = 0f;
            }

            else if (pickedUpHalt.Value == true)
            {
                if (serverTimeLeftHalt <= 0f)
                {
                    carSpeed.Value = 200f;
                    pickedUpHalt.Value = false;
                }
            }
        }
        
    }


    //---------------------
    // Checkpoint Handling
    //---------------------

    public void HandleCheckpoints(CheckpointScript checkpoint)
    {
        if(checkpoint.name == passedCheckpoints.Count.ToString())
        {
            //lastCheckpoint++;
            passedCheckpoints.Add(checkpoint.GetComponent<CheckpointScript>());

            Debug.Log("Checkpoints passed: " + passedCheckpoints.Count);
            if (passedCheckpoints.Count >= 10)
            {
                //lastCheckpoint = 0;
                passedCheckpoints.Clear();
                //passedCheckpoints.Add(checkpoint.GetComponent<CheckpointScript>());
                Debug.Log("Checkpoints passed: " + passedCheckpoints.Count);
                Lap.Value++;
            }
        }
        else
        {
            HostHandleWrongCheckpoint();

            Debug.Log("Wrong checkpoint bruh");
        }
        
        
    }

    public void HostHandleWrongCheckpoint()
    { 

        Vector3 respawnWorldLoc = new Vector3();
        Vector3 respawnLocalLoc = new Vector3(0, 0, 0);

        Vector3 respawnRotation = new Vector3();
        float respawnY;

        

        int n = passedCheckpoints.Count;
        var item = passedCheckpoints[n - 1];
        respawnWorldLoc = item.GetComponent<CheckpointScript>().gameObject.transform.position;
        respawnY = item.GetComponent<CheckpointScript>().gameObject.transform.rotation.eulerAngles.y - transform.localEulerAngles.y;
        respawnRotation.y = respawnY;

        respawnWorldLoc.y -= 25;
        respawnLocalLoc = transform.InverseTransformPoint(respawnWorldLoc);

        if (IsOwner)
        {
            requestPositionToMoveServerRpc(respawnLocalLoc, respawnRotation);
        }

        if (!IsOwner || IsHost)
        {
            transform.Translate(respawnLocalLoc);
            transform.Rotate(respawnRotation);
        }

        Debug.Log(respawnWorldLoc);
        Debug.Log(respawnLocalLoc);
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
