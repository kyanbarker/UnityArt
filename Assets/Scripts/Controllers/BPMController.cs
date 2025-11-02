using UnityEngine;

/// <summary>
/// A controller that uses bpm for calculations.
/// </summary>
public class BpmController : TimeController
{
    [Space(10)]
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
    // [ShowIfEqual("useExternalBPMTime", true)]
    private BpmTimer externalBpmTimer;
    public BpmTimer ExternalBpmTimer
    {
        get => externalBpmTimer;
        set => externalBpmTimer = value;
    }

    [SerializeField]
    [Min(1e-10f)]
    // [ShowIfEqual("useExternalBPMTime", false)]
    private float bpm = 120;
    public float Bpm
    {
        get
        {
            if (!UseExternalBPMTime)
            {
                return bpm;
            }
            if (ExternalBpmTimer == null)
            {
                ExternalBpmTimer = GetComponentInParent<BpmTimer>();
            }
            return ExternalBpmTimer.BPM;
        }
        set => bpm = Mathf.Max(1e-10f, value);
    }

    public float BeatsPerSecond => Bpm / 60;

    public float TimeBeats => TimeSeconds * BeatsPerSecond;
}
