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
        //spawnPointTransform = transform.Find("SpawnPoint");


        Vector3 spawnPosition = transform.position;
        //spawnPosition.Equals(transform.Find("SpawnPoint"));
        spawnPosition.y = 3;
        GameObject pu = Instantiate(bonusPrefab, spawnPosition, Quaternion.identity);
        pu.GetComponent<NetworkObject>().Spawn();
        curPowerUp = pu;
    }

    public void Update()
    {
        if(IsServer && curPowerUp == null)
        {
            SpawnBonus();
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


    //private void SpawnPowerUp()
    //{
    //    Vector3 spawnPosition = transform.position;
    //    spawnPosition.y = 0;
    //    spawnPosition.x = 0;
    //    GameObject pu = Instantiate(bonusPrefab, spawnPosition, transform.rotation);
    //    pu.GetComponent<NetworkObject>().Spawn();
    //    curPowerUp = pu;

    //}


}
