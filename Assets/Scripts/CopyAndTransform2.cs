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
    /// Specify a min and max color and a gradient will be applied across copies
    /// (and possibly original gameobjects dpeending on `recolorOriginalGameObjects`)
    /// </summary>
    ColorGradient,
}

public class CopyAndTransform2 : MonoBehaviour
{
    [SerializeField]
    private int numCopies = 0;
    public int NumCopies
    {
        get => numCopies;
        set => numCopies = value;
    }

    [SerializeField]
    private List<GameObject> originalGameObjects;
    public List<GameObject> OriginalGameObjects
    {
        get => originalGameObjects;
        set => originalGameObjects = value;
    }

    [SerializeField]
    private Vector3 deltaPosition = Vector3.zero;

    [SerializeField]
    private Vector3 deltaScale = Vector3.one;

    [SerializeField]
    private Vector3 deltaRotation = Vector3.zero;

    [SerializeField]
    private ColorMode colorMode = ColorMode.None;

    [SerializeField]
    [EnumConditionalHide("colorMode", (int)ColorMode.None, true, true)] // Hide when None is selected
    private bool recolorOriginalGameObjects = false;

    [SerializeField]
    [EnumConditionalHide("colorMode", (int)ColorMode.ColorList)] // Show only when ColorList is selected
    private List<Color> colors = new() { Color.red, Color.green, Color.blue };

    [SerializeField]
    [EnumConditionalHide("colorMode", (int)ColorMode.ColorGradient)] // Show only when ColorGradient is selected
    private Color minColor = Color.black;

    [SerializeField]
    [EnumConditionalHide("colorMode", (int)ColorMode.ColorGradient)] // Show only when ColorGradient is selected
    private Color maxColor = Color.white;

    private void OnValidate()
    {
        NumCopies = Mathf.Max(0, NumCopies);
    }

    private void Start()
    {
        // Apply color to original objects if needed
        if (colorMode != ColorMode.None && recolorOriginalGameObjects)
        {
            ApplyColorToOriginalGameObjects();
        }
    }

    private void ApplyColorToOriginalGameObjects()
    {
        Color color0 = GetColorForIndex(0);
        foreach (var originalGameObject in OriginalGameObjects)
        {
            ApplyColorToGameObject(originalGameObject, color0);
        }
    }

    private Color GetColorForIndex(int index)
    {
        switch (colorMode)
        {
            case ColorMode.ColorList:
                if (colors.Count == 0)
                    return Color.white;
                return colors[index % colors.Count];

            case ColorMode.ColorGradient:
                int totalColors = recolorOriginalGameObjects ? NumCopies + 1 : NumCopies;
                if (totalColors <= 1)
                    return minColor;
                float t = (float)index / (totalColors - 1);
                return Color.Lerp(minColor, maxColor, t);

            default:
                throw new System.Exception();
        }
    }

    private void ApplyColorToGameObject(GameObject gameObjectToColor, Color color)
    {
        if (gameObjectToColor.TryGetComponent<Renderer>(out var renderer))
        {
            if (renderer.material != null)
                renderer.material.color = color;

            return;
        }
        if (gameObjectToColor.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            spriteRenderer.color = color;
            return;
        }
        if (gameObjectToColor.TryGetComponent<UnityEngine.UI.Image>(out var image))
        {
            image.color = color;
            return;
        }
        if (gameObjectToColor.TryGetComponent<UnityEngine.UI.Text>(out var text))
        {
            text.color = color;
            return;
        }
        for (int i = 0; i < gameObjectToColor.transform.childCount; i++)
        {
            var child = gameObjectToColor.transform.GetChild(i);
            ApplyColorToGameObject(child.gameObject, color);
        }
    }

    public void CreateCopy(int copyIndex)
    {
        if (copyIndex >= NumCopies)
            return;

        foreach (var originalGameObject in OriginalGameObjects)
        {
            if (originalGameObject.CompareTag("Clone"))
                return;

            var clone = Instantiate(originalGameObject, transform, true);
            clone.name = originalGameObject.name + " Clone " + copyIndex;

            // Apply position transformation
            clone.transform.localPosition = deltaPosition * (copyIndex + 1);

            // Apply rotation transformation (each axis independently)
            Vector3 totalRotation = deltaRotation * (copyIndex + 1);
            clone.transform.rotation *= Quaternion.Euler(totalRotation);

            // Apply scale transformation
            clone.transform.localScale = Vector3.Scale(
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
                ApplyColorToGameObject(clone, copyColor);
            }

            clone.tag = "Clone";
        }
    }
}
