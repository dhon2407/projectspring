using System;
using System.Collections.Generic;
using MEC;

namespace Utilities.Helpers
{
    public static class CallTiming
    {
        public static void DelayInvoke(this Action action, float delay, bool timeScaleDependent = true)
        {
            Timing.RunCoroutine(DelayCallInvoke(delay), timeScaleDependent ? Segment.Update : Segment.SlowUpdate);

            IEnumerator<float> DelayCallInvoke(float d)
            {
                yield return Timing.WaitForSeconds(d);
                action?.Invoke();
            }
        }
        
        public static void DelayInvoke(this Action action, float delay, string tag, bool timeScaleDependent = true)
        {
            Timing.RunCoroutine(DelayCallInvoke(delay), timeScaleDependent ? Segment.Update : Segment.SlowUpdate, tag);

            IEnumerator<float> DelayCallInvoke(float d)
            {
                yield return Timing.WaitForSeconds(d);
                action?.Invoke();
            }
        }
    }
}