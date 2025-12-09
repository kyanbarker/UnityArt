using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

// We use a non generic base class so that LFOController can have a list of LFOTargets
// and each target can have its own type parameter T
// e.g. One LFOController can drive both LFOFloatTarget and LFOColorTarget
public abstract class LFOTarget : MonoBehaviour
{
    public abstract void Invoke(float t);
}

public abstract class LFOTarget<T> : LFOTarget
{
    public UnityAction<T> Action { get; set; }
    public T Min { get; set; }
    public T Max { get; set; }
    public abstract T Lerp(float t);

    public override void Invoke(float t)
    {
        Assert.IsTrue(0f <= t && t <= 1f, "t must be between 0 and 1");
        T value = Lerp(t);
        Action.Invoke(value);
    }
}
