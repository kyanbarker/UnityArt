using UnityEngine;

public class DeltaTransformCloneEffect : MonoBehaviour, ICloneEffect
{
    public Vector3 DeltaPosition { get; set; } = Vector3.zero;
    public Vector3 DeltaScale { get; set; } = Vector3.one;
    public Vector3 DeltaRotation { get; set; } = Vector3.zero;

    public void Apply(int index, GameObject clone)
    {
        clone.transform.localPosition = DeltaPosition * index;
        clone.transform.localRotation = Quaternion.Euler(DeltaRotation * index);
        clone.transform.localScale = new Vector3(
            Mathf.Pow(DeltaScale.x, index),
            Mathf.Pow(DeltaScale.y, index),
            Mathf.Pow(DeltaScale.z, index)
        );
    }
}
