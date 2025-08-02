using UnityEngine;

public class BPMController : MonoBehaviour
{
    [SerializeField]
    private bool useExternalBPM = true;

    public bool UseExternalBPM { get; set; }

    [SerializeField]
    [BoolConditionalHide("useExternalBPM")]
    private BPMTime externalBPM;

    [SerializeField]
    [BoolConditionalHide("useExternalBPM", true, true)]
    private float bpm = 120;
    public float BPM
    {
        get
        {
            if (useExternalBPM)
            {
                if (externalBPM == null)
                {
                    externalBPM = GetComponentInParent<BPMTime>();
                }
                if (externalBPM != null)
                {
                    return externalBPM.BPM;
                }
            }
            return bpm;
        }
        set => bpm = Mathf.Max(Mathf.Epsilon, value);
    }

    [SerializeField]
    private float beatsPerCycle = 1;

    public float BeatsPerCycle
    {
        get => beatsPerCycle;
        set => beatsPerCycle = Mathf.Max(Mathf.Epsilon, value);
    }

    public float BeatsPerSecond { get => BPM / 60; }

    public float SecondsPerCycle { get => BeatsPerCycle / BeatsPerSecond; }

    private void OnValidate()
    {
        bpm = Mathf.Max(Mathf.Epsilon, bpm);
        beatsPerCycle = Mathf.Max(Mathf.Epsilon, beatsPerCycle);
    }
}