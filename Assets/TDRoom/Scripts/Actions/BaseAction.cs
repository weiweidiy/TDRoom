using Game;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction
{
    public event Action<IUnit, BaseAction, Vector3> onCast;

    protected IFinder finder;
    protected IUnit self;

    public ActionData Data { get; protected set; }

    public BaseAction(ActionData data, IFinder finder, IUnit self)
    {
        this.finder = finder;
        this.self = self;
        this.Data = data;
    }

    public void Execute(List<IUnit> targets)
    {
        var result = DoExecute(targets);
        var (success, targetPos) = result;
        if (success)
            onCast?.Invoke(self, this, targetPos);
    }

    protected abstract (bool, Vector3) DoExecute(List<IUnit> targets);

    public List<IUnit> FindTargets()
    {
        var result = finder.FindTargets(OnFinder);
        return result;
    }

    protected abstract bool OnFinder(IUnit obj);

    public virtual void Update(float deltaTime)
    {
    }
}
