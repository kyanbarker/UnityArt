using UnityEngine;
using UnityEngine.Serialization;

public class InvokeUnityIntEventsOnStart : MonoBehaviour
{
    [SerializeField, Min(0)]
    [FormerlySerializedAs("k")]
    private int numIterations = 0;

    [SerializeField]
    private int start = 0;

    [SerializeField]
    private int step = 1;

    [SerializeField]
    private UnityIntEvent action;

    void Start()
    {
        for (int i = 0; i < numIterations; i++)
        {
            action.Invoke(start + i * step);
        }
    }
}
