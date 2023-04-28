using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PowerUpScript : NetworkBehaviour
{
    public bool spawnOnLoad = true;

    public GameObject bonusPrefab;
    private GameObject serverPowerUp = null;
    private Transform spawnPointTransform;
    private GameObject instantiatedPowerUp;

    private float spawnDelay = 5f;
    private float timeAfterDestroyed = 0f;

    private BonusScript _bonusBoost;
    


    public void SpawnBonus()
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.y = 4;
        instantiatedPowerUp = Instantiate(bonusPrefab, spawnPosition, Quaternion.identity);
        instantiatedPowerUp.GetComponent<NetworkObject>().Spawn();
        serverPowerUp = instantiatedPowerUp;
        
    }

    public void Update()
    {

        if(IsServer && serverPowerUp == null)
        {
            timeAfterDestroyed += Time.deltaTime;

            if(timeAfterDestroyed >= spawnDelay)
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
        if(bonusPrefab != null && spawnOnLoad)
        {
            SpawnBonus();
        }
    }

}
