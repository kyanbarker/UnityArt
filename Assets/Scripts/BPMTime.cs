using UnityEngine;

/// <summary>
/// A component which keeps track of a bpm value
/// </summary>
public class BPMTime : MonoBehaviour
{
    [SerializeField]
    [Min(1e-10f)]
    private float bpm = 120f;

    public float BPM
    {
        get => bpm;
        set => bpm = Mathf.Max(Mathf.Epsilon, value);
    }
}
