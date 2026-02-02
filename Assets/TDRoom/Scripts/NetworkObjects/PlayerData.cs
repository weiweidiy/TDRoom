using System;
using System.Collections.Generic;
using Unity.Netcode;

namespace Game
{

    [Serializable]
    public struct PlayerData : INetworkSerializable
    {
        public int hp;
        public int maxHp;
        public string playerName;
        public ushort lineIndex;
        public List<ActionData> skillDatas;
        public List<PetData> petDatas;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref playerName);
            serializer.SerializeValue(ref hp);
            serializer.SerializeValue(ref maxHp);
            serializer.SerializeValue(ref lineIndex);

            //// =========== 关键：序列化 List<ActionData> ===========
            //if (serializer.IsReader)
            //{
            //    // 读取模式：读取列表并反序列化
            //    skillDatas = new List<ActionData>();

            //    // 读取列表长度
            //    int listLength = 0;
            //    serializer.SerializeValue(ref listLength);

            //    // 读取每个技能数据
            //    for (int i = 0; i < listLength; i++)
            //    {
            //        ActionData skill = default;
            //        serializer.SerializeValue(ref skill);
            //        skillDatas.Add(skill);
            //    }
            //}
            //else
            //{
            //    // 写入模式：序列化列表
            //    // 写入列表长度
            //    int listLength = skillDatas?.Count ?? 0;
            //    serializer.SerializeValue(ref listLength);

            //    // 写入每个技能数据
            //    if (skillDatas != null && listLength > 0)
            //    {
            //        for (int i = 0; i < listLength; i++)
            //        {
            //            var skill = skillDatas[i];
            //            serializer.SerializeValue(ref skill);
            //        }
            //    }
            //}
        }
        // 辅助方法
        public void AddSkill(ActionData skill)
        {
            skillDatas ??= new List<ActionData>();
            skillDatas.Add(skill);
        }
    }
}
