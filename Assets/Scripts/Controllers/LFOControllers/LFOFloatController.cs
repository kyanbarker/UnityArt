using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An LFOController to automate float events.
/// </summary>
public class LFOFloatController : LFOController<float>
{
    public UnityFloatEvent action;

    protected override float LerpValue(float min, float max, float t)
    {
        return Mathf.Lerp(min, max, t);
    }

    protected override void InvokeAction(float value)
    {
        action.Invoke(value);
    }
}
