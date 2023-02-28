using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PowerUpScript : NetworkBehaviour
{
    public bool spawnOnLoad = true;

    private GameObject curPowerUp = null;
    private Transform spawnPointTransform;

    public float spawnDelay = 2f;

    public GameObject bonusPrefab;

    private void SpawnBonus()
    {
        spawnPointTransform = transform.Find("SpawnPoint");
    }

    public void Update()
    {
        if(IsServer && curPowerUp == null)
        {
            SpawnPowerUp();
        }
    }



    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            HostOnNetworkSpawn();
        }
    }

    private void HostOnNetworkSpawn()
    {
        if(bonusPrefab != null && spawnOnLoad)
        {
            SpawnPowerUp();
        }
    }


    private void SpawnPowerUp()
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.y = 1;
        GameObject pu = Instantiate(bonusPrefab, spawnPosition, Quaternion.identity);
        pu.GetComponent<NetworkObject>().Spawn();
        curPowerUp = pu;

    }


}
