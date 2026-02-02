using Game;
using JFramework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBottomPetController : MonoBehaviour
{


    [SerializeField] private Button btnOpenOrClosePet;
    [SerializeField] private GameObject goPetOpen;
    [SerializeField] private GameObject goPetClose;


    [SerializeField] private GameObject goPopup;
    [SerializeField] private UIPetScrollerController uiPetScrollerCtrl;
    EventManager eventManager;

    private void OnDestroy()
    {
        eventManager.RemoveListener<GameManager.EventInit>(OnInit);
    }

    private void OnInit(GameManager.EventInit e)
    {

    }

    private void Start()
    {
        var gameManager = GameObject.Find("GameManager(Clone)").GetComponent<GameManager>();
        eventManager = gameManager.EventManager;
        eventManager.AddListener<GameManager.EventInit>(OnInit);

        var allPlayers = FindObjectsOfType<PlayerObject>();
        Debug.Log("ui监听所有玩家数量:" + allPlayers.Length);
        foreach (PlayerObject player in allPlayers)
        {
            var data = player.data;
            Debug.Log("ui监听所有玩家数据:" + data.Value.playerName);
        }
    }

    private void OnEnable()
    {
        btnOpenOrClosePet.onClick.AddListener(OnClickOpenOrClosePet);

    }

    private void OnDisable()
    {
        btnOpenOrClosePet.onClick.RemoveAllListeners();

    }




    private void OnClickOpenOrClosePet()
    {
        if (goPetOpen.activeSelf)
            OpenPet();
        else
            ClosePet();
    }

    void OpenPet()
    {
        goPopup.SetActive(true);
        uiPetScrollerCtrl.gameObject.SetActive(true);
        goPetOpen.SetActive(false);
        goPetClose.SetActive(true);
    }

    void ClosePet()
    {
        goPopup.SetActive(false);
        uiPetScrollerCtrl.gameObject.SetActive(false);
        goPetOpen.SetActive(true);
        goPetClose.SetActive(false);
    }
}

