using System;
using UnityEngine;
using Unity.Netcode;

namespace Game
{
    [Serializable]
    public struct BulletData : INetworkSerializable
    {
        public int lineIndex;
        public int id;
        public float speed;
        public int damage;
        public Vector3 targetPosition;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref id);
            serializer.SerializeValue(ref speed);
            serializer.SerializeValue(ref damage);
            serializer.SerializeValue(ref targetPosition);
        }
    }
}
