using UnityEngine;

/// <summary>
/// A controller that uses bpm for calculations.
/// </summary>
public class BPMController : TimeControllerMonoBehaviour
{
    [Header("BPM Controller")]
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
            if (!UseExternalBPMTime)
            {
                return bpm;
            }
            if (ExternalBPMTime == null)
            {
                ExternalBPMTime = GetComponentInParent<BPMTime>();
            }
            return ExternalBPMTime.BPM;
        }
        set => bpm = Mathf.Max(Mathf.Epsilon, value);
    }

    public float BeatsPerSecond => BPM / 60;

    public float TimeBeats => TimeSeconds * BeatsPerSecond;
}
