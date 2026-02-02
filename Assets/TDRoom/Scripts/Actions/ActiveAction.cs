
using Game;


/// <summary>
/// 主动触发，CD结束可触发
/// </summary>
public abstract class ActiveAction : BaseAction
{
    private float cooldown = 2f; // 冷却时间（秒）
    private float timer = 0f;



    protected ActiveAction(ActionData data, IFinder finder, IUnit self) : base(data, finder, self)
    {
        cooldown = data.cd;


    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        timer -= deltaTime;
        if (timer <= 0f)
        {
            var lastTargets = FindTargets();
            Execute(lastTargets);
            timer = cooldown;
        }
    }
}
