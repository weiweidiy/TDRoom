using JFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestory : MonoBehaviour
{
    [SerializeField] float delay = 1f;

    ITimer timer;
    void Start()
    {
        timer?.Stop();
        var util = new DotweenUtils();
        timer = util.Regist(delay, 1, () =>
        {
            Destroy(gameObject);
        });
    }

    private void OnDestroy()
    {
        timer?.Stop();
    }
}
