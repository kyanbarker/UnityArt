using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ColorMode to use to transform colors of copies
/// </summary>
public enum ColorMode
{
    /// <summary>
    /// Do not transform colors of copies
    /// </summary>
    None,

    /// <summary>
    /// Specify a list of colors in the order in which they should be applied to copies
    /// (and possibly original gameobjects depending on `recolorOriginalGameObjects`)
    /// </summary>
    ColorList,

    /// <summary>
    /// Use the color list as a gradient that interpolates across copies
    /// </summary>
    Gradient,
}

public class CopyAndTransform : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> originalGameObjects;
    public List<GameObject> OriginalGameObjects
    {
        get => originalGameObjects.Count > 0 ? originalGameObjects : Children;
        set => originalGameObjects = value;
    }

    public List<GameObject> Children
    {
        get
        {
            var children = new List<GameObject>();
            var childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                children.Add(transform.GetChild(i).gameObject);
            }
            return children;
        }
    }

    [Header("Physical Transform")]
    [SerializeField]
    private Vector3 deltaPosition = Vector3.zero;

    [SerializeField]
    private Vector3 deltaScale = Vector3.one;

    [SerializeField]
    private Vector3 deltaRotation = Vector3.zero;

    [Header("Color Transform")]
    [SerializeField]
    private ColorMode colorMode = ColorMode.None;

    [SerializeField]
    [HideIfEqual("colorMode", (int)ColorMode.None)]
    private bool recolorOriginalGameObjects = false;

    [SerializeField]
    [HideIfEqual("colorMode", (int)ColorMode.None)]
    private List<Color> colors = new() { Color.red, Color.green, Color.blue };

    [SerializeField]
    [ShowIfEqual("colorMode", (int)ColorMode.Gradient)]
    [Min(2)]
    private int gradientLength = 10;

    private void Awake()
    {
        // Apply color to original objects if needed
        if (colorMode != ColorMode.None && recolorOriginalGameObjects)
        {
            Color color0 = GetColorForIndex(0);
            foreach (var originalGameObject in OriginalGameObjects)
            {
                ApplyColorToGameObject(originalGameObject, color0);
            }
        }
    }

    private Color GetColorForIndex(int index)
    {
        if (colors.Count == 0)
        {
            Debug.LogWarning("Color list is empty but colorMode requires colors");
            return Color.white;
        }

        switch (colorMode)
        {
            case ColorMode.ColorList:
                return colors[index % colors.Count];

            case ColorMode.Gradient:
                // Calculate position in gradient cycle
                float gradientPosition = (float)(index % gradientLength) / (gradientLength - 1);
                return GetColorFromGradient(gradientPosition);

            default:
                return Color.white;
        }
    }

    private Color GetColorFromGradient(float t)
    {
        if (colors.Count == 0)
            return Color.white;

        if (colors.Count == 1)
            return colors[0];

        // Scale t to the color list range
        float scaledPosition = t * (colors.Count - 1);

        // Find the two colors to lerp between
        int lowerIndex = Mathf.FloorToInt(scaledPosition);
        int upperIndex = Mathf.Min(lowerIndex + 1, colors.Count - 1);

        // Calculate local interpolation value
        float localT = scaledPosition - lowerIndex;

        // Lerp between the two colors
        return Color.Lerp(colors[lowerIndex], colors[upperIndex], localT);
    }

    private void ApplyColorToGameObject(GameObject gameObjectToColor, Color color)
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

        // Recursively apply to children
        foreach (Transform child in gameObjectToColor.transform)
        {
            ApplyColorToGameObject(child.gameObject, color);
        }
    }

    public void Execute(int copyIndex)
    {
        Debug.Log(copyIndex);
        foreach (var originalGameObject in OriginalGameObjects)
        {
            if (originalGameObject.CompareTag("Copy"))
            {
                return;
            }

            var copy = Instantiate(originalGameObject, transform, true);
            copy.name = originalGameObject.name + " Copy " + copyIndex;

            // Apply position transformation
            Vector3 totalDisplacement = deltaPosition * (copyIndex + 1);
            copy.transform.position += totalDisplacement;

            // Apply rotation transformation (each axis independently)
            Vector3 totalRotation = deltaRotation * (copyIndex + 1);
            copy.transform.rotation *= Quaternion.Euler(totalRotation);

            // Apply scale transformation
            copy.transform.localScale = Vector3.Scale(
                originalGameObject.transform.localScale,
                new Vector3(
                    Mathf.Pow(deltaScale.x, copyIndex + 1),
                    Mathf.Pow(deltaScale.y, copyIndex + 1),
                    Mathf.Pow(deltaScale.z, copyIndex + 1)
                )
            );

            // Apply color if color mode is enabled
            if (colorMode != ColorMode.None)
            {
                int colorIndex = recolorOriginalGameObjects ? copyIndex + 1 : copyIndex;
                Color copyColor = GetColorForIndex(colorIndex);
                ApplyColorToGameObject(copy, copyColor);
            }

            copy.tag = "Copy";
        }
    }
}
