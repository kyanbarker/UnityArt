using UnityEngine;

/// <summary>
/// A controller that uses bpm and cyclical beats for calculations
/// </summary>
public class CycleInBeatsController : BPMController, ICycleController
{
    [SerializeField]
    private float beatsPerCycle = 1;
    public float BeatsPerCycle
    {
        get => beatsPerCycle;
        set => beatsPerCycle = Mathf.Max(Mathf.Epsilon, value);
    }

    public float SecondsPerCycle => BeatsPerCycle / BeatsPerSecond;

    private void OnValidate()
    {
        beatsPerCycle = Mathf.Max(Mathf.Epsilon, beatsPerCycle);
    }
}
