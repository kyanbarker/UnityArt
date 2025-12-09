using UnityEngine;
using UnityEngine.Assertions;

public class LFOController : MonoBehaviour
{
    public LFOWaveform Waveform { get; set; }

    public float Frequency { get; set; }

    public float PhaseOffset { get; set; }

    public LFOTarget[] Targets { get; set; }

    private void Update()
    {
        float period = 1f / Frequency;
        float normalizedTime = Mathf.Repeat(Time.time / period + PhaseOffset, 1f);
        float waveformValue = Waveform.Evaluate(normalizedTime);

        Assert.IsNotNull(Targets);
        for (int i = 0; i < Targets.Length; i++)
        {
            Assert.IsNotNull(Targets[i]);
            Targets[i].Invoke(waveformValue);
        }
    }
}
