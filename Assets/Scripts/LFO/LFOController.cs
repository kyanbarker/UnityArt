using UnityEngine;
using UnityEngine.Assertions;

public class LFOController : MonoBehaviour
{
    [SerializeField]
    private LFOWaveform waveform;

    public LFOWaveform Waveform
    {
        get => waveform;
        set => waveform = value;
    }

    [SerializeField, Min(0.0001f)]
    private float frequency = 1f; // in Hz

    public float Frequency
    {
        get => frequency;
        set => frequency = value;
    }

    [SerializeField, Range(0f, 1f)]
    private float phaseOffset = 0f;

    public float PhaseOffset
    {
        get => phaseOffset;
        set => phaseOffset = value;
    }

    [SerializeReference]
    private LFOTarget[] targets;

    public LFOTarget[] Targets
    {
        get => targets;
        set => targets = value;
    }

    private void Update()
    {
        float period = 1f / frequency;
        float normalizedTime = Mathf.Repeat(Time.time / period + phaseOffset, 1f);
        float lfoValue = waveform.Evaluate(normalizedTime);

        Assert.IsNotNull(targets);
        for (int i = 0; i < targets.Length; i++)
        {
            Assert.IsNotNull(targets[i]);
            targets[i].Invoke(lfoValue);
        }
    }
}
