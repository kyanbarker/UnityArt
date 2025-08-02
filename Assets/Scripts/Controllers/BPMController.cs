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

    void OnValidate()
    {
        bpm = Mathf.Max(Mathf.Epsilon, bpm);
    }
}
