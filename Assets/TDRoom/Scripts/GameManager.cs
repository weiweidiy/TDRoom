using Game;
using JFramework;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour, IFinder
{
    public class EventWaveChanged : JFramework.Event { }
    public class EventEnemyHurt : JFramework.Event { }

    public class EventInit : JFramework.Event { }


    [SerializeField] private int requiredPlayers = 2; // 可配置所需玩家数

    private bool gameStarted = false; // 防止重复执行

    public EventManager EventManager = new EventManager();

    /// <summary>
    /// 波次管理器
    /// </summary>
    WaveManager waveManager;
    /// <summary>
    /// 战线管理器
    /// </summary>
    BattleLineManager battleLineManager;
    /// <summary>
    /// 地图管理器
    /// </summary>
    MapManager mapManager;
    /// <summary>
    /// 玩家管理器
    /// </summary>
    PlayerManager playerManager;
    /// <summary>
    /// 敌人管理器
    /// </summary>
    EnemyManager enemyManager;
    /// <summary>
    /// 子弹管理器
    /// </summary>
    BulletManager bulletManager;
    /// <summary>
    /// 宠物管理器
    /// </summary>
    PetManager petManager;

    //临时存放,to do:读取配置表
    [SerializeField] GameObject mapPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject bullet_1002Prefab;
    [SerializeField] GameObject bullet_1003Prefab;
    /*[SerializeField] */GameObject petPrefab;

    /// <summary>
    /// 检查当前连接的玩家数量是否满足开始游戏的条件
    /// </summary>
    private void CheckPlayerCount()
    {
        if (gameStarted) return;

        // 获取当前连接的客户端数量
        // ConnectedClientsIds 包含所有已连接客户端的ID（包括Host自己）
        int connectedCount = NetworkManager.Singleton.ConnectedClientsIds.Count;

        Debug.Log($"当前连接数: {connectedCount}");

        if (connectedCount >= requiredPlayers)
        {
            StartGame();
        }
    }

    /// <summary>
    /// 开始游戏，
    /// </summary>
    private void StartGame()
    {
        gameStarted = true;
        Debug.Log($"服务器: 已满足 {requiredPlayers} 名玩家，开始游戏！");

        // 在这里执行游戏开始逻辑
        // 例如：
        // 1. 生成游戏控制器
        // 2. 分配队伍
        // 3. 开始倒计时
        // 4. 通知所有客户端游戏开始

        // 示例：通知所有客户端
        StartGameClientRpc();

        waveManager.NextWave();

        // 可选：取消订阅，避免重复执行
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }

    #region 相应事件
    /// <summary>
    /// GameManager对象被创建完成了
    /// </summary>
    public override void OnNetworkSpawn()
    {
        
        if (IsClient)
        {
            Debug.Log("客户端设置事件监听spell");
            EventManager.AddListener<UIBottomSpellController.EventSpell>(OnUISpellClick);
        }



        if (IsServer)
        {
            //地图初始化
            mapManager = new MapManager(mapPrefab);
            //战线初始化
            battleLineManager = new BattleLineManager();
            //玩家管理器初始化
            playerManager = new PlayerManager(playerPrefab);
            playerManager.onActionCast += OnActionCast;
            //波次管理器初始化
            waveManager = new WaveManager(GetWaveConfigs());
            waveManager.onWaveChanged += WaveManager_onWaveChanged;
            //敌人管理器初始化
            enemyManager = new EnemyManager(GetEnemyPrefabs(), mapManager, this);
            enemyManager.onEnemyArrived += EnemyManager_onEnemyArrived;
            enemyManager.onEnemyHurt += EnemyManager_onEnemyHurt;
            enemyManager.onEnemyDead += EnemyManager_onEnemyDead;
            enemyManager.onAllEnemiesDead += EnemyManager_onAllEnemiesDead;
            //子弹管理器初始化
            bulletManager = new BulletManager(GetBulletPrefabs());
            bulletManager.onBulletHit += BulletManager_onHit;
            bulletManager.onBulletArrived += BulletManager_onArrived;

            //宠物管理器初始化
            petManager = new PetManager(GetPetPrefabs());

            //创建地图
            var mapObject = mapManager.Spawn(0);

            Debug.Log("服务器: 开始监听客户端连接...");
            // 订阅客户端连接事件
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

            // 检查是否已经满足条件（例如Host启动时已有1个连接）
            CheckPlayerCount();
        }
    }



    /// <summary>
    /// 有客户端连接了
    /// </summary>
    /// <param name="clientId"></param>
    private void OnClientConnected(ulong clientId)
    {
        if (!IsServer || gameStarted) return;

        //初始化battleLine
        battleLineManager.InitBattleLineWithClient(clientId);

        var lineIndex = battleLineManager.GetClientLineIndex(clientId);
        var seatPos = mapManager.GetSeatPosition(lineIndex);

        //创建角色
        playerManager.Spawn(clientId, GetPlayerData(clientId, lineIndex), seatPos, this);

        OnInitDataRPC(clientId);

        Debug.Log($"服务器: 客户端 {clientId} 已连接");
        CheckPlayerCount();
    }

    // 处理客户端断开，如果人数不足可能需要暂停游戏
    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            EventManager.RemoveListener<UIBottomSpellController.EventSpell>(OnUISpellClick);
        }


        if (IsServer)
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            }

            gameStarted = false;

            mapManager = null;
            battleLineManager = null;
            playerManager.onActionCast -= OnActionCast;
            playerManager = null;
            waveManager.onWaveChanged -= WaveManager_onWaveChanged;
            waveManager = null;
            enemyManager.onEnemyArrived -= EnemyManager_onEnemyArrived;
            enemyManager.onEnemyHurt -= EnemyManager_onEnemyHurt;
            enemyManager.onEnemyDead -= EnemyManager_onEnemyDead;
            enemyManager.onAllEnemiesDead -= EnemyManager_onAllEnemiesDead;
            enemyManager = null;
            bulletManager.onBulletHit -= BulletManager_onHit;
            bulletManager.onBulletArrived -= BulletManager_onArrived;
            bulletManager = null;
        }
        base.OnNetworkDespawn();
    }

    /// <summary>
    /// 客户端UI事件
    /// </summary>
    /// <param name="e"></param>
    private void OnUISpellClick(UIBottomSpellController.EventSpell e)
    {
        Debug.Log("准备调用RPC :OnUISpellClick");
        SpellServerRPC();
    }

    /// <summary>
    /// 当前波次所以怪物死光了
    /// </summary>
    private void EnemyManager_onAllEnemiesDead()
    {
        waveManager.NextWave();
    }

    /// <summary>
    /// 怪物死亡
    /// </summary>
    /// <param name="enemyObject"></param>
    private void EnemyManager_onEnemyDead(EnemyObject enemyObject)
    {
        enemyManager.RemoveEnemy(enemyObject);
    }

    /// <summary>
    /// 怪物收到伤害
    /// </summary>
    /// <param name="enemyObject"></param>
    /// <param name="damage"></param>
    private void EnemyManager_onEnemyHurt(EnemyObject enemyObject, int damage)
    {
        
        //通知客户端怪物受伤
        OnEnemyHurtClientRPC(enemyObject.GetNetworkObjectId(), damage);
    }

    /// <summary>
    /// 子弹命中敌人（发生碰撞）
    /// </summary>
    /// <param name="bulletObject"></param>
    /// <param name="enemyObject"></param>
    private void BulletManager_onHit(BulletObject bulletObject, EnemyObject enemyObject)
    {
        enemyObject.Hurt(bulletObject.GetData().damage);
        bulletManager.RemoveBullet(enemyObject.LineIndex, bulletObject);
    }

    /// <summary>
    /// 子弹飞到终点
    /// </summary>
    /// <param name="bulletObject"></param>
    private void BulletManager_onArrived(BulletObject bulletObject)
    {
        bulletManager.RemoveBullet(bulletObject.GetData().lineIndex, bulletObject);
    }

    /// <summary>
    /// 敌人走到终点(玩家处）
    /// </summary>
    /// <param name="enemy"></param>
    private void EnemyManager_onEnemyArrived(EnemyObject enemy)
    {
        //Debug.Log("EnemyManager_onEnemyArrived line:" + enemy.GetData().lineIndex);
        var lineIndex = enemy.GetData().lineIndex;
        var clientId = battleLineManager.GetClientIdByLineIndex(lineIndex);

        var playerObject = playerManager.GetPlayerObject(clientId);
        var enemyAtk = 10;
        playerObject.Hurt(enemyAtk);
    }

    /// <summary>
    /// 波次发生变化
    /// </summary>
    /// <param name="waveData"></param>
    private void WaveManager_onWaveChanged(WaveData waveData)
    {
        if (IsServer)
        {
            ClientAndHostChangeWaveRpc(waveData);

            var lineCount = battleLineManager.GetLineCount();
            for (ushort lineIndex = 0; lineIndex < lineCount; lineIndex++)
            {
                //每条线生成怪物
                enemyManager.SpawnEnemiesByInterval(lineIndex, GetEnemiesData(lineIndex, waveData.waveNumber));
            }
        }
    }

    /// <summary>
    /// 有技能被释放了
    /// </summary>
    /// <param name="launcher"></param>
    /// <param name="action"></param>
    /// <param name="targetPos"></param>
    private void OnActionCast(IUnit launcher, BaseAction action, Vector3 targetPos)
    {
        var bulletData = GetBulletData(launcher.LineIndex, action);
        bulletManager.SpawnBullet(launcher.LineIndex, bulletData, launcher.GetPosition(), targetPos);
    }
    #endregion

    #region 获取配置数据

    /// <summary>
    /// 获取玩家数据
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="lineIndex"></param>
    /// <returns></returns>
    private PlayerData GetPlayerData(ulong clientId, ushort lineIndex)
    {
        return new PlayerData()
        {
            hp = 100 + (int)clientId,
            maxHp = 100 + (int)clientId,
            playerName = $"Player{clientId}",
            lineIndex = lineIndex,
            skillDatas = GetPlayerSkillDatas(clientId),
            petDatas = GetPetDatas(clientId)
        };
    }

    /// <summary>
    /// 获取波次配置
    /// </summary>
    /// <returns></returns>
    private Dictionary<int, WaveData> GetWaveConfigs()
    {
        var result = new Dictionary<int, WaveData>();

        result.Add(1, new WaveData()
        {
            waveNumber = 1,
            waveDuration = 7,
        });
        result.Add(2, new WaveData()
        {
            waveNumber = 2,
            waveDuration = 20,
        });

        return result;
    }

    /// <summary>
    /// 获取怪物数据
    /// </summary>
    /// <param name="lineIndex"></param>
    /// <returns></returns>
    private List<EnemyData> GetEnemiesData(ushort lineIndex, ushort waveNumber)
    {
        var result = new List<EnemyData>();

        var e = new EnemyData()
        {
            lineIndex = lineIndex,
            enemyId = 0,
            uid = Guid.NewGuid().ToString(),
            speed = 0.5f,
            skillDatas = GetEnemySkillDatas(0),
            hp = 50,
            maxHp = 50
        };

        result.Add(e);
        result.Add(e);
        result.Add(e);
        result.Add(e);
        result.Add(e);
        result.Add(e);
        result.Add(e);
        result.Add(e);

        return result;
    }

    /// <summary>
    /// 获取玩家技能数据
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns></returns>
    List<ActionData> GetPlayerSkillDatas(ulong clientId)
    {
        var result = new List<ActionData>();
        result.Add(new ActionData()
        {
            actionId = 1, //玩家技能暂时用1
            skillLevel = 1,
            cd = UnityEngine.Random.Range(0.5f, 1f)
        });
        return result;
    }

    /// <summary>
    /// 获取怪物技能数据
    /// </summary>
    /// <param name="enemyId"></param>
    /// <returns></returns>
    List<ActionData> GetEnemySkillDatas(int enemyId)
    {
        var result = new List<ActionData>();
        result.Add(new ActionData()
        {
            actionId = 2, //怪物技能暂时用2
            skillLevel = 1,
            cd = 2f
        });
        return result;
    }

    /// <summary>
    /// 获取玩家数据
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns></returns>
    public List<PetData> GetPetDatas(ulong clientId)
    {
        var result = new List<PetData>();

        var pet = new PetData()
        {
            petId = 0,
            skillDatas = new List<ActionData>()
            //skillDatas = new List<ActionData>()
            //{
            //    new ActionData()
            //    {
            //        actionId = 4, //宠物技能暂时用4
            //        skillLevel = 1,
            //        cd = 1.5f
            //    }
            //}
        };
        result.Add(pet);

        return result;
    }

    /// <summary>
    /// 获取技能子弹数据
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    BulletData GetBulletData(int lineIndex, BaseAction action)
    {
        switch (action.Data.actionId)
        {
            case 1:
                return new BulletData()
                {
                    lineIndex = lineIndex,
                    id = 1002,
                    speed = 2f,
                    damage = 15, //根据action属性设置伤害
                };
            case 3:
                return new BulletData()
                {
                    lineIndex = lineIndex,
                    id = 1003,
                    speed = 1.5f,
                    damage = 25, //根据action属性设置伤害
                };
            default:
                throw new Exception($"未知的技能ID: {action.Data.actionId}");
        }
    }

    /// <summary>
    /// 获取怪物预制体列表
    /// </summary>
    /// <returns></returns>
    Dictionary<int, GameObject> GetEnemyPrefabs()
    {
        var result = new Dictionary<int, GameObject>();

        result.Add(0, enemyPrefab);

        return result;
    }

    /// <summary>
    /// 获取子弹预制体列表
    /// </summary>
    /// <returns></returns>
    Dictionary<int, GameObject> GetBulletPrefabs()
    {
        var result = new Dictionary<int, GameObject>();
        result.Add(1002, bullet_1002Prefab);
        result.Add(1003, bullet_1003Prefab);
        return result;
    }

    /// <summary>
    /// 获取神兽预制体列表
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private Dictionary<int, GameObject> GetPetPrefabs()
    {
        var result = new Dictionary<int, GameObject>();

        result.Add(0, petPrefab);

        return result;
    }
    #endregion


    #region 客户端RPC
    /// <summary>
    /// 通知客户端怪物受伤，可以播放受伤特效等
    /// </summary>
    /// <param name="enemyUid"></param>
    /// <param name="damage"></param>
    [Rpc(SendTo.ClientsAndHost)]
    public void OnEnemyHurtClientRPC(ulong enemyNetworkId, int damage)
    {
        Debug.Log("EnemyManager_onEnemyHurt :" + enemyNetworkId + " damage:" + damage);
        var e = new EventEnemyHurt();
        e.Body = (enemyNetworkId, damage);
        EventManager.Raise(e);
        Debug.Log($"OnEnemyHurtClientRPC {enemyNetworkId} EnemyHurt Received the RPC #{damage} on NetworkObject #");
    }

    /// <summary>
    /// 通知客户端游戏开始，可以播放开始动画等
    /// </summary>
    [ClientRpc]
    private void StartGameClientRpc()
    {
        Debug.Log($"客户端: 游戏开始！当前玩家数: {NetworkManager.Singleton.ConnectedClientsIds.Count}");
        // 客户端收到游戏开始通知后的处理
    }

    /// <summary>
    /// 通知客户端初始化数据
    /// </summary>
    [Rpc(SendTo.ClientsAndHost)]
    private void OnInitDataRPC(ulong clientId)
    {
        if (NetworkManager.Singleton.LocalClientId != clientId)
        {
            return;
        }
        Debug.Log("OnInitData : " + clientId);
        var e = new EventInit();
        e.Body = GetPetDatas(clientId);
        EventManager.Raise(e);
    }

    /// <summary>
    /// 通知客户端波次变化，可以刷新UI等
    /// </summary>
    /// <param name="data"></param>
    [Rpc(SendTo.ClientsAndHost)]
    private void ClientAndHostChangeWaveRpc(WaveData data)
    {
        Debug.Log($"ClientAndHostChangeWaveRpc{NetworkObjectId} Received the RPC #{data.waveNumber} on NetworkObject #");
        var e = new EventWaveChanged();
        e.Body = data;
        EventManager.Raise(e);
    }
    #endregion

    #region 服务器RPC
    [ServerRpc(RequireOwnership = false)]
    public void SpellServerRPC(ServerRpcParams rpcParams = default)
    {
        // 获取发送者的 ClientId
        ulong senderClientId = rpcParams.Receive.SenderClientId;
        var playerObject = playerManager.GetPlayerObject(senderClientId);
        if (playerObject != null)
        {
            Debug.Log($"服务器: 收到客户端 {senderClientId} 释放技能请求");
            playerObject.AddAction(new ActionData()
            {
                actionId = 3, //玩家技能暂时用3
                skillLevel = 1,
                cd = 3f
            });
        }
    }
    #endregion


    #region IFinder 接口实现
    /// <summary>
    /// 查找符合条件的目标单位
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public List<IUnit> FindTargets(Predicate<IUnit> predicate)
    {
        var result = new List<IUnit>();
        var allEnemies = enemyManager.GetAllEnemies();
        foreach (var enemy in allEnemies)
        {
            if (predicate(enemy))
            {
                result.Add(enemy);
            }
        }

        var allPlayer = playerManager.GetAllPlayerObjects();
        foreach (var player in allPlayer)
        {
            if (predicate(player))
            {
                result.Add(player);
            }
        }

        return result;
    }

    /// <summary>
    /// 查找指定战线的门位置
    /// </summary>
    /// <param name="lineIndex"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Vector3 FindDoor(int lineIndex)
    {
        return mapManager.GetDoorPosition(lineIndex) + new Vector3(0, 0.2f, 0);
    }
    #endregion
    /// <summary>
    /// 游戏开始后每帧调用
    /// </summary>
    public void Update()
    {
        if (gameStarted)
        {
            var deltaTime = Time.deltaTime;
            enemyManager.UpdateLogic(deltaTime);
            playerManager.UpdateLogic(deltaTime);
            bulletManager.UpdateLogic(deltaTime);
        }
    }


}



//public static List<GameObject> GetAllNetworkPrefabs()
//{
//    if (NetworkManager.Singleton == null)
//    {
//        Debug.LogError("NetworkManager is not initialized!");
//        return new List<GameObject>();
//    }

//    List<GameObject> prefabs = new List<GameObject>();

//    // 方法1：获取NetworkPrefabs列表（旧API）
//    if (NetworkManager.Singleton.NetworkConfig != null &&
//        NetworkManager.Singleton.NetworkConfig.Prefabs != null)
//    {
//        foreach (var prefab in NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs)
//        {
//            if (prefab != null && prefab.Prefab != null)
//            {
//                prefabs.Add(prefab.Prefab);
//            }
//        }
//    }

//    return prefabs;
//}