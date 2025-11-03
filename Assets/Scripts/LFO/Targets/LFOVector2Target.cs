using System;
using UnityEngine;
using UnityEngine.Events;

public class LFOVector2Target : LFOTarget<Vector2>
{
    [SerializeField]
    private Vector2 min = Vector2.zero;

    [SerializeField]
    private Vector2 max = Vector2.one;

    public override Vector2 Lerp(float t) => Vector2.Lerp(min, max, t);
}
