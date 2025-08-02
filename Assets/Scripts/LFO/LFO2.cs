using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class LFO2 : BPMController
{
    [SerializeField]
    protected LFOWaveformType waveform = LFOWaveformType.Sine;

    [SerializeField]
    protected AnimationCurve customCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [SerializeField]
    //[ViewAnimationCurve]
    protected AnimationCurve curve;

    [SerializeField]
    [Range(0, 2 * Mathf.PI)]
    protected float phaseOffset = 0;

    protected Dictionary<LFOWaveformType, AnimationCurve> waveformCurves;

    public LFOWaveformType Waveform { get; set; }

    public AnimationCurve CustomCurve
    {
        get => customCurve;
        set => customCurve = NormalizeCurve(value);
    }

    protected void Awake()
    {
        customCurve = NormalizeCurve(customCurve);
        curve = new Dictionary<LFOWaveformType, AnimationCurve>
        {
            [LFOWaveformType.Sine] = CreateSineCurve(),
            [LFOWaveformType.Triangle] = CreateTriangleCurve(),
            [LFOWaveformType.Square] = CreateSquareCurve(),
            [LFOWaveformType.Linear] = CreateLinearCurve(),
            [LFOWaveformType.Custom] = customCurve
        }[waveform];
    }

    protected AnimationCurve CreateSineCurve()
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

    protected AnimationCurve NormalizeCurve(AnimationCurve input)
    {
        if (input == null || input.length == 0) return AnimationCurve.Linear(0, 0, 1, 1);

        // Ensure domain [0,1] and normalize range to [0,1]
        var normalized = new AnimationCurve();

        // Sample the curve and find actual min/max
        float minVal = float.MaxValue, maxVal = float.MinValue;
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
            float normalizedValue = maxVal > minVal ?
                (key.value - minVal) / (maxVal - minVal) : 0;
            normalized.AddKey(normalizedTime, normalizedValue);
        }

        return normalized;
    }

    protected AnimationCurve CreateTriangleCurve()
    {
        return AnimationCurveUtils.Join(
            AnimationCurve.Linear(0, 0, 0.5f, 1),
            AnimationCurve.Linear(0.5f + 1e-5f, 1, 1, 0)
        );
    }

    protected AnimationCurve CreateSquareCurve()
    {
        return AnimationCurveUtils.Join(
            AnimationCurve.Constant(0, 0.5f, 1),
           AnimationCurve.Constant(0.5f + 1e-5f, 1, 0)
        );
    }

    protected AnimationCurve CreateLinearCurve()
    {
        return AnimationCurve.Linear(0, 0, 1, 1);
    }

    public float Evaluate(float timeSeconds)
    {
        try
        {
            float cycleProgress = GetCycleProgress(timeSeconds);
            return curve.Evaluate(cycleProgress);
        }
        catch (NullReferenceException e)
        {
            Debug.Assert(curve != null);
            Debug.Assert(waveformCurves != null);
            //Debug.Assert(waveform != null);
            Debug.Assert(waveformCurves[waveform] != null, $"waveformCurves={waveformCurves};waveform={waveform}");
            throw e;
        }
    }

    public float GetCycleProgress(float timeSeconds)
    {
        float cycleProgress = (timeSeconds / SecondsPerCycle + phaseOffset / (2 * Mathf.PI)) % 1;

        if (cycleProgress < 0) cycleProgress += 1;

        return cycleProgress;
    }
}