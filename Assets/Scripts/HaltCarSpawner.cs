using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HaltCarSpawner : NetworkBehaviour
{
    public bool spawnOnLoad = true;

    public GameObject haltPrefab;
    private GameObject serverPowerUp = null;
    private Transform spawnPointTransform;
    private GameObject instantiatedPowerUp;

    private float spawnDelay = 2f;
    private float timeAfterDestroyed = 0f;

    private HaltScript _bonusBoost;



    public void SpawnBonus()
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.y = 4;
        instantiatedPowerUp = Instantiate(haltPrefab, spawnPosition, Quaternion.identity);
        instantiatedPowerUp.GetComponent<NetworkObject>().Spawn();
        serverPowerUp = instantiatedPowerUp;

    }
    public void Update()
    {

        if (IsServer && serverPowerUp == null)
        {
            timeAfterDestroyed += Time.deltaTime;

            if (timeAfterDestroyed >= spawnDelay)
            {
                SpawnBonus();
                timeAfterDestroyed = 0f;
            }
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
        if (haltPrefab != null && spawnOnLoad)
        {
            SpawnBonus();
        }
    }
}
