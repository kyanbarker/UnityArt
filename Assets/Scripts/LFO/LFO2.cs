using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LFO2 : CycleController
{
    [SerializeField]
    protected LFOWaveformType waveform = LFOWaveformType.Sine;

    /// <summary>
    /// The custom curve to use for calculations when `waveform` is set to `Custom`.
    /// </summary>
    [SerializeField]
    protected AnimationCurve customCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve CustomCurve
    {
        get => customCurve;
        set => customCurve = NormalizeCurve(value);
    }

    /// <summary>
    /// The curve to use for calculations, set according to `waveform`.
    /// </summary>
    [SerializeField]
    //[ViewAnimationCurve]
    protected AnimationCurve curve;

    [SerializeField]
    [Range(0, 2 * Mathf.PI)]
    protected float phaseOffset = 0;

    protected void Awake()
    {
        customCurve = NormalizeCurve(customCurve);
        curve = new Dictionary<LFOWaveformType, AnimationCurve>
        {
            [LFOWaveformType.Sine] = SineCurve,
            [LFOWaveformType.Triangle] = TriangleCurve,
            [LFOWaveformType.Square] = SquareCurve,
            [LFOWaveformType.Linear] = LinearCurve,
            [LFOWaveformType.Custom] = customCurve,
        }[waveform];
    }

    protected AnimationCurve SineCurve
    {
        get
        {
            var curve = new AnimationCurve();
            int samples = 500;
            for (int i = 0; i <= samples; i++)
            {
                float t = (float)i / samples;
                float value = (Mathf.Sin(t * 2 * Mathf.PI) + 1) / 2;
                curve.AddKey(t, value);
            }
            return curve;
        }
    }

    protected AnimationCurve TriangleCurve =>
        AnimationCurveUtils.Join(
            AnimationCurve.Linear(0, 0, 0.5f, 1),
            AnimationCurve.Linear(0.5f + 1e-5f, 1, 1, 0)
        );

    protected AnimationCurve SquareCurve =>
        AnimationCurveUtils.Join(
            AnimationCurve.Constant(0, 0.5f, 1),
            AnimationCurve.Constant(0.5f + 1e-5f, 1, 0)
        );

    protected AnimationCurve LinearCurve => AnimationCurve.Linear(0, 0, 1, 1);

    /// <summary>
    /// A normalized curve of `input` is the curve identical to `input` up to scale and position
    /// with a domain and range of [0,1]
    /// </summary>
    protected AnimationCurve NormalizeCurve(AnimationCurve input)
    {
        if (input == null || input.length == 0)
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
            float val = input.Evaluate(t);
            minVal = Mathf.Min(minVal, val);
            maxVal = Mathf.Max(maxVal, val);
        }

        // Rebuild curve with normalized values
        foreach (var key in input.keys)
        {
            float normalizedTime = Mathf.Clamp01(key.time);
            float normalizedValue = maxVal > minVal ? (key.value - minVal) / (maxVal - minVal) : 0;
            normalized.AddKey(normalizedTime, normalizedValue);
        }

        return normalized;
    }

    /// <summary>
    /// Evaluates the value of this LFO at the supplied time in seconds.
    /// </summary>
    public float Evaluate(float timeSeconds)
    {
        return curve.Evaluate(GetCycleProgress(timeSeconds));
    }

    /// <summary>
    /// The proportion of the cycle that has been completed at the supplied time in seconds.
    /// </summary>
    public float GetCycleProgress(float timeSeconds)
    {
        float cycleProgress = (timeSeconds / SecondsPerCycle + phaseOffset / (2 * Mathf.PI)) % 1;

        if (cycleProgress < 0)
            cycleProgress += 1;

        return cycleProgress;
    }
}
