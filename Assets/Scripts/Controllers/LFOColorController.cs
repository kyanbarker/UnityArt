using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class LFOColorEvent : UnityEvent<Color> { }

public class LFOColorController : LFOController<Color>
{
    public LFOColorEvent action;

    protected override Color LerpValue(Color min, Color max, float t)
    {
        return Color.Lerp(min, max, t);
    }

    protected override void InvokeAction(Color value)
    {
        action.Invoke(value);
    }
}