using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class UIDamageHudController : MonoBehaviour
{
    [SerializeField] GameObject damageHud;
    void Start()
    {
        Debug.Log("UIDamageHudManager Start");
        var gameManager = GameObject.Find("GameManager(Clone)").GetComponent<GameManager>();
        gameManager.EventManager.AddListener<GameManager.EventEnemyHurt>(OnEnemyHurt);

    }

    //private void OnServerStarted()
    //{
    //    var gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    //    gameManager.EventManager.AddListener<GameManager.EventEnemyHurt>(OnEnemyHurt);
    //}

    private void OnEnemyHurt(GameManager.EventEnemyHurt e)
    {
        Debug.Log("OnEnemyHurt");

        var tuple = (ValueTuple<ulong, int>)e.Body;
        //var uid = tuple.Item1;
        var dmg = tuple.Item2;

        //Debug.Log($"UIDamageHudManager OnEnemyHurt uid:{uid} dmg:{dmg}");
        ulong networkObjectId = tuple.Item1; // 你要查找的对象的ID
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(
            networkObjectId, out NetworkObject networkObject))
        {
            // 成功获取到 NetworkObject
            var obj = Resources.Load<GameObject>("UIDamageHud");
            var go = Instantiate(obj, networkObject.transform.position, Quaternion.identity);
            go.GetComponent<UIDamage>().SetDamage(dmg);
        }
    }
}
