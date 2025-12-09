using System;
using UnityEngine;
using UnityEngine.Events;

public class LFOVector2Target : LFOTarget<Vector2>
{
    public override Vector2 Lerp(float t) => Vector2.Lerp(Min, Max, t);
}
