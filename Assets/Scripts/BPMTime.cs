using UnityEngine;

/// <summary>
/// A component which keeps track of a bpm value
/// </summary>
public class BPMTime : MonoBehaviour
{
    [SerializeField]
    private float bpm = 120f;

    public float BPM
    {
        get => bpm;
        set => bpm = Mathf.Max(Mathf.Epsilon, value);
    }

    private void OnValidate()
    {
        bpm = Mathf.Max(Mathf.Epsilon, bpm);
    }
}
