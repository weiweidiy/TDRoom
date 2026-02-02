using JFramework;
using System;
using UnityEngine;
using UnityEngine.UI;
using Event = JFramework.Event;
public class UIBottomSpellController : MonoBehaviour
{
    public class EventSpell : Event { }

    [SerializeField] private Button btnSpell;



    EventManager eventManager;

    private void Start()
    {
        var gameManager = GameObject.Find("GameManager(Clone)").GetComponent<GameManager>();
        eventManager = gameManager.EventManager;
    }

    private void OnEnable()
    {
        btnSpell.onClick.AddListener(OnClickSpell);
    }

    private void OnDisable()
    {
        btnSpell.onClick.RemoveAllListeners();
    }


    private void OnClickSpell()
    {
        Debug.Log("OnClickSpell");
        var e = new EventSpell();
        eventManager.Raise<EventSpell>(e);
    }


}

