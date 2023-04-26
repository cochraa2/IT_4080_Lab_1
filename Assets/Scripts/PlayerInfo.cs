using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerInfo : INetworkSerializable, System.IEquatable<PlayerInfo>
{
    public  ulong clientId;

    public PlayerInfo(ulong id)
    {
        clientId = id;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
    }

    public bool Equals(PlayerInfo other)
    {
        throw new System.NotImplementedException();
    }


}
