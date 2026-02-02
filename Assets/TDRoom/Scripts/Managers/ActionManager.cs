
using Game;
using System;
using System.Collections.Generic;
using UnityEngine;
public class ActionManager
{
    public event Action<IUnit, BaseAction, Vector3> onActionCast;

    List<BaseAction> activeActions = new List<BaseAction>();

    ActionFactory factory = new ActionFactory();

    List<BaseAction> actionNeedToAdd = new List<BaseAction>();
    List<BaseAction> actionNeedToRemove = new List<BaseAction>();

    IFinder finder;
    IUnit self;

    public void Init(List<ActionData> skillDatas, IFinder finder, IUnit self)
    {
        this.finder = finder;
        this.self = self;
        foreach (var data in skillDatas)
        {
            BaseAction action = factory.Create(data, finder, self);
            action.onCast += Skill_onCast;
            activeActions.Add(action);
        }
    }

    public void Clear()
    {
        foreach (var action in activeActions)
        {
            action.onCast -= Skill_onCast;
        }
        activeActions.Clear();

        actionNeedToAdd.Clear();
        actionNeedToRemove.Clear();
        finder = null;
        self = null;
    }

    public void AddAction(ActionData data)
    {
        BaseAction action = factory.Create(data, finder, self);
        actionNeedToAdd.Add(action);
    }

    public void RemoveAction(ActionData data)
    {
        BaseAction action = factory.Create(data, finder, self);
        actionNeedToRemove.Remove(action);
    }


    public void Update(float deltaTime)
    {
        foreach (var action in actionNeedToAdd)
        {
            action.onCast += Skill_onCast;
            activeActions.Add(action);
        }
        actionNeedToAdd.Clear();

        foreach (var action in actionNeedToRemove)
        {
            action.onCast -= Skill_onCast;
            activeActions.Remove(action);
        }
        actionNeedToRemove.Clear();

        foreach (var action in activeActions)
        {
            action.Update(deltaTime);
        }

        //更新删除或者添加action的逻辑？
    }


    private void Skill_onCast(IUnit launcher, BaseAction action, Vector3 targetPos)
    {
        //onActionCast?.Invoke
        onActionCast?.Invoke(launcher, action, targetPos);
    }
}
