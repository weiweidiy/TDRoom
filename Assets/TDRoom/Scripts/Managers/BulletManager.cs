using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public class BulletManager
    {
        public event Action<BulletObject, EnemyObject> onBulletHit;
        public event Action<BulletObject> onBulletArrived;

        Dictionary<int, GameObject> bulletPrefabs;

        Dictionary<int, List<BulletObject>> aliveBullets = new Dictionary<int, List<BulletObject>>();

        List<BulletObject> bulletsToRemove = new List<BulletObject>();

        public BulletManager(Dictionary<int, GameObject> bulletPrefabs)
        {
            this.bulletPrefabs = bulletPrefabs;
        }

        public void SpawnBullet(int lineIndex, BulletData data, Vector3 startPos, Vector3 targetPos)
        {
            var prefab = GetPrefab(data.id);
            if (prefab == null)
            {
                Debug.LogError("BulletManager SpawnBullet 找不到子弹预制体 id=" + data.id);
                return;
            }

            var go = NetworkObjectPool.Singleton.GetNetworkObject(prefab, startPos, Quaternion.identity);
            go.Spawn();

            var bulletObject = go.GetComponent<BulletObject>();
            bulletObject.onArrived += BulletObject_onArrived;
            bulletObject.onHit += BulletObject_onHit;
            bulletObject.Init(data, targetPos);
            bulletObject.MoveTo(targetPos);

 


            if (!aliveBullets.ContainsKey(lineIndex))
            {
                var values = new List<BulletObject>();
                values.Add(bulletObject);
                aliveBullets.Add(lineIndex, values);
            }
            else
            {
                var values = aliveBullets[lineIndex];
                values.Add(bulletObject);
            }
        }

        public void RemoveBullet(int lineIndex, BulletObject bulletObject)
        {
            if (aliveBullets.ContainsKey(lineIndex))
            {
                bulletsToRemove.Add(bulletObject);

                //var values = aliveBullets[lineIndex];
                //values.Remove(bulletObject);
                //bulletObject.onArrived -= BulletObject_onArrived;
                //bulletObject.onHit -= BulletObject_onHit;
                //bulletObject.GetComponent<NetworkObject>().Despawn();
            }
        }

        private void BulletObject_onHit(BulletObject bulletObject, EnemyObject enemy)
        {
            onBulletHit?.Invoke(bulletObject, enemy);
        }

        private void BulletObject_onArrived(BulletObject bulletObject)
        {
            onBulletArrived?.Invoke(bulletObject);
        }

        private GameObject GetPrefab(int id)
        {
            return bulletPrefabs.ContainsKey(id) ? bulletPrefabs[id] : null;
        }

        public void UpdateLogic(float deltaTime)
        {
            foreach (var bullet in bulletsToRemove)
            {
                foreach (var kv in aliveBullets)
                {
                    var values = kv.Value;
                    if (values.Contains(bullet))
                    {
                        values.Remove(bullet);
                        bullet.onArrived -= BulletObject_onArrived;
                        bullet.onHit -= BulletObject_onHit;
                        bullet.GetComponent<NetworkObject>().Despawn();
                        break;
                    }
                }
            }

            bulletsToRemove.Clear();

            foreach (var bullets in aliveBullets.Values)
            {
                foreach (var bullet in bullets)
                {
                    bullet.UpdateLogic(deltaTime);
                }
            }
        }
    }
}
