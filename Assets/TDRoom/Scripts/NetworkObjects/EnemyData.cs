using System.Collections.Generic;

namespace Game
{
    public struct EnemyData
    {
        public ushort lineIndex;
        public string uid;
        public int enemyId;
        public float speed;
        public List<ActionData> skillDatas;
        public int hp;
        public int maxHp;
    }
}
