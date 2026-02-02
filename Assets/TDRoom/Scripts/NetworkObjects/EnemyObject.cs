using System;
using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public class EnemyObject : NetworkBehaviour, IUnit
    {
        public event Action<EnemyObject> onArrived;
        public event Action<EnemyObject, int> onHurt;
        public event Action<EnemyObject> onDead;

        EnemyData data;

        Vector3 targetPosition;

        bool isMoving = false;
        bool isArrived = false;
        bool isAttacking = false;

        ActionManager actionManager = new ActionManager();

        public UnitType UnitType => UnitType.Enemy;

        public int LineIndex => data.lineIndex;

        private void OnDisable()
        {
            isMoving = false;
            isArrived = false;
            isAttacking = false;
            actionManager.Clear();
        }

        public void Init(EnemyData data, IFinder finder)
        {
            this.data = data;
            actionManager.Init(data.skillDatas, finder, this);
        }

        public ulong GetNetworkObjectId()
        {
            return NetworkObjectId;
        }

        public EnemyData GetData()
        {
            return data;
        }

        public void MoveTo(Vector3 targetPos)
        {
            targetPosition = targetPos;
            isMoving = true;
        }

        public void StartAction(PlayerObject mainTarget)
        {
            isAttacking = true;
        }

        public void UpdateLogic(float deltaTime)
        {
            actionManager.Update(Time.deltaTime);

            if (isArrived) return;
            // 只在服务器上移动，客户端由 NetworkTransform 同步
            if (!IsServer) return;

            if (!isMoving) return;

            // 如果距离很小则不再移动
            float distance = Vector3.Distance(transform.position, targetPosition);
            if (distance > 0.1f)
            {
                float step = data.speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
                isArrived = false;
                return;
            }

            isArrived = true;
            //已经到了,通知出去
            onArrived?.Invoke(this);
        }

        public void Hurt(int damage)
        {
            data.hp = Mathf.Max(0, data.hp - damage);
            onHurt?.Invoke(this, damage);
            if (data.hp <= 0)
            {
                onDead?.Invoke(this);
            }
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}
