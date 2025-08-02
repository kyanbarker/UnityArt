using UnityEngine;

public static class AnimationCurveUtils
{
    public static AnimationCurve Join(AnimationCurve a, AnimationCurve b)
    {
        // Get keyframes
        var aKeys = a.keys;
        var bKeys = b.keys;

        // Optional: prevent duplicate keyframe if B starts where A ends
        bool shouldSkipFirstBKey = bKeys.Length > 0 && aKeys.Length > 0 &&
                                   Mathf.Approximately(aKeys[^1].time, bKeys[0].time);

        int totalLength = aKeys.Length + (shouldSkipFirstBKey ? bKeys.Length - 1 : bKeys.Length);
        Keyframe[] combinedKeys = new Keyframe[totalLength];

        // Copy A's keys
        for (int i = 0; i < aKeys.Length; i++)
            combinedKeys[i] = aKeys[i];

        // Copy B's keys, skipping the first if needed
        for (int i = (shouldSkipFirstBKey ? 1 : 0); i < bKeys.Length; i++)
        {
            combinedKeys[aKeys.Length + i - (shouldSkipFirstBKey ? 1 : 0)] = bKeys[i];
        }

        return new AnimationCurve(combinedKeys);
    }
}
