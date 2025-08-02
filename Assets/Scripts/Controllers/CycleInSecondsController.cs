using UnityEngine;

public class CycleInSecondsController : MonoBehaviour, ICycleController
{
    [SerializeField]
    [Min(1e-10f)]
    private float secondsPerCycle = 1;
    public float SecondsPerCycle
    {
        get => secondsPerCycle;
        set => secondsPerCycle = value;
    }
}
