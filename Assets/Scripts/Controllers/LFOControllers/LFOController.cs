using UnityEngine;

/// <summary>
/// The waveform type to use for an LFOController
/// </summary>
public enum LFOWaveformType
{
    Sine,
    Triangle,
    Square,
    Linear,
    Custom,
}

/// <summary>
/// A controller that uses an LFO for calculations.
/// </summary>
public abstract class LFOController : CycleInBeatsController
{
    private AnimationCurve SineCurve
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

    private AnimationCurve TriangleCurve =>
        AnimationCurveUtils.Join(
            AnimationCurve.Linear(0, 0, 0.5f, 1),
            // `timeStart` cannot equal `0.5f + 1e-10f`
            // because 1e-10f is small enough such that
            // Unity believes 0.5f equals `0.5f + 1e-10f`
            // leading to unsuccesful curve joining
            AnimationCurve.Linear(0.5f + 1e-5f, 1, 1, 0)
        );

    private AnimationCurve SquareCurve =>
        AnimationCurveUtils.Join(
            AnimationCurve.Constant(0, 0.5f, 1),
            // `timeStart` cannot equal `0.5f + 1e-10f`
            // because 1e-10f is small enough such that
            // Unity believes 0.5f equals `0.5f + 1e-10f`
            // leading to unsuccesful curve joining
            AnimationCurve.Constant(0.5f + 1e-5f, 1, 0)
        );

    private AnimationCurve LinearCurve => AnimationCurve.Linear(0, 0, 1, 1);

    // ============================================================================================
    // ============================================================================================

    /// <summary>
    /// The waveform to use for calculations.
    /// Should not be changed.
    /// </summary>
    [Header("LFO")]
    [SerializeField]
    private LFOWaveformType waveform = LFOWaveformType.Sine;

    /// <summary>
    /// The custom curve to use for calculations when `waveform` is set to `Custom`.
    /// Should not be changed.
    /// </summary>
    [SerializeField, ShowIfEqual("waveform", LFOWaveformType.Custom)]
    private AnimationCurve customCurve = AnimationCurve.Linear(0, 0, 1, 1);

    /// <summary>
    /// The curve to use for calculations, set according to `waveform`.
    /// Should not be changed.
    /// </summary>
    [SerializeField]
    private AnimationCurve curve;

    /// <summary>
    /// The number of radians at which the lfo should start if the period of the lfo is 2pi.
    /// </summary>
    [SerializeField]
    [Range(0, 2 * Mathf.PI)]
    private float phaseOffset = 0;

    private void Awake()
    {
        customCurve = AnimationCurveUtils.Normalize(customCurve);
        curve = waveform switch
        {
            LFOWaveformType.Sine => SineCurve,
            LFOWaveformType.Triangle => TriangleCurve,
            LFOWaveformType.Square => SquareCurve,
            LFOWaveformType.Linear => LinearCurve,
            LFOWaveformType.Custom => customCurve,
            _ => throw new System.Exception(),
        };
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

    /// <summary>
    /// Whether or not this controller should loop.
    /// If false, for all time after `SecondsPerCycle` seconds, this controller will supply the terminal value of the LFO when invoking the action.
    /// </summary>
    [SerializeField]
    protected bool loop = true;
}

// Generic version for internal use (not serialized by Unity)
// If a symbol in this class does not depend on `T`,
// it should be refactored and moved to LFOController so it can be serialized by unity.
public abstract class LFOController<T> : LFOController
{
    /// <summary>
    /// The min value for this `LFOController` to lerp between.
    /// </summary>
    [SerializeField]
    protected T min;

    /// <summary>
    /// The max value for this `LFOController` to lerp between.
    /// </summary>
    [SerializeField]
    protected T max;

    protected abstract T LerpValue(T min, T max, float t);
    protected abstract void InvokeAction(T value);

    private void Update()
    {
        var lfoValue = Evaluate(loop ? TimeSeconds : Mathf.Min(TimeSeconds, SecondsPerCycle));
        T mappedValue = LerpValue(min, max, lfoValue);
        InvokeAction(mappedValue);
    }
}
