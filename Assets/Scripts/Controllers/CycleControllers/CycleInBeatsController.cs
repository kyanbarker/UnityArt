using UnityEngine;

/// <summary>
/// A controller that uses cycles measured in beats for calculations
/// </summary>
public class CycleInBeatsController : BPMController, ICycleController
{
    [Header("Beats Cycle")]
    [SerializeField]
    [Min(1e-10f)]
    private float beatsPerCycle = 1;
    public float BeatsPerCycle
    {
        get => beatsPerCycle;
        set => beatsPerCycle = Mathf.Max(1e-10f, value);
    }

    public float SecondsPerCycle => BeatsPerCycle / BeatsPerSecond;
}
