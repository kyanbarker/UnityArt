using UnityEngine;

public class CycleInSecondsController : MonoBehaviour
{
    [SerializeField]
    private float secondsPerCycle = 1;
    public float SecondsPerCycle { get => secondsPerCycle; set => secondsPerCycle = value; }

    private void OnValidate()
    {
        SecondsPerCycle = Mathf.Max(Mathf.Epsilon, SecondsPerCycle);
    }
}
