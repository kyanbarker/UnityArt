using UnityEngine;

/// <summary>
/// A controller that uses bpm for calculations.
/// </summary>
public class BPMController : MonoBehaviour
{
    [Header("BPM / BPM Time")]

    [SerializeField]
    private bool useExternalBPMTime = true;
    public bool UseExternalBPMTime
    {
        get => useExternalBPMTime;
        set => useExternalBPMTime = value;
    }

    /// <summary>
    /// The external `BPMTime` to use when `useExternalBPMTime` is true.
    /// If no value is supplied, defaults to `GetComponentInParent<BPMTime>()`
    /// </summary>
    [SerializeField]
    [ShowIfEqual("useExternalBPMTime", true)]
    private BPMTime externalBPMTime;
    public BPMTime ExternalBPMTime
    {
        get => externalBPMTime;
        set => externalBPMTime = value;
    }

    [SerializeField]
    [Min(1e-10f)]
    [ShowIfEqual("useExternalBPMTime", false)]
    private float bpm = 120;
    public float BPM
    {
        get
        {
            if (UseExternalBPMTime)
            {
                if (ExternalBPMTime == null)
                {
                    ExternalBPMTime = GetComponentInParent<BPMTime>();
                }
                if (ExternalBPMTime != null)
                {
                    return ExternalBPMTime.BPM;
                }
            }
            return bpm;
        }
        set => bpm = Mathf.Max(Mathf.Epsilon, value);
    }

    /// <summary>
    /// We say that an `BPMController` uses global time if `TimeSeconds` is simply `realtimeSinceStartup`.
    ///
    /// We say that an `BPMController` uses local time if `TimeSeconds` is instead
    /// `realtimeSinceStartup - startTimeSeconds` (i.e., the number of seconds this controller has been active).
    /// </summary>
    [SerializeField]
    private bool useGlobalTime = false;
    public bool UseGlobalTime { get => useGlobalTime; set => useGlobalTime = value; }

    /// <summary>
    /// The value of `Time.realtimeSinceStartup` recorded at the time this controller's `Start` method ran
    /// </summary>
    private float startTimeSeconds;
    public float StartTimeSeconds { get => startTimeSeconds; set => startTimeSeconds = value; }

    /// <summary>
    /// `Time.realtimeSinceStartup` if `useGlobalTime` is true, else `startTimeSeconds` less.
    /// </summary>
    public float TimeSeconds => Time.realtimeSinceStartup - (UseGlobalTime ? 0 : StartTimeSeconds);

    public float BeatsPerSecond => BPM / 60;

    public float TimeBeats => TimeSeconds * BeatsPerSecond;

    void Start()
    {
        StartTimeSeconds = Time.realtimeSinceStartup;
    }
}
