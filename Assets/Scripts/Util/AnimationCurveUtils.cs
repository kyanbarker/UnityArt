using UnityEngine;

public static class AnimationCurveUtils
{
    /// <summary>
    /// Returns a new animation curve that appends the keys of `a` then the keys of `b`
    /// where `a` and `b` are animation curves and `b` begins where `a` ends.
    /// </summary>
    public static AnimationCurve Join(AnimationCurve a, AnimationCurve b)
    {
        RequireNonNullNonEmptyCurve(a);
        RequireNonNullNonEmptyCurve(b);

        // Ensure B begins where A ends (plus a tiny epsilon so they don't replace one another)
        // Note that if epsilon is too small, Unity will replace one of the keys because they are too close together
        float aLastTime = a.keys[^1].time;
        float bFirstTime = b.keys[0].time;
        const float epsilon = 1e-5f;
        float shift = Mathf.Max(0f, aLastTime - bFirstTime + epsilon);

        int totalLength = a.keys.Length + b.keys.Length;
        Keyframe[] combinedKeys = new Keyframe[totalLength];

        // Copy A's keys
        for (int i = 0; i < a.keys.Length; i++)
            combinedKeys[i] = a.keys[i];

        // Copy B's keys, shifted so B starts after A
        for (int i = 0; i < b.keys.Length; i++)
        {
            Keyframe k = b.keys[i];
            k.time += shift;
            combinedKeys[a.keys.Length + i] = k;
        }

        return new AnimationCurve(combinedKeys);
    }

    /// <summary>
    /// A normalized curve of `curve` is the curve identical to `curve` up to scale and position
    /// with a domain and range of [0,1]
    /// </summary>
    public static AnimationCurve Normalize(AnimationCurve curve)
    {
        RequireNonNullNonEmptyCurve(curve);
        Util.RequireDifferent(GetTimeRangeOfCurve(curve), 0f);

        float minTime = GetMinTimeOfCurve(curve);
        float timeRange = GetTimeRangeOfCurve(curve);
        float minValue = GetMinValueOfCurve(curve);
        float valueRange = GetValueRangeOfCurve(curve);
        Keyframe[] normalizedKeys = new Keyframe[curve.length];
        for (int i = 0; i < curve.length; i++)
        {
            Keyframe k = curve.keys[i];
            float normalizedTime = (k.time - minTime) / timeRange;
            float normalizedValue = (k.value - minValue) / valueRange;
            normalizedKeys[i] = new Keyframe(normalizedTime, normalizedValue);
        }
        return new AnimationCurve(normalizedKeys);
    }

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
        float minValue = float.MaxValue;
        foreach (var key in curve.keys)
        {
            minValue = Mathf.Min(minValue, key.value);
        }
        return minValue;
    }

    public static float GetMaxValueOfCurve(AnimationCurve curve)
    {
        RequireNonNullNonEmptyCurve(curve);
        float maxValue = float.MinValue;
        foreach (var key in curve.keys)
        {
            maxValue = Mathf.Max(maxValue, key.value);
        }
        return maxValue;
    }

    public static float GetValueRangeOfCurve(AnimationCurve curve)
    {
        RequireNonNullNonEmptyCurve(curve);
        float minValue = float.MaxValue;
        float maxValue = float.MinValue;
        foreach (var key in curve.keys)
        {
            minValue = Mathf.Min(minValue, key.value);
            maxValue = Mathf.Max(maxValue, key.value);
        }
        return maxValue - minValue;
    }
}
