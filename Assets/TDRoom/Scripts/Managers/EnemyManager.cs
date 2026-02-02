
using Cysharp.Threading.Tasks;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// 每隔一定时间，自动生成怪物
/// </summary>
public class EnemyManager
{
    public event Action<EnemyObject> onEnemyArrived;
    public event Action<EnemyObject, int> onEnemyHurt;
    public event Action<EnemyObject> onEnemyDead;
    public event Action onAllEnemiesDead;

    Dictionary<int, GameObject> enemiesPrefabs;
    MapManager mapManager;

    IFinder finder;

    Dictionary<int, List<EnemyObject>> aliveEnemies = new Dictionary<int, List<EnemyObject>>();

    List<EnemyObject> enemiesToRemove = new List<EnemyObject>();

    bool isSpawning = false;

    public EnemyManager(Dictionary<int, GameObject> enemiesPrefabs, MapManager mapManager, IFinder finder)
    {
        this.enemiesPrefabs = enemiesPrefabs;
        this.mapManager = mapManager;
        this.finder = finder;
    }

    /// <summary>
    /// 开始间隔创建怪物
    /// </summary>
    /// <param name="lineIndex"></param>
    /// <param name="enemies"></param>
    public async UniTask SpawnEnemiesByInterval(ushort lineIndex, List<EnemyData> enemies)
    {
        if (enemies == null || enemies.Count == 0)
            throw new System.Exception("怪物列表ID为空");

        if (mapManager == null)
            throw new System.Exception("地图管理器为空");

        var pos = mapManager.GetDoorPosition(lineIndex);
        var targetPos = mapManager.GetSeatPosition(lineIndex) + new Vector3(0, -0.2f, 0);

        if (enemies.Count > 0)
            isSpawning = true;

        foreach (var enemyData in enemies)
        {
            var prefab = GetPrefab(enemyData.enemyId);
            if (prefab == null)
            {
                Debug.LogError($"未找到怪物预制体，ID: {enemyData}");
                continue;
            }

            var go = NetworkObjectPool.Singleton.GetNetworkObject(prefab, pos, Quaternion.identity);
            go.Spawn();
            //var go = prefab.SpawnNetworkObject(pos, Quaternion.identity);
            var enemyObject = go.GetComponent<EnemyObject>();
            enemyObject.onArrived += EnemyObject_onArrived;
            enemyObject.onHurt += EnemyObject_onHurt;
            enemyObject.onDead += EnemyObject_onDead;
            enemyObject.Init(enemyData, finder);
            enemyObject.MoveTo(targetPos);

            if (!aliveEnemies.ContainsKey(lineIndex))
            {
                var values = new List<EnemyObject>();
                values.Add(enemyObject);
                aliveEnemies.Add(lineIndex, values);
            }
            else
            {
                var values = aliveEnemies[lineIndex];
                values.Add(enemyObject);
            }


            // 这里可以添加延时逻辑，控制生成间隔
            await UniTask.Delay(500);
        }

        isSpawning = false;
    }

    private void EnemyObject_onDead(EnemyObject enemyObject)
    {
        onEnemyDead?.Invoke(enemyObject);
    }

    private void EnemyObject_onHurt(EnemyObject enemyObject, int damage)
    {
        onEnemyHurt?.Invoke(enemyObject, damage);
    }

    public void RemoveEnemy(EnemyObject enemy)
    {
        if (aliveEnemies.ContainsKey(enemy.LineIndex))
        {
            //enemiesToRemove.Add(enemy);

            var enemies = aliveEnemies[enemy.LineIndex];
            enemies.Remove(enemy);

            //aliveEnemies.Remove(enemy.GetData().lineIndex);
            enemy.onArrived -= EnemyObject_onArrived;
            enemy.onHurt -= EnemyObject_onHurt;
            enemy.onDead -= EnemyObject_onDead;

            var component = enemy.GetComponent<NetworkObject>();
            component.Despawn();

            //NetworkObjectPool.Singleton.ReturnNetworkObject(component, enemy.gameObject);
            //enemy.GetComponent<NetworkObject>().Despawn();
        }

    }

    private void EnemyObject_onArrived(EnemyObject enemy)
    {
        onEnemyArrived?.Invoke(enemy);
    }

    public void SpawnEnemyImmediatly(int lineIndex, int enemy)
    {

    }

    GameObject GetPrefab(int enemyId)
    {
        if (enemiesPrefabs.ContainsKey(enemyId))
        {
            return enemiesPrefabs[enemyId];
        }
        return null;
    }

    public List<EnemyObject> GetAllEnemies()
    {
        return aliveEnemies.Values.SelectMany(list => list).ToList();
    }

    public void UpdateLogic(float deltaTime)
    {
        //foreach (var enemy in enemiesToRemove)
        //{
        //    if (aliveEnemies.ContainsKey(enemy.LineIndex))
        //    {
        //        var enemies = aliveEnemies[enemy.LineIndex];
        //        enemies.Remove(enemy);
        //        enemy.onArrived -= EnemyObject_onArrived;
        //        enemy.onHurt -= EnemyObject_onHurt;
        //        enemy.onDead -= EnemyObject_onDead;
        //        enemy.GetComponent<NetworkObject>().Despawn();
        //    }
        //}

        foreach (var enemyList in aliveEnemies.Values)
        {
            foreach (var enemy in enemyList)
            {
                enemy.UpdateLogic(deltaTime);
            }
        }

        if (!isSpawning && aliveEnemies.Values.All(list => list.Count == 0))
        {
            onAllEnemiesDead?.Invoke();
        }
    }
}
