using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

[Serializable]
public class DeltaTransform
{
    [SerializeField]
    private Vector3 position = Vector3.zero;

    public Vector3 Position
    {
        get => position;
        set => position = value;
    }

    [SerializeField]
    private Vector3 scale = Vector3.one;

    public Vector3 Scale
    {
        get => scale;
        set => scale = value;
    }

    [SerializeField]
    private Vector3 rotation = Vector3.zero;

    public Vector3 Rotation
    {
        get => rotation;
        set => rotation = value;
    }
}

/// <summary>
/// The ColorMode to use to transform colors of game objects
/// </summary>
public enum ColorMode
{
    /// <summary>
    /// Do not transform colors of game objects
    /// </summary>
    None,

    /// <summary>
    /// Use the color list as a gradient that interpolates across game objects
    /// </summary>
    Gradient,
}

[Serializable]
public class ColorSettings
{
    [SerializeField]
    private ColorMode colorMode = ColorMode.None;

    public ColorMode ColorMode
    {
        get { return colorMode; }
        set { colorMode = value; }
    }

    // Keep an integer length so users can specify discrete sampling/repeats
    // across the Gradient (matches previous behaviour).
    [Min(2)]
    [SerializeField]
    private int gradientLength = 10;

    public int GradientLength
    {
        set { gradientLength = Mathf.Max(2, value); }
        get { return gradientLength; }
    }

    // Use Unity's built-in Gradient type for Gradient mode.
    [SerializeField]
    private Gradient gradient = new();

    public Gradient Gradient
    {
        get { return gradient; }
        set { gradient = value; }
    }
}

public class ClonePattern : MonoBehaviour
{
    [SerializeField]
    private ColorSettings colorSettings = new();

    public ColorMode ColorMode
    {
        get { return colorSettings.ColorMode; }
        set { colorSettings.ColorMode = value; }
    }
    public int GradientLength
    {
        set { colorSettings.GradientLength = Mathf.Max(2, value); }
        get { return colorSettings.GradientLength; }
    }
    public Gradient Gradient
    {
        get { return colorSettings.Gradient; }
        set { colorSettings.Gradient = value; }
    }

    [SerializeField]
    public DeltaTransform deltaTransform = new();

    public Vector3 DeltaPosition
    {
        get => deltaTransform.Position;
        set => deltaTransform.Position = value;
    }
    public Vector3 DeltaScale
    {
        get => deltaTransform.Scale;
        set => deltaTransform.Scale = value;
    }
    public Vector3 DeltaRotation
    {
        get => deltaTransform.Rotation;
        set => deltaTransform.Rotation = value;
    }

    // The original game object to clone
    // should be a prefab; not a child of the ClonePattern object
    [SerializeField]
    private GameObject originalGameObject;

    public GameObject OriginalGameObject
    {
        get { return originalGameObject; }
        set { originalGameObject = value; }
    }

    [SerializeField, Min(1)]
    private int numClones = 1;

    /// <summary>
    /// The number of clones in this pattern.
    /// This number counts the original game object as a clone.
    /// </summary>
    public int NumClones
    {
        get { return numClones; }
        set { numClones = Mathf.Max(1, value); }
    }

    private void TransformClone(int cloneIndex, GameObject clone)
    {
        Vector3 totalDisplacement = DeltaPosition * cloneIndex;
        clone.transform.localPosition =
            originalGameObject.transform.localPosition + totalDisplacement;

        Vector3 totalRotation = DeltaRotation * cloneIndex;
        clone.transform.localRotation =
            originalGameObject.transform.localRotation * Quaternion.Euler(totalRotation);

        clone.transform.localScale = Vector3.Scale(
            originalGameObject.transform.localScale,
            new Vector3(
                Mathf.Pow(DeltaScale.x, cloneIndex),
                Mathf.Pow(DeltaScale.y, cloneIndex),
                Mathf.Pow(DeltaScale.z, cloneIndex)
            )
        );
    }

    private Color GetColor(int index)
    {
        Assert.AreNotEqual(ColorMode, ColorMode.None, "GetColor called when colorMode is None");
        float t = (float)(index % GradientLength) / (GradientLength - 1);
        return Gradient.Evaluate(t);
    }

    private void SetColor(GameObject gameObjectToColor, Color color)
    {
        // Get or add ColorController component and set its color
        if (!gameObjectToColor.TryGetComponent<ColorController>(out var colorController))
        {
            colorController = gameObjectToColor.AddComponent<ColorController>();
        }
        colorController.Color = color;
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        Assert.AreEqual(
            transform.childCount,
            NumClones,
            "Number of clones does not match NumClones"
        );
        for (int i = 0; i < NumClones; i++)
        {
            GameObject clone = transform.GetChild(i).gameObject;
            TransformClone(i, clone);
            if (ColorMode != ColorMode.None)
            {
                SetColor(clone, GetColor(i));
            }
        }
    }

    public void Init()
    {
        Assert.IsNotNull(originalGameObject, "originalGameObject");
        Clear();
        for (int i = 0; i < NumClones; i++)
        {
            CreateClone(i);
        }
    }

    public void Clear()
    {
        // The foreach loop version does not work for whatever reason
        // foreach (Transform child in transform)
        // {
        //     DestroyImmediate(child.gameObject);
        // }

        // Iterate backwards to avoid issues with changing indices while deleting.
        // Iterating forward effectively skips some children
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    private GameObject CreateClone(int index)
    {
        var clone = Instantiate(originalGameObject, transform, true);
        clone.name = originalGameObject.name + " Clone " + index;
        TransformClone(index, clone);
        if (ColorMode != ColorMode.None)
        {
            SetColor(clone, GetColor(index));
        }
        return clone;
    }
}
