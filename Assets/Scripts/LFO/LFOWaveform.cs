using UnityEngine;
using UnityEngine.Assertions;

public class LFOWaveform : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve curve;

    public AnimationCurve Curve => curve;

    public float Evaluate(float time)
    {
        Assert.IsTrue(0 <= time && time <= 1, "time must be in [0, 1].");
        float value = curve.Evaluate(time);
        Assert.IsTrue(0 <= value && value <= 1, "value must be in [0, 1].");
        return value;
    }

    private void Start()
    {
        float minValue = curve.keys[0].value;
        float maxValue = curve.keys[^1].value;
        float minTime = curve.keys[0].time;
        float maxTime = curve.keys[^1].time;
        Assert.AreEqual(minTime, 0f, "minTime != 0f");
        Assert.AreEqual(maxTime, 1f, "maxTime != 1f");
        Assert.IsTrue(0 <= minValue && minValue <= 1, "minValue must be in [0, 1].");
        Assert.IsTrue(0 <= maxValue && maxValue <= 1, "maxValue must be in [0, 1].");
    }
}
