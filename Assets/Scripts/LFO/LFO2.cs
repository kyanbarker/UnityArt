using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LFO2 : CycleInBeatsController
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
        set => customCurve = AnimationCurveUtils.Normalize(value);
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
        customCurve = AnimationCurveUtils.Normalize(customCurve);
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
