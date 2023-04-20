using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BonusScript : NetworkBehaviour
{

    public NetworkVariable<float> increasedSpeed = new NetworkVariable<float>(55);
    public NetworkVariable<float> increasedTurnSpeed = new NetworkVariable<float>(175);
    public NetworkVariable<float> increasedBulletSpeed = new NetworkVariable<float>(70);

   
}
