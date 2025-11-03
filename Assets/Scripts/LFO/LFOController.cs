using UnityEngine;

public class LFOController : MonoBehaviour
{
    [SerializeField]
    private LFOWaveform waveform;

    [SerializeField, Min(0.0001f)]
    private float frequency = 1f; // in Hz

    [SerializeField, Range(0f, 1f)]
    private float phaseOffset = 0f;

    [SerializeReference]
    private LFOTarget[] targets;

    private float elapsedTime = 0f;

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        float period = 1f / frequency;
        float normalizedTime = Mathf.Repeat(elapsedTime / period + phaseOffset, 1f);
        float lfoValue = waveform.Evaluate(normalizedTime);

        Util.RequireNonNull(targets, "targets");
        for (int i = 0; i < targets.Length; i++)
        {
            Util.RequireNonNull(targets[i], $"targets[{i}]");
            targets[i].Invoke(lfoValue);
        }
    }
}
