using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JFramework
{
    public enum SceneMode
    {
        Single,
        Additive
    }
    public interface IAssetsLoader
    {
        UniTask<Scene> LoadSceneAsync(string sceneName, SceneMode mode);

        UniTask<GameObject> InstantiateAsync(string location);

        UniTask<GameObject> InstantiateAsync(string location, Transform parent);

        UniTask<T> LoadAssetAsync<T>(string location) where T : Object ;
    }
}

