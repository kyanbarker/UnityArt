using System;
using UnityEngine;
using UnityEngine.Events;

public class LFOColorTarget : LFOTarget<Color>
{
    [SerializeField]
    private Color min = Color.black;

    public override Color Min
    {
        get => min;
        set => min = value;
    }

    [SerializeField]
    private Color max = Color.white;

    public override Color Max
    {
        get => max;
        set => max = value;
    }

    public override Color Lerp(float t) => Color.Lerp(min, max, t);
}
