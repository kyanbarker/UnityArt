using System;
using UnityEngine;
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
    public abstract T Lerp(float t);

    public override void Invoke(float t)
    {
        Util.RequireBetweenZeroToOne(t, "t");
        T value = Lerp(t);
        action.Invoke(value);
    }
}
