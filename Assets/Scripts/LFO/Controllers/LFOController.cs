using UnityEngine;

public abstract class LFOController : LFO
{
    [SerializeField]
    protected bool useExternalLFO = false;

    [SerializeField]
    [BoolConditionalHide("useExternalLFO")]
    protected LFO externalLFO;

    [SerializeField]
    protected bool useGlobalTime = false;

    [SerializeField]
    protected bool loop = true;

    protected float startTimeSeconds;

    protected float TimeSeconds
    {
        get => Time.realtimeSinceStartup - (
            useGlobalTime
                ? 0
                : startTimeSeconds
        );
    }

    protected LFO LFO
    {
        get
        {
            return useExternalLFO && externalLFO != null ? externalLFO : this;
        }
    }

    protected virtual void Start()
    {
        startTimeSeconds = Time.realtimeSinceStartup;
    }

    protected virtual void Update()
    {
        ApplyLFOValue(
            (!loop && TimeSeconds >= SecondsPerCycle)
                ? LFO.Evaluate(SecondsPerCycle)
                : LFO.Evaluate(TimeSeconds)
        );
    }

    protected float GetFinalLFOValue(float timeSeconds)
    {
        // For non-looping behavior, get the LFO value at exactly one complete cycle
        if (useExternalLFO)
        {
            if (externalLFO == null) return 0f;
            return externalLFO.Evaluate(SecondsPerCycle);
        }
        else
        {
            return Evaluate(SecondsPerCycle);
        }
    }

    protected abstract void ApplyLFOValue(float lfoValue);
}

// Generic version for internal use (not serialized by Unity)
public abstract class LFOController<T> : LFOController
{
    [SerializeField]
    protected T min;

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