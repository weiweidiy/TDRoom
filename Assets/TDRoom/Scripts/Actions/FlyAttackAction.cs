using Game;
using System.Collections.Generic;
using UnityEngine;

public class FlyAttackAction : ActiveAction
{
    public FlyAttackAction(ActionData data, IFinder finder, IUnit self) : base(data, finder, self)
    {
    }

    protected override (bool, Vector3) DoExecute(List<IUnit> targets)
    {
        if (targets.Count == 0)
            return (false, Vector3.zero);

        var targetPos = targets[0].GetPosition();

        //Debug.Log("释放一个飞行道具");
        return (true, targetPos);
    }

    protected override bool OnFinder(IUnit target)
    {
        if (target.UnitType == self.UnitType)
            return false;

        if (target == self)
            return false;

        if (target.LineIndex != self.LineIndex)
            return false;

        return true;
    }
}
