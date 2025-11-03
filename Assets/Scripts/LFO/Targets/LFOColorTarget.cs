using System;
using UnityEngine;
using UnityEngine.Events;

public class LFOColorTarget : LFOTarget<Color>
{
    [SerializeField]
    private Color min = Color.black;

    [SerializeField]
    private Color max = Color.white;

    public override Color Lerp(float t) => Color.Lerp(min, max, t);
}
