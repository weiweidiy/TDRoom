//using Sirenix.OdinInspector;

using DG.Tweening;
using JFramework;

namespace TDRoom
{
    public class DotweenTimer : ITimer
    {
        Tween tween;
        public DotweenTimer(Tween tween)
        {
            this.tween = tween;
        }
        public void Stop()
        {
            this.tween.Kill();
        }
    }
}
