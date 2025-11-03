using System;
using UnityEngine;
using UnityEngine.Events;

public class LFOFloatTarget : LFOTarget<float>
{
    [SerializeField]
    private float min = 0;

    [SerializeField]
    private float max = 1;

    public override float Lerp(float t) => Mathf.Lerp(min, max, t);
}
