using System;
using UnityEngine;
using UnityEngine.Events;

public class LFOQuaternionTarget : LFOTarget<Quaternion>
{
    public override Quaternion Lerp(float t) => Quaternion.Slerp(Min, Max, t);
}
