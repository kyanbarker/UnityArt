using UnityEngine;

public class BPMController : MonoBehaviour
{
    [SerializeField]
    private bool useExternalBPMTime = true;
    public bool UseExternalBPMTime { get => useExternalBPMTime; set => useExternalBPMTime = value; }

    [SerializeField]
    [BoolConditionalHide("useExternalBPMTime")]
    private BPMTime externalBPMTime;
    public BPMTime ExternalBPMTime { get => externalBPMTime; set => externalBPMTime = value; }

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
