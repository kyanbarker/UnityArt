using UnityEngine;

/// <summary>
/// An LFO controller to automate color events.
/// </summary>
public class LFOColorController : LFOController<Color>
{
    public UnityColorEvent action;

    protected override Color LerpValue(Color min, Color max, float t)
    {
        return Color.Lerp(min, max, t);
    }

    protected override void InvokeAction(Color value)
    {
        action.Invoke(value);
    }
}
