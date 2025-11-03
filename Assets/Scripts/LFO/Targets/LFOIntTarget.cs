using System;
using UnityEngine;
using UnityEngine.Events;

public class LFOIntTarget : LFOTarget<int>
{
    [SerializeField]
    private int min = 0;

    [SerializeField]
    private int max = 1;

    public override int Lerp(float t) => Mathf.RoundToInt(Mathf.Lerp(min, max, t));
}
