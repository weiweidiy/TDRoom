using DG.Tweening;
using JFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using TDRoom;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class UIWave : MonoBehaviour
{
    [SerializeField] private TMP_Text textWave;
    [SerializeField] private TMP_Text textWaveTime;
    [SerializeField] private GameObject objWaveBalloon;
    [SerializeField] private TMP_Text textBalloonWaveTime;

    [SerializeField] private GameObject objInfinity;

    private int _prevWave = -1;

    ITimer timer;

    private void Start()
    {
        var gameManager = GameObject.Find("GameManager(Clone)").GetComponent<GameManager>();
        gameManager.EventManager.AddListener<GameManager.EventWaveChanged>(OnWaveChanged);

        //textWave.CheckLocalizationFont();
        //textWaveTime.CheckLocalizationFont();
        //textBalloonWaveTime.CheckLocalizationFont();
        UpdateWaveUI(1);
        objWaveBalloon.SetActive(false);
        //QuantumCallback.Subscribe<CallbackUpdateView>(this, UpdateQuantum);
        //QuantumEvent.Subscribe<EventNoticeWaveInfo>(this, OnEventNoticeWaveInfo);
    }

    private void OnWaveChanged(GameManager.EventWaveChanged e)
    {
        var waveData = (WaveData)e.Body;
        UpdateWaveUI(waveData.waveNumber);
        //if (waveData.waveNumber == 1)
        //    PlayStartUi();
        var util = new DotweenUtils();
        var duration = waveData.waveDuration;
        timer = util.Regist(1f, (int)waveData.waveDuration, () =>
        {
            this.textWaveTime.text = $"{(int)(duration / 60):D2}:{(int)(duration % 60):D2}";
            if (duration <= 5 && !this.objWaveBalloon.activeSelf)
                ActiveBalloon();
            if (objWaveBalloon.activeSelf)
                textBalloonWaveTime.text = $"{(int)(duration % 60)}";
            if (duration <= 5)
                CheckBlink();
            else
                CheckBlinkOff();

            duration--;
        }, true);
    }

    //private void OnEventNoticeWaveInfo(EventNoticeWaveInfo callback)
    //{
    //    UpdateWaveUI(callback.Wave);

    //    if (callback.Wave == 1 && !GameScene.Instance.IsRunningTutorial)
    //        PlayStartUi();
    //}

    //private async void PlayStartUi()
    //{
    //    string prefabPath = string.Empty;
    //    string soundKey = "sfx_ingame_ui_animal";
    //    bool isCoopMode = false;

    //    switch ((GameModeId)GameScene.Instance.GameModeId)
    //    {
    //        case GameModeId.Normal:
    //            prefabPath = "UI/UI GameStart";
    //            break;
    //        case GameModeId.Coop:
    //            prefabPath = "UI/UI GameStart Coopmode";
    //            soundKey = "sfx_coop_ui_animals_fog";
    //            isCoopMode = true;
    //            break;
    //        case GameModeId.Hard:
    //            prefabPath = "UI/UI GameStart Hardmode";
    //            break;
    //        case GameModeId.Hell:
    //        case GameModeId.Nightmare:
    //        case GameModeId.PVP:
    //        case GameModeId.Guild_BOSS:
    //            prefabPath = "UI/UI GameStart Hellmode";
    //            break;
    //    }

    //    if (prefabPath.IsNullOrEmpty())
    //        return;

    //    var obj = await AssetManager.LoadAndInstantiateAssetAsync<GameObject>(prefabPath, CanvasView.CanvasInstance.transform);
    //    if (!isCoopMode)
    //    {
    //        SoundManager.Instance.PlayEffect(soundKey);
    //    }
    //    await UniTask.Delay(TimeSpan.FromSeconds(2f));
    //    if (isCoopMode)
    //    {
    //        SoundManager.Instance.PlayEffect(soundKey);
    //    }
    //    await UniTask.Delay(TimeSpan.FromSeconds(5f));

    //    Destroy(obj);
    //}

    private void UpdateWaveUI(int wave)
    {
        // if (QuantumRunner.DefaultGame?.Frames?.Verified == null)
        //     return;
        // var f = QuantumRunner.DefaultGame.Frames.Verified;
        //var gameMode = (int)PhotonStatics.Client.CurrentRoom.CustomProperties[PropertyId.GameMode];
        //this.waveTableData = Table.DataCenter.WaveData.GetWaveData(wave, gameMode.ToEnum<GameModeId>());
        //textWave.CheckLocalizationFont();
        this.textWave.text = "Wave:" + wave;
        if (objWaveBalloon) objWaveBalloon.SetActive(false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(textWave.rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(textWave.transform.parent as RectTransform);

        //switch ((GameModeId)GameScene.Instance.GameModeId)
        //{
        //    case GameModeId.Coop:
        //        bool isBossWave = (wave % 10 == 0);
        //        objInfinity.SetActive(isBossWave);
        //        textWaveTime.gameObject.SetActive(isBossWave == false);
        //        break;
        //    default:
        //        // objInfinity.SetActive();
        //        // textWaveTime.gameObject.SetActive(isBossWave == false);
        //        break;
        //}
    }

    private int countdown = 10;

    private void UpdateTime()
    {

    }
    //private unsafe void UpdateQuantum(CallbackUpdateView quantumView)
    //{
    //    var f = quantumView.Game.Frames.Verified;
    //    if (_prevWave != f.Global->WaveInfo.WaveIndex)
    //    {
    //        _prevWave = f.Global->WaveInfo.WaveIndex;
    //        UpdateWaveUI(f.Global->WaveInfo.WaveIndex);
    //    }

    //    if (f.Global->GameStatus.WaitingTimer > 0)
    //    {
    //        var startWaitingTimer = f.Global->GameStatus.WaitingTimer;
    //        var startWaitingTimerFloat = Mathf.Max(0, startWaitingTimer.AsFloat);
    //        this.textWaveTime.text = $"{(int)(startWaitingTimerFloat / 60):D2}:{(int)(startWaitingTimerFloat % 60):D2}";
    //        return;
    //    }

    //    var waveInfo = f.Global->WaveInfo;
    //    var waveLeftTime = Mathf.Max(0, (f.Global->WaveInfo.TableData.WaveTime - waveInfo.WaveTimer).AsFloat);
    //    this.textWaveTime.text = $"{(int)(waveLeftTime / 60):D2}:{(int)(waveLeftTime % 60):D2}";

    //    if (waveLeftTime < 3 && !this.objWaveBalloon.activeSelf)
    //        ActiveBalloon();
    //    if (objWaveBalloon.activeSelf)
    //        textBalloonWaveTime.text = $"{(int)(waveLeftTime % 60)}";
    //    if (waveLeftTime <= 5)
    //        CheckBlink();
    //    else
    //        CheckBlinkOff();

    //    var maxId = Table.DataCenter.WaveData.GetMaxWave(f.RuntimeConfig.gameModeId);
    //    if (f.Global->WaveInfo.WaveIndex == maxId && waveLeftTime - 1 <= countdown && countdown > 0)
    //    {
    //        ToastMessageController.Message($"{countdown}");
    //        countdown--;
    //    }
    //}

    private void ActiveBalloon()
    {
        objWaveBalloon.SetActive(false);
        objWaveBalloon.SetActive(true);
    }

    private bool _isBlink = false;

    private void CheckBlink()
    {
        if (_isBlink)
            return;
        _isBlink = true;
        //textWaveTime.DOColor(Color.red, 0.3f).SetDelay(0.2f).SetLoops(-1, LoopType.Yoyo);
        if (objWaveBalloon) objWaveBalloon.SetActive(true);
    }

    private void CheckBlinkOff()
    {
        if (_isBlink)
        {
            textWaveTime.DOKill();
            textWaveTime.color = Color.white;
            _isBlink = false;
            if (objWaveBalloon) objWaveBalloon.SetActive(false);
        }
    }
}


//public class UIWave : MonoBehaviour
//{
//    [SerializeField] TextMeshProUGUI txtWave;

//    ITimer timer;
//    // Start is called before the first frame update
//    void Start()
//    {
//        var gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
//        gameManager.EventManager.AddListener<GameManager.EventWaveChanged>(OnInit);
//    }

//    private void OnInit(GameManager.EventWaveChanged e)
//    {

//        timer?.Stop();
//        var waveData = (WaveData)e.Body;

//        var util = new DotweenUtils();
//        var duration = waveData.waveDuration;
//        Debug.Log("OnInit " + waveData.waveNumber + " duration:" + duration);

//        timer = util.Regist(1f, (int)waveData.waveDuration, () =>
//        {

//            var content = "Wave:" + (waveData.waveNumber).ToString() + "\n" + duration;
//            txtWave.text = content;

//            duration--;
//        }, true);
//    }

//}

