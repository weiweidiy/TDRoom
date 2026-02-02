//using Sirenix.OdinInspector;

using DG.Tweening;
using System;

namespace JFramework
{
    public class DotweenUtils : ITimerUtils

    {
        //Timeline m_TimeLine;

        //private void Update()
        //{
        //    //Debug.Assert(m_TimeLine != null, "没有找到timeline " + gameObject);
        //    float deltaTime = m_TimeLine ? m_TimeLine.deltaTime : Time.deltaTime;
        //    deltaTime = deltaTime * Time.timeScale;
        //    DOTween.ManualUpdate(deltaTime, m_TimeLine.unscaledTime);
        //}

        //public Tween DOTweenDelay(float delayedTimer, int loopTimes, Action action, bool immediatly = false, UpdateType updateType = UpdateType.Normal)
        //{
        //    if (immediatly)
        //        action?.Invoke();

        //    float timer = 0;
        //    //DOTwwen.To()中参数：前两个参数是固定写法，第三个是到达的最终值，第四个是渐变过程所用的时间
        //    Tween t = DOTween.To(() => timer, x => timer = x, 1, delayedTimer).SetUpdate(updateType)
        //        .OnStepComplete(() => { action?.Invoke(); })
        //        .SetLoops(loopTimes);

        //    return t;
        //}

        public ITimer Regist(float interval, int loopTimes, Action action, bool immediatly = false, bool useRealTime = false)
        {
            if (immediatly)
                action?.Invoke();

            float timer = 0;
            //DOTwwen.To()中参数：前两个参数是固定写法，第三个是到达的最终值，第四个是渐变过程所用的时间
            Tween t = DOTween.To(() => timer, x => timer = x, 1, interval).SetUpdate(UpdateType.Normal)
                .OnStepComplete(() => { action?.Invoke(); })
                .SetLoops(loopTimes);

            return new DotweenTimer(t);
        }

        public void Update()
        {
            //throw new NotImplementedException();
        }
    }
}
