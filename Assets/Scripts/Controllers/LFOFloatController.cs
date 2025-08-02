using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class LFOFloatEvent : UnityEvent<float> { }

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