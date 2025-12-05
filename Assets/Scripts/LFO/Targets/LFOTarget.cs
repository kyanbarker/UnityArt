using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

// Base class for LFO targets
// Since UnityEvents do not support generics, we create a non-generic base class
// and a generic derived class for specific types.
public abstract class LFOTarget : MonoBehaviour
{
    public abstract void Invoke(float t);
}

public abstract class LFOTarget<T> : LFOTarget
{
    public UnityEvent<T> action;
    public abstract T Min { get; set; }
    public abstract T Max { get; set; }
    public abstract T Lerp(float t);

    public override void Invoke(float t)
    {
        Assert.IsTrue(0f <= t && t <= 1f, "t must be between 0 and 1");
        T value = Lerp(t);
        action.Invoke(value);
    }
}
