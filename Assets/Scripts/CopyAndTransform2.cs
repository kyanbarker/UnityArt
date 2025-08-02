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

    [SerializeField]
    private List<GameObject> originalGameObjects;

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
        numCopies = Mathf.Max(0, numCopies);
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
        Color originalColor = GetColorForIndex(0);
        foreach (var gameObject in originalGameObjects)
        {
            ApplyColorToGameObject(gameObject, originalColor);
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
                int totalColors = recolorOriginalGameObjects ? numCopies + 1 : numCopies;
                if (totalColors <= 1)
                    return minColor;
                float t = (float)index / (totalColors - 1);
                return Color.Lerp(minColor, maxColor, t);

            default:
                return Color.white;
        }
    }

    private void ApplyColorToGameObject(GameObject gameObjectToColor, Color color)
    {
        // Try to find and set color on various common components
        Renderer renderer = gameObjectToColor.GetComponent<Renderer>();
        if (renderer != null && renderer.material != null)
        {
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
        if (copyIndex >= numCopies)
            enabled = false;

        foreach (var gameObjectToClone in originalGameObjects)
        {
            if (gameObjectToClone.CompareTag("Clone"))
                return;

            var clone = Instantiate(gameObjectToClone, transform, true);
            clone.name = gameObjectToClone.name + " Clone " + copyIndex;

            // Apply position transformation
            clone.transform.localPosition = deltaPosition * (copyIndex + 1);

            // Apply rotation transformation (each axis independently)
            Vector3 totalRotation = deltaRotation * (copyIndex + 1);
            clone.transform.rotation *= Quaternion.Euler(totalRotation);

            // Apply scale transformation
            clone.transform.localScale = Vector3.Scale(
                gameObjectToClone.transform.localScale,
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
