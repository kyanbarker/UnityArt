using System;
using UnityEngine;
using UnityEngine.Events;

public class LFOFloatTarget : LFOTarget<float>
{
    [SerializeField]
    private float min = 0;

    public float Min
    {
        get => min;
        set => min = value;
    }

    [SerializeField]
    private float max = 1;

    public float Max
    {
        get => max;
        set => max = value;
    }

    public override float Lerp(float t) => Mathf.Lerp(min, max, t);
}
