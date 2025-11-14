using System;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

public static class AnimationCurveUtils
{
    public static void RequireNonNullNonEmptyCurve(AnimationCurve curve)
    {
        Util.RequireNonNull(curve);
        Util.Assert(curve.length >= 2);
    }

    public static float GetMinTimeOfCurve(AnimationCurve curve)
    {
        RequireNonNullNonEmptyCurve(curve);
        return curve.keys[0].time;
    }

    public static float GetMaxTimeOfCurve(AnimationCurve curve)
    {
        RequireNonNullNonEmptyCurve(curve);
        return curve.keys[^1].time;
    }

    public static float GetTimeRangeOfCurve(AnimationCurve curve)
    {
        return GetMaxTimeOfCurve(curve) - GetMinTimeOfCurve(curve);
    }

    public static float GetMinValueOfCurve(AnimationCurve curve)
    {
        RequireNonNullNonEmptyCurve(curve);
        return curve.keys.Min(key => key.value);
    }

    public static float GetMaxValueOfCurve(AnimationCurve curve)
    {
        RequireNonNullNonEmptyCurve(curve);
        return curve.keys.Max(key => key.value);
    }

    public static float GetValueRangeOfCurve(AnimationCurve curve)
    {
        RequireNonNullNonEmptyCurve(curve);
        return GetMaxValueOfCurve(curve) - GetMinValueOfCurve(curve);
    }
}
