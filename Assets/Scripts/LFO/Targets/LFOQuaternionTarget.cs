using System;
using UnityEngine;
using UnityEngine.Events;

public class LFOQuaternionTarget : LFOTarget<Quaternion>
{
    [SerializeField]
    private Quaternion min = Quaternion.identity;

    [SerializeField]
    private Quaternion max = Quaternion.identity;

    public override Quaternion Lerp(float t) => Quaternion.Slerp(min, max, t);
}
