using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PowerUpScript : NetworkBehaviour
{
    public bool spawnOnLoad = true;

    private GameObject curPowerUp = null;
    private Transform spawnPointTransform;
    private GameObject pu;

    private BonusScript _bonusBoost;

    public float spawnDelay = 2f;

    public GameObject bonusPrefab;

    public void SpawnBonus()
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.y = 4;
        pu = Instantiate(bonusPrefab, spawnPosition, Quaternion.identity);
        pu.GetComponent<NetworkObject>().Spawn();
        curPowerUp = pu;

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsServer)
        {
            if (collision.collider.gameObject.tag == "DaCar")
            {
                pu.GetComponent<NetworkObject>().Despawn();
                StartCoroutine(RespawnPowerUps());
                
            }
        }
       
    }

    IEnumerator RespawnPowerUps()
    {
        yield return new WaitForSeconds(spawnDelay);
        SpawnBonus();
    }



    public void Update()
    {
        //if(IsServer && curPowerUp == null)
        //{
        //    SpawnBonus();
        //}
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

    [ServerRpc]
    void ThePowerUpMethodServerRpc(ServerRpcParams rpcParams = default)
    {

    }

}
