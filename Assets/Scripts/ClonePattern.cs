using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ClonePattern : MonoBehaviour
{
    /// <summary>
    /// The ColorMode to use to transform colors of copies
    /// </summary>
    private enum ColorMode
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

    private GameObject originalGameObject;

    [SerializeField, Min(1)]
    [FormerlySerializedAs("numCopies")]
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

    [Header("Transform Pattern")]
    [SerializeField]
    private Vector3 deltaPosition = Vector3.zero;
    public Vector3 DeltaPosition
    {
        get { return deltaPosition; }
        set { deltaPosition = value; }
    }

    [SerializeField]
    private Vector3 deltaScale = Vector3.one;
    public Vector3 DeltaScale
    {
        get { return deltaScale; }
        set { deltaScale = value; }
    }

    [SerializeField]
    private Vector3 deltaRotation = Vector3.zero;
    public Vector3 DeltaRotation
    {
        get { return deltaRotation; }
        set { deltaRotation = value; }
    }

    [Header("Color Pattern")]
    [SerializeField]
    private ColorMode colorMode = ColorMode.None;

    [SerializeField]
    // [HideIfEqual("colorMode", (int)ColorMode.None)]
    private List<Color> colors = new() { Color.red, Color.green, Color.blue };

    [SerializeField]
    // [ShowIfEqual("colorMode", (int)ColorMode.Gradient)]
    [Min(2)]
    private int gradientLength = 10;

    // We explicit getters and setters instead of using arrow syntax,
    // because Unity can serialize these as UnityEvent<int>
    public int GradientLength
    {
        get { return gradientLength; }
        set { gradientLength = Mathf.Max(2, value); }
    }

    /// <summary>
    /// List of all game objects (original + clones).
    /// </summary>
    [SerializeField]
    private List<GameObject> gameObjects = new();

    private void TransformClone(int cloneIndex, GameObject clone)
    {
        Vector3 totalDisplacement = deltaPosition * cloneIndex;
        clone.transform.localPosition =
            originalGameObject.transform.localPosition + totalDisplacement;

        Vector3 totalRotation = deltaRotation * cloneIndex;
        clone.transform.localRotation =
            originalGameObject.transform.localRotation * Quaternion.Euler(totalRotation);

        clone.transform.localScale = Vector3.Scale(
            originalGameObject.transform.localScale,
            new Vector3(
                Mathf.Pow(deltaScale.x, cloneIndex),
                Mathf.Pow(deltaScale.y, cloneIndex),
                Mathf.Pow(deltaScale.z, cloneIndex)
            )
        );
    }

    private Color GetColor(int index)
    {
        if (colorMode == ColorMode.None)
        {
            Debug.LogWarning("GetColor called when colorMode is None");
            return Color.white;
        }
        if (colors.Count == 0)
        {
            Debug.LogWarning("Color list is empty but colorMode requires colors");
            return Color.white;
        }

        return colorMode switch
        {
            ColorMode.ColorList => colors[index % colors.Count],
            ColorMode.Gradient => GetColorFromGradient(
                (float)(index % gradientLength) / (gradientLength - 1)
            ),
            _ => throw new Exception(),
        };
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

    // Copies should be called in Awake instead of Start so that all clones are spawned at the same time
    // Awake is called before Start.
    void Awake()
    {
        if (transform.childCount == 1)
        {
            originalGameObject = transform.GetChild(0).gameObject;
        }
        else
        {
            Debug.LogWarning("child count is not 1!");
            return;
        }
        gameObjects.Add(originalGameObject);

        // Optionally, recolor the original once at start
        if (colorMode != ColorMode.None)
        {
            SetColor(originalGameObject, GetColor(0));
        }

        // See note on clone indices in `CreateClone`.
        // Creating a 0'th clone would duplicate the original, so we start from index 1.
        for (int cloneIndex = 1; cloneIndex <= NumClones - 1; cloneIndex++)
        {
            CreateClone(cloneIndex);
        }
    }

    /// <summary>
    /// Creates a clone of the original game object;
    /// Clone index 0 is the original gameobject,
    /// clone index 1 is the 1st gameobject,
    /// clone index 2 is the 2nd gameobject, etc.
    /// </summary>
    private void CreateClone(int index)
    {
        if (index <= 0)
        {
            Debug.LogWarning(
                "Clone index = 0 refers to the original game object. Clone index < 0 is meaningless. CreateClone should only be called with a positive index."
            );
            return;
        }
        var clone = Instantiate(originalGameObject, transform, true);
        clone.name = originalGameObject.name + " Clone " + index;
        gameObjects.Add(clone);
        TransformClone(index, clone);
        if (colorMode != ColorMode.None)
        {
            SetColor(clone, GetColor(index));
        }
    }

    void Update()
    {
        // Add clones if NumClones is greater than current count
        while (gameObjects.Count < NumClones)
        {
            int cloneIndex = gameObjects.Count;
            CreateClone(cloneIndex);
        }

        // Remove clones if desiredCount is less than the current count
        while (gameObjects.Count > NumClones)
        {
            var lastClone = gameObjects[gameObjects.Count - 1];
            gameObjects.RemoveAt(gameObjects.Count - 1);
            if (lastClone != null)
            {
                DestroyImmediate(lastClone);
            }
        }

        // Update transform and color for each game object after changes
        for (int i = 0; i < gameObjects.Count; i++)
        {
            TransformClone(i, gameObjects[i]);
            if (colorMode != ColorMode.None)
            {
                SetColor(gameObjects[i], GetColor(i));
            }
        }
    }
}
