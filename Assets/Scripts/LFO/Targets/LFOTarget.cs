using System;
using UnityEngine;
using UnityEngine.Events;

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
