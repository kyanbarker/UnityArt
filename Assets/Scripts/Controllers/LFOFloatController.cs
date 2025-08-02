using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class LFOFloatEvent : UnityEvent<float> { }

/// <summary>
/// An LFOController to automate float events.
/// </summary>
public class LFOFloatController : LFOController<float>
{
    public LFOFloatEvent action;

    protected override float LerpValue(float min, float max, float t)
    {
        return Mathf.Lerp(min, max, t);
    }

    protected override void InvokeAction(float value)
    {
        action.Invoke(value);
    }
}
