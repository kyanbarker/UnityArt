using System;
using UnityEngine;
using UnityEngine.Events;

public class LFOVector3Target : LFOTarget<Vector3>
{
    public override Vector3 Lerp(float t) => Vector3.Lerp(Min, Max, t);
}
