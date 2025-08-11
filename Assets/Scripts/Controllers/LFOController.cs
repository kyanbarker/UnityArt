using UnityEngine;

/// <summary>
/// A controller that uses an `LFO` for calculations.
/// </summary>
public abstract class LFOController : LFO
{
    [Header("LFO Controller")]
    [SerializeField]
    protected bool useExternalLFO = false;

    /// <summary>
    /// The `LFO` to use when `useExternalLFO` is true.
    /// Defaults to this `LFO` if no value is supplied.
    /// </summary>
    [SerializeField]
    [ShowIfEqual("useExternalLFO", true)]
    protected LFO externalLFO;

    /// <summary>
    /// Whether or not this controller should loop.
    /// If false, for all time after `SecondsPerCycle` seconds, this controller will supply the terminal value of the LFO when invoking the action.
    /// </summary>
    [SerializeField]
    protected bool loop = true;

    /// <summary>
    /// The LFO to use to calculate the argument for this controllers action.
    /// </summary>
    protected LFO LFO => useExternalLFO && externalLFO != null ? externalLFO : this;

    protected virtual void Update()
    {
        ApplyLFOValue(
            loop ? LFO.Evaluate(TimeSeconds) : LFO.Evaluate(Mathf.Min(TimeSeconds, SecondsPerCycle))
        );
    }

    /// <summary>
    /// Apply the lfo value to this controller's action.
    /// </summary>
    protected abstract void ApplyLFOValue(float lfoValue);
}

// Generic version for internal use (not serialized by Unity)
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

    protected override void ApplyLFOValue(float lfoValue)
    {
        T mappedValue = LerpValue(min, max, lfoValue);
        InvokeAction(mappedValue);
    }

    protected abstract T LerpValue(T min, T max, float t);
    protected abstract void InvokeAction(T value);
}
