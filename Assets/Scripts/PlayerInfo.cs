using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public struct PlayerInfo : INetworkSerializable, System.IEquatable<PlayerInfo>
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
        return other.clientId == clientId;
    }


}
