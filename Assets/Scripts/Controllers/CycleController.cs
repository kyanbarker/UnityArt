using UnityEngine;

public class CycleController : BPMController
{
    [SerializeField]
    private float beatsPerCycle = 1;

    public float BeatsPerCycle
    {
        get => beatsPerCycle;
        set => beatsPerCycle = Mathf.Max(Mathf.Epsilon, value);
    }

    public float BeatsPerSecond { get => BPM / 60; }

    public float SecondsPerCycle { get => BeatsPerCycle / BeatsPerSecond; }

    private void OnValidate()
    {
        beatsPerCycle = Mathf.Max(Mathf.Epsilon, beatsPerCycle);
    }
}