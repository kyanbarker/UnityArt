using System;
using System.Collections.Generic;
using UnityEngine;

public class ClonePattern : MonoBehaviour
{
    [SerializeField]
    private GameObject originalGameObject;

    [SerializeField, Min(0)]
    private int numCopies = 0;

    [Header("Transform Pattern")]
    [SerializeField]
    private Vector3 deltaPosition = Vector3.zero;

    [SerializeField]
    private Vector3 deltaScale = Vector3.one;

    [SerializeField]
    private Vector3 deltaRotation = Vector3.zero;

    [Header("Color Pattern")]
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

    [SerializeField]
    private List<GameObject> copies = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Optionally, recolor the original once at start
        if (colorMode != ColorMode.None && recolorOriginalGameObjects)
        {
            SetColor(originalGameObject, GetColor(0));
        }

        // Initial creation of clones (if any desired)
        int desiredCount = numCopies - 1;
        for (int i = 0; i < desiredCount; i++)
        {
            var copy = Instantiate(originalGameObject, transform, true);
            copy.name = originalGameObject.name + " Copy " + (i + 1);
            copies.Add(copy);
            TransformCopy(i + 1, copy);
            SetColor(copy, GetColor(i + 1));
        }
    }

    void Update()
    {
        // Determine desired number of clones (excluding the original)
        int desiredCount = numCopies - 1;

        // Add clones if desiredCount is greater than current count
        while (copies.Count < desiredCount)
        {
            int cloneIndex = copies.Count + 1;
            var copy = Instantiate(originalGameObject, transform, true);
            copy.name = originalGameObject.name + " Copy " + cloneIndex;
            copies.Add(copy);
            // Initialize transform and color for the new clone
            TransformCopy(cloneIndex, copy);
            SetColor(copy, GetColor(cloneIndex));
        }

        // Remove clones if desiredCount is less than the current count
        while (copies.Count > desiredCount)
        {
            var lastClone = copies[copies.Count - 1];
            copies.RemoveAt(copies.Count - 1);
            if (lastClone != null)
            {
                DestroyImmediate(lastClone);
            }
        }

        // Update transform and color for each clone after changes
        for (int i = 0; i < copies.Count; i++)
        {
            int cloneIndex = i + 1;
            TransformCopy(cloneIndex, copies[i]);
            SetColor(copies[i], GetColor(cloneIndex));
        }
    }

    private void TransformCopy(int copyIndex, GameObject copy)
    {
        Vector3 totalDisplacement = deltaPosition * copyIndex;
        copy.transform.localPosition = originalGameObject.transform.localPosition + totalDisplacement;

        Vector3 totalRotation = deltaRotation * copyIndex;
        copy.transform.localRotation =
            originalGameObject.transform.localRotation * Quaternion.Euler(totalRotation);

        copy.transform.localScale = Vector3.Scale(
            originalGameObject.transform.localScale,
            new Vector3(
                Mathf.Pow(deltaScale.x, copyIndex),
                Mathf.Pow(deltaScale.y, copyIndex),
                Mathf.Pow(deltaScale.z, copyIndex)
            )
        );
    }

    private Color GetColor(int index)
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

        float scaledPosition = t * (colors.Count - 1);
        int lowerIndex = Mathf.FloorToInt(scaledPosition);
        int upperIndex = Mathf.Min(lowerIndex + 1, colors.Count - 1);
        float localT = scaledPosition - lowerIndex;
        return Color.Lerp(colors[lowerIndex], colors[upperIndex], localT);
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
}
