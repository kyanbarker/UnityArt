using System;
using UnityEngine;
using UnityEngine.Events;

public class LFOVector3Target : LFOTarget<Vector3>
{
    [SerializeField]
    private Vector3 min = Vector3.zero;

    public override Vector3 Min
    {
        get => min;
        set => min = value;
    }

    [SerializeField]
    private Vector3 max = Vector3.one;

    public override Vector3 Max
    {
        get => max;
        set => max = value;
    }

    public override Vector3 Lerp(float t) => Vector3.Lerp(min, max, t);
}
