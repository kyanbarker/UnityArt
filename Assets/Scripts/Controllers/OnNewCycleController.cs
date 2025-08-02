using UnityEngine;
using UnityEngine.Events;

public class NewCycleEvent : UnityEvent<int> { }

public class OnNewCycleController : MonoBehaviour
{
    [SerializeField]
    private ICycleController cycleController;
    public ICycleController CycleController
    {
        get => cycleController;
        set => cycleController = value;
    }

    [SerializeField]
    private NewCycleEvent action;

    private int currentCycleIndex = 0;

    void Update()
    {
        if (cycleController.NumElapsedCycles > currentCycleIndex)
        {
            action.Invoke(currentCycleIndex);
            currentCycleIndex++;
        }
    }
}
