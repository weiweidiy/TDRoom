using Game;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NormalAttackAction : ActiveAction
{
    public NormalAttackAction(ActionData data, IFinder finder, IUnit self) : base(data, finder, self)
    {
    }

    protected override (bool, Vector3) DoExecute(List<IUnit> targets)
    {
        //Debug.Log($"NormalAttackAction Execute by {self.UnitType}, targets count: {targets.Count} ");
        Vector3 targetPos = Vector3.zero;
        var result = false;
        foreach (var unit in targets)
        {
            unit.Hurt(2);
            targetPos = unit.GetPosition();
            result = true;
        }

        return (result, targetPos);
    }

    protected override bool OnFinder(IUnit target)
    {
        if (target.UnitType == self.UnitType)
            return false;

        if (target == self)
            return false;

        //º∆À„self∫Õtargetµƒæ‡¿Î
        var selfPos = self.GetPosition();
        var targetPos = target.GetPosition();

        float distance = Vector3.Distance(selfPos, targetPos);

        return distance < 0.1f;
    }
}
