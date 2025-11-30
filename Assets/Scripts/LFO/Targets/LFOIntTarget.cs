using System;
using UnityEngine;
using UnityEngine.Events;

public class LFOIntTarget : LFOTarget<int>
{
    [SerializeField]
    private int min = 0;

    public int Min
    {
        get => min;
        set => min = value;
    }

    [SerializeField]
    private int max = 1;

    public int Max
    {
        get => max;
        set => max = value;
    }

    public override int Lerp(float t) => Mathf.RoundToInt(Mathf.Lerp(min, max, t));
}
