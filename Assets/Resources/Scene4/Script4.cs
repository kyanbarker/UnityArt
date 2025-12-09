using UnityEngine;

public class Script4 : MonoBehaviour
{
    void Start()
    {
        LFOWaveform sineWaveform = Resources
            .Load<GameObject>("Waveforms/Sine")
            .GetComponent<LFOWaveform>();

        LFOWaveform linearWaveform = Resources
            .Load<GameObject>("Waveforms/Linear")
            .GetComponent<LFOWaveform>();

        int numTrails = 5;
        float initialFrequency = 1f;

        for (int i = 0; i < numTrails; i++)
        {
            float frequency = initialFrequency * (i + 1);
            GameObject trail = new($"Trail {frequency} Hz");
            TrailRenderer trailRenderer = trail.AddComponent<TrailRenderer>();
            trailRenderer.time = 0.1f;
            trailRenderer.startWidth = 0.1f;
            trailRenderer.endWidth = 0.0f;
            trailRenderer.material = new Material(Shader.Find("Sprites/Default"));

            TransformController transformController = trail.AddComponent<TransformController>();

            LFOFloatTarget trailXTarget = trail.AddComponent<LFOFloatTarget>();
            trailXTarget.Min = -10f;
            trailXTarget.Max = 10f;
            trailXTarget.Action = x => transformController.PositionX = x;

            LFOController trailXController = trail.AddComponent<LFOController>();
            trailXController.Waveform = sineWaveform;
            trailXController.Frequency = frequency / 4;
            trailXController.Targets = new LFOTarget[] { trailXTarget };

            LFOFloatTarget trailYTarget = trail.AddComponent<LFOFloatTarget>();
            trailYTarget.Min = -5f;
            trailYTarget.Max = 5f;
            trailYTarget.Action = y => transformController.PositionY = y;

            LFOController trailYController = trail.AddComponent<LFOController>();
            trailYController.Waveform = sineWaveform;
            trailYController.Frequency = frequency;
            trailYController.Targets = new LFOTarget[] { trailYTarget };
        }
    }

    void Update() { }
}
