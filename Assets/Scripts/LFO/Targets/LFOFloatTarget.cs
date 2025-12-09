using System;
using UnityEngine;
using UnityEngine.Events;

public class LFOFloatTarget : LFOTarget<float>
{
    public override float Lerp(float t) => Mathf.Lerp(Min, Max, t);
}
