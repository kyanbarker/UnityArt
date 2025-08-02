using UnityEngine;

public interface ICycleController
{
    public float SecondsPerCycle { get; }
    public float NumElapsedCycles => Time.realtimeSinceStartup / SecondsPerCycle;
}
