
using Cysharp.Threading.Tasks;
using Game.Common;
using JFramework;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace TDRoom
{
    public class TDGameObjectPool : BaseGameObjectPool
    {

        protected IAssetsLoader _assetsLoader;

        public TDGameObjectPool(IAssetsLoader assetsLoader)
        {
            if (assetsLoader == null)
                throw new Exception(this.GetType().ToString() + " Inject IAssetsLoader failed!");
            _assetsLoader = assetsLoader;
        }

        protected override IAssetsLoader GetAssetLoader()
        {
            return _assetsLoader;
        }

    }
}
