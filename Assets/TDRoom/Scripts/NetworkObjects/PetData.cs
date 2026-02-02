using System;
using System.Collections.Generic;
using Unity.Netcode;

namespace Game
{
    [Serializable]
    public struct PetData : INetworkSerializable
    {
        public int petId;
        public int level;
        public List<ActionData> skillDatas;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref petId);
            serializer.SerializeValue(ref level);
        }
    }
}
