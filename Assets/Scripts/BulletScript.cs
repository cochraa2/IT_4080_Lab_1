using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletScript : NetworkBehaviour
{
    public NetworkVariable<int> Damage = new NetworkVariable<int>(1);
}
