using System.Collections.Generic;
using UnityEngine;

public class BattleLineManager
{
    Dictionary<ulong, ushort> mapClientBattleLine = new Dictionary<ulong, ushort>();
    ushort curIndex = 0;
    public void InitBattleLineWithClient(ulong clientId)
    {
        mapClientBattleLine[clientId] = curIndex;
        curIndex++;
    }

    public ushort GetClientLineIndex(ulong clientId)
    {
        if (mapClientBattleLine.ContainsKey(clientId))
            return mapClientBattleLine[clientId];

        throw new System.Exception("没有找到客户端的battleLine数据：clientId:" + clientId);
    }

    public ulong GetClientIdByLineIndex(ushort lineIndex)
    {
        foreach (var kvp in mapClientBattleLine)
        {
            if (kvp.Value == lineIndex)
                return kvp.Key;
        }
        throw new System.Exception("没有找到对应线路的客户端ID：lineIndex:" + lineIndex);
    }

    public int GetLineCount()
    {
        return mapClientBattleLine.Count;
    }
}
