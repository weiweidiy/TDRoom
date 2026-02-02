using System;
using Unity.Netcode;

namespace Game
{
    [Serializable]
    public struct ActionData : INetworkSerializable
    {
        //public string skillName;
        public int skillLevel;
        public int actionId;
        public float cd;

        public ActionData(int id, int level, float cd)
        {
            actionId = id;
            skillLevel = level;
            this.cd = cd;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            // 序列化技能名
            serializer.SerializeValue(ref actionId);

            // 序列化技能等级
            serializer.SerializeValue(ref skillLevel);
            serializer.SerializeValue(ref cd);
        }
    }
}
