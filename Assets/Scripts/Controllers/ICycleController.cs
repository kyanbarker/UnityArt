/// <summary>
/// A controller that uses cycles for calculations
/// </summary>
public interface ICycleController : ITimeController
{
    public float SecondsPerCycle { get; }
    public float TimeCycles => TimeSeconds / SecondsPerCycle;
}
