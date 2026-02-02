using JFramework;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WaveManager
{
    public event Action<WaveData> onWaveChanged;
    public event Action onAllWavesCompleted;

    Dictionary<int, WaveData> wavesConfig;

    int curWave = 0;

    ITimer timer;

    public WaveManager(Dictionary<int, WaveData> wavesConfig)
    {
        this.wavesConfig = wavesConfig;
    }

    public void NextWave()
    {
        timer?.Stop();
        curWave++;
        if (!wavesConfig.ContainsKey(curWave))
        {
            onAllWavesCompleted?.Invoke();
            return;
        }


        var waveData = wavesConfig[curWave];
        Debug.Log($"WaveManager NextWave: {curWave} Duration: {waveData.waveDuration}");

        var util = new DotweenUtils();
        var duration = waveData.waveDuration;
        timer = util.Regist(1f, (int)waveData.waveDuration, () =>
        {
            duration--;
            if (duration <= 0)
                NextWave();
        }, true);

        onWaveChanged?.Invoke(waveData);
    }

}
