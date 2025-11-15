using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class DeltaTransform
{
    [SerializeField]
    private Vector3 position = Vector3.zero;

    public Vector3 Position => position;

    [SerializeField]
    private Vector3 scale = Vector3.one;

    public Vector3 Scale => scale;

    [SerializeField]
    private Vector3 rotation = Vector3.zero;

    public Vector3 Rotation => rotation;
}

[Serializable]
public class ColorSettings
{
    [SerializeField]
    private ColorMode colorMode = ColorMode.None;

    public ColorMode ColorMode => colorMode;

    [SerializeField]
    private List<Color> colors = new()
    {
        Color.red,
        Color.yellow,
        Color.green,
        Color.cyan,
        Color.blue,
        Color.magenta,
    };

    public List<Color> Colors => colors;

    [Min(2)]
    [SerializeField]
    private int gradientLength = 10;

    public int GradientLength
    {
        set { gradientLength = Mathf.Max(2, value); }
        get { return gradientLength; }
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
    /// Specify a list of colors in the order in which they should be applied to game objects
    /// </summary>
    ColorList,

    /// <summary>
    /// Use the color list as a gradient that interpolates across game objects
    /// </summary>
    Gradient,
}

// Unfortunately we cannot use ExecuteAlways nor ExecuteInEditMode here because
// for color settings we need to modify materials, which is not allowed in edit mode.
// Only shared materials can be modified in edit mode, which would change the material for all objects using it.
// So we have to rely on the custom editor to call Init() and Clear() manually.
public class ClonePattern : MonoBehaviour
{
    [SerializeField]
    private ColorSettings colorSettings = new();

    public ColorMode ColorMode => colorSettings.ColorMode;
    public List<Color> Colors => colorSettings.Colors;
    public int GradientLength
    {
        set { colorSettings.GradientLength = Mathf.Max(2, value); }
        get { return colorSettings.GradientLength; }
    }

    [SerializeField]
    private DeltaTransform deltaTransform = new();

    public Vector3 DeltaPosition => deltaTransform.Position;
    public Vector3 DeltaScale => deltaTransform.Scale;
    public Vector3 DeltaRotation => deltaTransform.Rotation;

    // The original game object to clone
    // should be a prefab; not a child of the ClonePattern object
    [SerializeField]
    private GameObject originalGameObject;

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
        Util.Assert(ColorMode != ColorMode.None, "GetColor called when colorMode is None");
        Util.Assert(Colors.Count > 0, "Color list is empty but colorMode requires colors");

        return ColorMode switch
        {
            ColorMode.ColorList => Colors[index % Colors.Count],
            ColorMode.Gradient => GetColorFromGradient(
                (float)(index % GradientLength) / (GradientLength - 1)
            ),
            _ => throw new Exception(),
        };
    }

    private Color GetColorFromGradient(float t)
    {
        Util.Assert(Colors.Count >= 2, "At least two colors are required for gradient mode.");

        float scaledPosition = t * (Colors.Count - 1);
        int lowerIndex = Mathf.FloorToInt(scaledPosition);
        int upperIndex = Mathf.Min(lowerIndex + 1, Colors.Count - 1);
        float localT = scaledPosition - lowerIndex;
        return Color.Lerp(Colors[lowerIndex], Colors[upperIndex], localT);
    }

    private void SetColor(GameObject gameObjectToColor, Color color)
    {
        if (
            gameObjectToColor.TryGetComponent<Renderer>(out var renderer)
            && renderer.material != null
        )
        {
            renderer.material.color = color;
        }
        if (gameObjectToColor.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            spriteRenderer.color = color;
        }
        if (gameObjectToColor.TryGetComponent<UnityEngine.UI.Image>(out var image))
        {
            image.color = color;
        }
        if (gameObjectToColor.TryGetComponent<UnityEngine.UI.Text>(out var text))
        {
            text.color = color;
        }

        foreach (Transform child in gameObjectToColor.transform)
        {
            SetColor(child.gameObject, color);
        }
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        Util.Assert(transform.childCount == NumClones, "Number of clones does not match NumClones");
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
        Util.RequireNonNull(originalGameObject, "originalGameObject");
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

        // Iterate backwards to avoid issues with changing indices while deleting
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
