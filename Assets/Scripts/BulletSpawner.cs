using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletSpawner : NetworkBehaviour
{
    public Rigidbody bulletPrefab;
    private float bulletSpeed = 40f;
    private float timeToLive = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ServerRpc]
    public void FireServerRpc(Color color, ServerRpcParams rpcParams = default)
    {
        Rigidbody newBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        GetComponent<NetworkObject>().SpawnWithOwnership(rpcParams.Receive.SenderClientId);
        newBullet.velocity = transform.forward * bulletSpeed;
        Destroy(newBullet.gameObject, timeToLive);
    }
}
