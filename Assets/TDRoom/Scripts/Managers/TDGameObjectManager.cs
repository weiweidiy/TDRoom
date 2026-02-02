using Cysharp.Threading.Tasks;
using JFramework;
using JFramework.Game;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Tiktok
{
    public class TDGameObjectManager
    {
        /// <summary>
        /// 游戏对象对象池
        /// </summary>
        TDGameObjectPool goPool;

        //GameLevelBackgroundViewController bgController;

        GameObject pools;

        public GameObject GoRoot { get; set; }

        public TDGameObjectManager(TDGameObjectPool goPool)
        {
            if (goPool == null)
                throw new Exception(this.GetType().ToString() + " Inject TiktokGameObjectPool failed!");

            this.goPool = goPool;
        }

        public async UniTask Initialize()
        {
            pools = new GameObject("Pools");

            //var prefabsList = jConfigManager.GetAll<PrefabsCfgData>();

            var prefabsList = new List<string>();

            var tasks = new List<UniTask>();
            foreach (var prefabs in prefabsList)
            {
                var location = prefabs;
                if (goPool.HasRegist(location))
                    continue;

                var taskLevels = goPool.Regist(location, pools.transform, 10, OnCreate, OnRelease, OnRent, OnReturn);
                tasks.Add(taskLevels);
            }

            await UniTask.WhenAll(tasks);
        }

        private void OnReturn(GameObject go)
        {
            go.SetActive(false);
            go.transform.SetParent(pools.transform);
        }

        private void OnRent(GameObject go)
        {
            go.SetActive(true);
        }

        /// <summary>
        /// 新的实例创建了
        /// </summary>
        /// <param name="go"></param>
        private void OnCreate(GameObject go)
        {
        }

        /// <summary>
        /// 实例是被释放掉了
        /// </summary>
        /// <param name="object"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnRelease(GameObject go)
        {
            GameObject.Destroy(go);
        }

        public GameObject Rent(string name)
        {
            return goPool.Rent(name);
        }

        public void Return(GameObject go)
        {
            goPool.Return(go);
        }

    }
}
