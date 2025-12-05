using System;
using UnityEngine;
using UnityEngine.Events;

public class LFOQuaternionTarget : LFOTarget<Quaternion>
{
    [SerializeField]
    private Quaternion min = Quaternion.identity;

    public override Quaternion Min
    {
        get => min;
        set => min = value;
    }

    [SerializeField]
    private Quaternion max = Quaternion.identity;

    public override Quaternion Max
    {
        get => max;
        set => max = value;
    }

    public override Quaternion Lerp(float t) => Quaternion.Slerp(min, max, t);
}
