
using Game;
using System;

public class ActionFactory
{
    public BaseAction Create(ActionData actionData, IFinder finder, IUnit self)
    {
        BaseAction action = null;
        switch (actionData.actionId)
        {
            case 1: //玩家默认技能ID对应的动作
                {
                    action = new FlyAttackAction(actionData, finder, self);
                    break;
                }
            case 2:
                {//怪物技能ID对应的动作
                    action = new NormalAttackAction(actionData, finder, self);
                    break;
                }
            case 3:
                {
                    action = new PassthroughAttackAction(actionData, finder, self);
                    break;
                }
            default:
                throw new Exception($"没有实现对应的 action id: {actionData.actionId}");
        }
        return action;
    }
}
