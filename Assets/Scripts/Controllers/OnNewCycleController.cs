using UnityEngine;
using UnityEngine.Events;

public class NewCycleEvent : UnityEvent<int> { }

public class OnNewCycleController : CycleInBeatsController
{
    [SerializeField]
    private NewCycleEvent action;

    private int currentCycleIndex = 0;

    void Update()
    {
        if (TimeBeats / BeatsPerCycle > currentCycleIndex)
        {
            action.Invoke(currentCycleIndex);
            currentCycleIndex++;
        }
    }
}
