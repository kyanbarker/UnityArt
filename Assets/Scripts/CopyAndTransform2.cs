using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ColorMode to use to transform colors of copies
/// </summary>
public enum ColorMode
{
    None,
    ColorList,
    ColorGradient,
}

public class CopyAndTransform2 : MonoBehaviour
{
    [SerializeField]
    private int numCopies = 0;

    [SerializeField]
    private Vector3 deltaPosition = Vector3.zero;

    [SerializeField]
    private Vector3 deltaScale = Vector3.one;

    [SerializeField]
    private Vector3 deltaRotation = Vector3.zero;

    [SerializeField]
    private bool useParentBPM = true;

    [SerializeField]
    [BoolConditionalHide("useParentBPM", true, true)]
    [Range(60, 200)]
    private float bpm = 120f;

    [SerializeField]
    [Range(0, 16)]
    private float delayBeats = 1f;

    [Header("Color Settings")]
    [SerializeField]
    private ColorMode colorMode = ColorMode.None;

    [SerializeField]
    [EnumConditionalHide("colorMode", (int)ColorMode.None, true, true)] // Hide when None is selected
    private bool recolorOriginalGameObject = false;

    [Header("Color List Mode")]
    [SerializeField]
    [EnumConditionalHide("colorMode", (int)ColorMode.ColorList)] // Show only when ColorList is selected
    private List<Color> colors = new List<Color> { Color.red, Color.green, Color.blue };

    [Header("Color Gradient Mode")]
    [SerializeField]
    [EnumConditionalHide("colorMode", (int)ColorMode.ColorGradient)] // Show only when ColorGradient is selected
    private Color minColor = Color.black;

    [SerializeField]
    [EnumConditionalHide("colorMode", (int)ColorMode.ColorGradient)] // Show only when ColorGradient is selected
    private Color maxColor = Color.white;

    private BPMTime cachedBPMComponent;

    public float BPM
    {
        get
        {
            if (useParentBPM)
            {
                if (cachedBPMComponent == null)
                {
                    CacheBPMComponent();
                }

                if (cachedBPMComponent != null)
                {
                    return cachedBPMComponent.BPM;
                }
            }

            return bpm;
        }
        set => bpm = Mathf.Max(Mathf.Epsilon, value);
    }

    public bool UseParentBPM
    {
        get => useParentBPM;
        set
        {
            useParentBPM = value;
            if (value)
            {
                CacheBPMComponent();
            }
        }
    }

    [SerializeField]
    private List<GameObject> gameObjectsToClone;

    private void OnValidate()
    {
        bpm = Mathf.Max(Mathf.Epsilon, bpm);
        delayBeats = Mathf.Max(0, delayBeats);
        numCopies = Mathf.Max(0, numCopies);
    }

    private void Start()
    {
        if (useParentBPM)
        {
            CacheBPMComponent();
        }

        // Apply color to original objects if needed
        if (colorMode != ColorMode.None && recolorOriginalGameObject)
        {
            ApplyColorToOriginals();
        }

        StartCoroutine(CreateCopiesOverTime());
    }

    private void CacheBPMComponent()
    {
        cachedBPMComponent = GetComponentInParent<BPMTime>();
    }

    private float GetSecondsFromBeats(float beats)
    {
        float beatsPerSecond = BPM / 60;
        return beats / beatsPerSecond;
    }

    private void ApplyColorToOriginals()
    {
        Color originalColor = GetColorForIndex(0);
        foreach (var gameObject in gameObjectsToClone)
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
                int totalColors = recolorOriginalGameObject ? numCopies + 1 : numCopies;
                if (totalColors <= 1)
                    return minColor;
                float t = (float)index / (totalColors - 1);
                return Color.Lerp(minColor, maxColor, t);

            default:
                return Color.white;
        }
    }

    private void ApplyColorToGameObject(GameObject obj, Color color)
    {
        // Try to find and set color on various common components
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null && renderer.material != null)
        {
            renderer.material.color = color;
            return;
        }

        // Try SpriteRenderer for 2D sprites
        if (obj.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            spriteRenderer.color = color;
            return;
        }

        // Try UI Image component
        if (obj.TryGetComponent<UnityEngine.UI.Image>(out var image))
        {
            image.color = color;
            return;
        }

        // Try UI Text component
        if (obj.TryGetComponent<UnityEngine.UI.Text>(out var text))
        {
            text.color = color;
            return;
        }

        // If no suitable component found, try children
        renderer = obj.GetComponentInChildren<Renderer>();
        if (renderer != null && renderer.material != null)
        {
            renderer.material.color = color;
        }
    }

    private void CreateCopy(int copyIndex)
    {
        foreach (var gameObjectToClone in gameObjectsToClone)
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
                    Mathf.Pow(deltaScale.x, (copyIndex + 1)),
                    Mathf.Pow(deltaScale.y, (copyIndex + 1)),
                    Mathf.Pow(deltaScale.z, (copyIndex + 1))
                )
            );

            // Apply color if color mode is enabled
            if (colorMode != ColorMode.None)
            {
                int colorIndex = recolorOriginalGameObject ? copyIndex + 1 : copyIndex;
                Color copyColor = GetColorForIndex(colorIndex);
                ApplyColorToGameObject(clone, copyColor);
            }

            clone.tag = "Clone";
        }
    }

    private IEnumerator CreateCopiesOverTime()
    {
        int remainingCopies = numCopies;
        int copyIndex = 0;
        while (remainingCopies > 0)
        {
            // Wait first, then create (for consistent timing)
            float delayInSeconds = GetSecondsFromBeats(delayBeats);
            yield return new WaitForSeconds(delayInSeconds);

            CreateCopy(copyIndex);
            copyIndex++;
            remainingCopies--;
        }
    }
}
