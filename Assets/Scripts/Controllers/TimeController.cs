using UnityEngine;

/// <summary>
/// A MonoBehaviour that uses global or local time for calculations
/// </summary>
public class TimeController : MonoBehaviour, ITimeController
{
    [SerializeField]
    private bool useGlobalTime = false;
    public bool UseGlobalTime
    {
        get => useGlobalTime;
        set => useGlobalTime = value;
    }

    public float StartTimeSeconds { get; set; }

    // Expose the interface's default implementation to extending classes
    public float TimeSeconds => Time.realtimeSinceStartup - (UseGlobalTime ? 0 : StartTimeSeconds);

    private void Start()
    {
        StartTimeSeconds = Time.realtimeSinceStartup;
    }
}
