using UnityEngine;

public class LFOIntController : LFOController<int>
{
    [SerializeField]
    private UnityIntEvent action;

    protected override void InvokeAction(int value)
    {
        action.Invoke(value);
    }

    protected override int LerpValue(int min, int max, float t)
    {
        return Mathf.RoundToInt(Mathf.Lerp(min, max, t));
    }
}
