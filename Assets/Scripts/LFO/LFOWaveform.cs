using System;
using UnityEngine;

public class LFOWaveform : MonoBehaviour
{
    // Technically a cosine curve with a period of 1, but close enough
    public static AnimationCurve SinCurve =>
        new(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));

    // Like the sine curve but linear rising and falling
    public static AnimationCurve TriangleCurve =>
        AnimationCurveUtils.Join(
            AnimationCurve.Linear(0f, 0f, 0.5f, 1f),
            AnimationCurve.Linear(0.5f, 1f, 1f, 0f)
        );

    public static AnimationCurve SquareCurve =>
        AnimationCurveUtils.Join(
            AnimationCurve.Constant(0f, 0.5f, 1f),
            AnimationCurve.Constant(0.5f, 1f, 0f)
        );

    public static AnimationCurve LinearCurve => AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [SerializeField]
    private AnimationCurve curve = SinCurve;

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
        curve = AnimationCurveUtils.Normalize(curve);
        Util.Assert(IsWellFormed);
    }

    public bool IsWellFormed
    {
        get
        {
            try
            {
                float minValue = AnimationCurveUtils.GetMinValueOfCurve(curve);
                float maxValue = AnimationCurveUtils.GetMaxValueOfCurve(curve);
                float minTime = AnimationCurveUtils.GetMinTimeOfCurve(curve);
                float maxTime = AnimationCurveUtils.GetMaxTimeOfCurve(curve);
                Util.RequireEquals(minTime, 0f, "minTime");
                Util.RequireEquals(maxTime, 1f, "maxTime");
                Util.RequireBetweenZeroToOne(minValue, "minValue");
                Util.RequireBetweenZeroToOne(maxValue, "maxValue");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
