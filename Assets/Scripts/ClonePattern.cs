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

[ExecuteAlways]
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

    private enum GameObjectsOption
    {
        UseExternalGameObject,
        UseChild,
    }

    [SerializeField]
    private GameObjectsOption gameObjectsOption = GameObjectsOption.UseChild;

    private GameObject originalGameObject;

    [SerializeField]
    private GameObject externalGameObject;

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

    [SerializeField]
    private List<GameObject> gameObjects = new();

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
        Util.RequireDifferent(ColorMode, ColorMode.None);
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
            && renderer.sharedMaterial != null
        )
        {
            renderer.sharedMaterial.color = color;
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
        switch (gameObjectsOption)
        {
            case GameObjectsOption.UseChild:
                Util.RequireEquals(transform.childCount, 1, "transform.childCount");
                originalGameObject = transform.GetChild(0).gameObject;
                break;
            case GameObjectsOption.UseExternalGameObject:
                Util.RequireNonNull(externalGameObject, "externalGameObject");
                originalGameObject = externalGameObject;
                break;
        }
        Util.RequireNonNull(originalGameObject, "originalGameObject");

        // must include this!
        // it will break without this!
        gameObjects.Add(originalGameObject);
    }

    private void Update()
    {
        // Spawn up to NumClones
        while (gameObjects.Count < NumClones)
        {
            int cloneIndex = gameObjects.Count;
            CreateClone(cloneIndex);
        }

        // Remove extra clones if NumClones decreased
        while (gameObjects.Count > NumClones)
        {
            DestroyLastClone();
        }

        // Update transform and color for each game object after changes
        for (int i = 0; i < gameObjects.Count; i++)
        {
            TransformClone(i, gameObjects[i]);
            if (ColorMode != ColorMode.None)
            {
                SetColor(gameObjects[i], GetColor(i));
            }
        }
    }

    private void DestroyLastClone()
    {
        Util.Assert(gameObjects.Count > 1, "No clones to destroy.");
        var last = gameObjects[gameObjects.Count - 1];
        gameObjects.RemoveAt(gameObjects.Count - 1);
        Util.RequireNonNull(last, "last");
        Util.RequireDifferent(last, originalGameObject, "last", "originalGameObject");
        DestroyImmediate(last);
    }

    private void CreateClone(int index)
    {
        if (index <= 0)
        {
            throw new Exception("CreateClone should only be called with a positive index.");
        }
        var clone = Instantiate(originalGameObject, transform, true);
        clone.name = originalGameObject.name + " Clone " + index;
        gameObjects.Add(clone);
        TransformClone(index, clone);
        if (ColorMode != ColorMode.None)
        {
            SetColor(clone, GetColor(index));
        }
    }
}
