using System;
using UnityEngine;
using UnityEngine.Events;

public class LFOVector2Target : LFOTarget<Vector2>
{
    [SerializeField]
    private Vector2 min = Vector2.zero;

    public Vector2 Min
    {
        get => min;
        set => min = value;
    }

    [SerializeField]
    private Vector2 max = Vector2.one;

    public Vector2 Max
    {
        get => max;
        set => max = value;
    }

    public override Vector2 Lerp(float t) => Vector2.Lerp(min, max, t);
}
