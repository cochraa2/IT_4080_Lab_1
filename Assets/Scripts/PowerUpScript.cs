using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PowerUpScript : NetworkBehaviour
{
    public bool spawnOnLoad = true;
    public float refreshTime = 2f;

    public GameObject bonusPrefab;

    private void SpawnBonus()
    {
        Vector3 spawnPosition = new Vector3(0, 0, 0);
    }

}
