using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class LFOVector3Event : UnityEvent<Vector3> { }

public class LFOVector3Controller : LFOController<Vector3>
{
    public LFOVector3Event action;

    protected override Vector3 LerpValue(Vector3 min, Vector3 max, float t)
    {
        return Vector3.Lerp(min, max, t);
    }

    protected override void InvokeAction(Vector3 value)
    {
        action.Invoke(value);
    }
}