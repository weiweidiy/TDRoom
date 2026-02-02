using Game;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 穿透伤害
/// </summary>
public class PassthroughAttackAction : ActiveAction
{
    public PassthroughAttackAction(ActionData data, IFinder finder, IUnit self) : base(data, finder, self)
    {

    }
    protected override (bool, Vector3) DoExecute(List<IUnit> targets)
    {
        if (targets.Count == 0)
            return (false, Vector3.zero);

        var targetPos = targets[0].GetPosition();

        //Debug.Log("释放一个飞行道具");
        return (true, targetPos);

        //var targetPos = finder.FindDoor(self.LineIndex);
        //Debug.Log("释放一个穿透道具");
        //return (true, targetPos);
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
