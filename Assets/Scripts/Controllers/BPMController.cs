using UnityEngine;

/// <summary>
/// A controller that uses bpm for calculations.
/// </summary>
public class BPMController : MonoBehaviour
{
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
    [BoolConditionalHide("useExternalBPMTime")]
    private BPMTime externalBPMTime;
    public BPMTime ExternalBPMTime
    {
        get => externalBPMTime;
        set => externalBPMTime = value;
    }

    [SerializeField]
    [BoolConditionalHide("useExternalBPMTime", true, true)]
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

    /// <summary>
    /// The value of `Time.realtimeSinceStartup` recorded at the time this controller's `Start` method ran
    /// </summary>
    private float startTimeSeconds;

    /// <summary>
    /// `Time.realtimeSinceStartup` if `useGlobalTime` is true, else `startTimeSeconds` less.
    /// </summary>
    public float TimeSeconds => Time.realtimeSinceStartup - (useGlobalTime ? 0 : startTimeSeconds);

    public float BeatsPerSecond => BPM / 60;

    public float TimeBeats => TimeSeconds * BeatsPerSecond;

    void OnValidate()
    {
        bpm = Mathf.Max(Mathf.Epsilon, bpm);
    }

    void Start()
    {
        startTimeSeconds = Time.realtimeSinceStartup;
    }
}
