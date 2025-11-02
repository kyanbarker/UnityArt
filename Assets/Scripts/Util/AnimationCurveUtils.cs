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

    /// <summary>
    /// A normalized curve of `input` is the curve identical to `input` up to scale and position
    /// with a domain and range of [0,1]
    /// </summary>
    public static AnimationCurve Normalize(AnimationCurve curve)
    {
        if (curve == null || curve.length == 0)
            return AnimationCurve.Linear(0, 0, 1, 1);

        // Ensure domain [0,1] and normalize range to [0,1]
        var normalized = new AnimationCurve();

        // Sample the curve and find actual min/max
        float minVal = float.MaxValue,
            maxVal = float.MinValue;
        int samples = 500;

        for (int i = 0; i <= samples; i++)
        {
            float t = Mathf.Clamp01((float)i / samples);
            float val = curve.Evaluate(t);
            minVal = Mathf.Min(minVal, val);
            maxVal = Mathf.Max(maxVal, val);
        }

        // Rebuild curve with normalized values
        foreach (var key in curve.keys)
        {
            float normalizedTime = Mathf.Clamp01(key.time);
            float normalizedValue = maxVal > minVal ? (key.value - minVal) / (maxVal - minVal) : 0;
            normalized.AddKey(normalizedTime, normalizedValue);
        }

        return normalized;
    }
}
