using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class NewCycleEvent : UnityEvent<int> { }

public class OnNewCycleController : MonoBehaviour
{
    [SerializeField]
    private MonoBehaviour cycleController;

    public ICycleController CycleController
    {
        get => cycleController as ICycleController;
        set => cycleController = value as MonoBehaviour;
    }

    [SerializeField]
    private NewCycleEvent action;

    private int currentCycleIndex = 0;

    void Update()
    {
        if (CycleController.NumElapsedCycles > currentCycleIndex)
        {
            action.Invoke(currentCycleIndex);
            currentCycleIndex++;
        }
    }
}
