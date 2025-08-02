using UnityEngine;

public static class AnimationCurveUtils
{
    /// <summary>
    /// Returns a new animation curve that appends the keys of `a` then the keys of `b`
    /// where `a` and `b` are animation curves and `b` begins after `a` ends.
    /// </summary>
    public static AnimationCurve Join(AnimationCurve a, AnimationCurve b)
    {
        // Optional: prevent duplicate keyframe if B starts where A ends
        bool shouldSkipFirstBKey =
            b.keys.Length > 0
            && a.keys.Length > 0
            && Mathf.Approximately(a.keys[^1].time, b.keys[0].time);

        int totalLength = a.keys.Length + (shouldSkipFirstBKey ? b.keys.Length - 1 : b.keys.Length);
        Keyframe[] combinedKeys = new Keyframe[totalLength];

        // Copy A's keys
        for (int i = 0; i < a.keys.Length; i++)
        {
            combinedKeys[i] = a.keys[i];
        }

        // Copy B's keys, skipping the first if needed
        for (int i = shouldSkipFirstBKey ? 1 : 0; i < b.keys.Length; i++)
        {
            combinedKeys[a.keys.Length + i - (shouldSkipFirstBKey ? 1 : 0)] = b.keys[i];
        }

        return new AnimationCurve(combinedKeys);
    }
}
