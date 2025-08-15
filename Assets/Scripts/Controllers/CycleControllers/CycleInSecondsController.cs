using UnityEngine;

/// <summary>
/// A controller that uses cycles measured in seconds for calculations
/// </summary>
public class CycleInSecondsController : TimeController, ICycleController
{
    [Header("Cycle In Seconds Controller")]
    [SerializeField]
    [Min(1e-10f)]
    private float secondsPerCycle = 1;
    public float SecondsPerCycle
    {
        get => secondsPerCycle;
        set => secondsPerCycle = value;
    }
}
