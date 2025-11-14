using System;
using UnityEngine;

public class LFOWaveform : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve curve;

    public AnimationCurve Curve => curve;

    public float Evaluate(float time)
    {
        Util.RequireBetweenZeroToOne(time, "time");
        float value = curve.Evaluate(time);
        Util.RequireBetweenZeroToOne(value, "value");
        return value;
    }

    private void Start()
    {
        float minValue = AnimationCurveUtils.GetMinValueOfCurve(curve);
        float maxValue = AnimationCurveUtils.GetMaxValueOfCurve(curve);
        float minTime = AnimationCurveUtils.GetMinTimeOfCurve(curve);
        float maxTime = AnimationCurveUtils.GetMaxTimeOfCurve(curve);
        Util.RequireEquals(minTime, 0f, "minTime");
        Util.RequireEquals(maxTime, 1f, "maxTime");
        Util.RequireBetweenZeroToOne(minValue, "minValue");
        Util.RequireBetweenZeroToOne(maxValue, "maxValue");
    }
}
