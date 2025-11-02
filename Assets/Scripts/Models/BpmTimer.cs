using UnityEngine;

/// <summary>
/// A component which keeps track of a bpm value
/// </summary>
public class BpmTimer : MonoBehaviour
{
    [SerializeField]
    [Min(1e-10f)]
    private float bpm = 120f;

    public float BPM
    {
        get => bpm;
        set => bpm = Mathf.Max(1e-10f, value);
    }
}
