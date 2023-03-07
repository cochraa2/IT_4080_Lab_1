using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletSpawner : NetworkBehaviour
{
    public Rigidbody bulletPrefab;
    private float bulletSpeed = 70f;
    private float timeToLive = 3f;

    [ServerRpc]
    public void FireServerRpc(ServerRpcParams rpcParams = default)
    {
            Rigidbody newBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            newBullet.gameObject.GetComponent<NetworkObject>().SpawnWithOwnership(rpcParams.Receive.SenderClientId);
            newBullet.velocity = transform.right * bulletSpeed;
            Destroy(newBullet.gameObject, timeToLive);
    }
}
