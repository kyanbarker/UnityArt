using UnityEngine;

/// <summary>
/// A controller which invokes the specified action upon each new cycle
/// </summary>
public class OnNewCycleController : MonoBehaviour
{
    [SerializeField]
    private MonoBehaviour cycleController;

    [SerializeField, Min(-1)]
    private int maxCycles = 1; // -1 indicates infinite cycles

    public ICycleController CycleController
    {
        get => cycleController as ICycleController;
        set => cycleController = value as MonoBehaviour;
    }

    [SerializeField]
    private UnityIntEvent action;

    private int currentCycleIndex = 0;

    void Update()
    {
        if (maxCycles != -1 && maxCycles <= currentCycleIndex)
        {
            return;
        }
        // else maxCycles == -1 (infinite cycles) or maxCycles > currentCycleIndex; thus, we continue

        bool isNewCycle = CycleController.TimeCycles > currentCycleIndex;
        if (isNewCycle)
        {
            action.Invoke(currentCycleIndex);
            currentCycleIndex++;
        }
    }
}
