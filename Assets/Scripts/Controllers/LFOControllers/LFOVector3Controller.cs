using UnityEngine;

/// <summary>
/// An LFOController to automate vector3 events.
/// </summary>
public class LFOVector3Controller : LFOController<Vector3>
{
    public UnityVector3Event action;

    protected override Vector3 LerpValue(Vector3 min, Vector3 max, float t)
    {
        return Vector3.Lerp(min, max, t);
    }

    protected override void InvokeAction(Vector3 value)
    {
        action.Invoke(value);
    }
}
