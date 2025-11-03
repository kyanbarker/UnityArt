using System;
using UnityEngine;
using UnityEngine.Events;

public class LFOVector3Target : LFOTarget<Vector3>
{
    [SerializeField]
    private Vector3 min = Vector3.zero;

    [SerializeField]
    private Vector3 max = Vector3.one;

    public override Vector3 Lerp(float t) => Vector3.Lerp(min, max, t);
}
