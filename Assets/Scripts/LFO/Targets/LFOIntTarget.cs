using System;
using UnityEngine;
using UnityEngine.Events;

public class LFOIntTarget : LFOTarget<int>
{
    public override int Lerp(float t) => Mathf.RoundToInt(Mathf.Lerp(Min, Max, t));
}
