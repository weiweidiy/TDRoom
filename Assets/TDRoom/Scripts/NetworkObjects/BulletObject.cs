using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Game
{
    public class BulletObject : NetworkBehaviour
    {
        public event Action<BulletObject> onArrived;
        public event Action<BulletObject, EnemyObject> onHit;

        public NetworkVariable<BulletData> data = new NetworkVariable<BulletData>();

        //BulletData data;
        //Vector3 targetPosition;
        bool isArrived = false;

        [SerializeField] private float clientLeadTime = 0.5f; // 客户端提前时间

        public void Init(BulletData data, Vector3 targetPosition)
        {
            //Debug.Log("子弹初始化: " + data.speed + " / " + transform.position + "/" + targetPosition);
            this.data.Value = data;
            //this.targetPosition = targetPosition;
            isArrived = false;
        }

        public BulletData GetData()
        {
            return data.Value;
        }

        public void MoveTo(Vector3 targetPos)
        {
            var temp = data.Value;
            temp.targetPosition = targetPos;
            data.Value = temp;
            MoveToClientRPC(targetPos, data.Value);
        }

        [Rpc(SendTo.ClientsAndHost)]
        public void MoveToClientRPC(Vector3 targetPos, BulletData data)
        {
            //targetPosition = targetPos;
            //Debug.Log("客户端收到子弹移动指令: " + data.speed + " / " + transform.position + "/" + targetPos);
            //this.data.Value = data;
        }

        public void Update()
        {
            if (!IsClient) return;

            float distance = Vector3.Distance(transform.position, data.Value.targetPosition);

            if (distance > 0.1f)
            {
                //Debug.Log("客户端：子弹移动中..." + data.speed + " / " + transform.position + "/" + targetPosition);
                float step = data.Value.speed * Time.deltaTime;
                var lastPos = Vector3.MoveTowards(transform.position, data.Value.targetPosition, step);
                transform.position = CalculatePredictedPosition(lastPos);
                isArrived = false;
                return;
            }
        }

        private Vector3 CalculatePredictedPosition(Vector3 lastPosition)
        {
            // 基于服务器最后位置 + 预测移动
            Vector3 direction = (lastPosition - transform.position).normalized;
            return transform.position + direction * data.Value.speed * (Time.deltaTime + clientLeadTime);
        }

        public void UpdateLogic(float deltaTime)
        {
            if (isArrived) return;
            // 只在服务器上移动，客户端由 NetworkTransform 同步

            if (!IsServer) return;
            //Debug.Log("服务器：子弹移动中...");
            // 如果距离很小则不再移动
            float distance = Vector3.Distance(transform.position, data.Value.targetPosition);
            if (distance > 0.1f)
            {
                float step = data.Value.speed * deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, data.Value.targetPosition, step);
                isArrived = false;
                return;
            }

            isArrived = true;
            //已经到了,通知出去
            onArrived?.Invoke(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!IsOwner || IsClient)
                return;

            //Debug.Log("子弹碰撞到: " + collision.gameObject.name);
            if (collision.GetComponent<EnemyObject>() is EnemyObject enemy)
            {
                //命中敌人
                onHit?.Invoke(this, enemy);
            }
        }


    }
}
