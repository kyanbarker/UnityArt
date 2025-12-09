using System;
using UnityEngine;
using UnityEngine.Events;

public class LFOColorTarget : LFOTarget<Color>
{
    public override Color Lerp(float t) => Color.Lerp(Min, Max, t);
}
