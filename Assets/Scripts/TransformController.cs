using UnityEngine;

public class TransformController : MonoBehaviour
{
    public float PositionX
    {
        get => transform.position.x;
        set
        {
            Vector3 position = transform.position;
            position.x = value;
            transform.position = position;
        }
    }

    public float PositionY
    {
        get => transform.position.y;
        set
        {
            Vector3 position = transform.position;
            position.y = value;
            transform.position = position;
        }
    }

    public float PositionZ
    {
        get => transform.position.z;
        set
        {
            Vector3 position = transform.position;
            position.z = value;
            transform.position = position;
        }
    }

    public float RotationX
    {
        get => transform.eulerAngles.x;
        set
        {
            Vector3 rotation = transform.eulerAngles;
            rotation.x = value;
            transform.eulerAngles = rotation;
        }
    }

    public float RotationY
    {
        get => transform.eulerAngles.y;
        set
        {
            Vector3 rotation = transform.eulerAngles;
            rotation.y = value;
            transform.eulerAngles = rotation;
        }
    }

    public float RotationZ
    {
        get => transform.eulerAngles.z;
        set
        {
            Vector3 rotation = transform.eulerAngles;
            rotation.z = value;
            transform.eulerAngles = rotation;
        }
    }

    public float ScaleX
    {
        get => transform.localScale.x;
        set
        {
            Vector3 scale = transform.localScale;
            scale.x = value;
            transform.localScale = scale;
        }
    }

    public float ScaleY
    {
        get => transform.localScale.y;
        set
        {
            Vector3 scale = transform.localScale;
            scale.y = value;
            transform.localScale = scale;
        }
    }

    public float ScaleZ
    {
        get => transform.localScale.z;
        set
        {
            Vector3 scale = transform.localScale;
            scale.z = value;
            transform.localScale = scale;
        }
    }
}
