using UnityEngine;

/// <summary>
/// Controls the color of a GameObject without creating new material instances.
/// Uses MaterialPropertyBlock for efficient per-object color overrides.
/// </summary>
[ExecuteAlways]
public class ColorController : MonoBehaviour
{
    [SerializeField]
    private Color color = Color.white;

    public Color Color
    {
        get => color;
        set
        {
            color = value;
            ApplyColor();
        }
    }

    private MaterialPropertyBlock propertyBlock;
    private static readonly int ColorPropertyID = Shader.PropertyToID("_Color");
    private static readonly int BaseColorPropertyID = Shader.PropertyToID("_BaseColor");

    void OnEnable()
    {
        propertyBlock ??= new MaterialPropertyBlock();
        ApplyColor();
    }

    void OnValidate()
    {
        ApplyColor();
    }

    void Update()
    {
        // Only needed in edit mode to keep colors synced with inspector changes
        if (!Application.isPlaying)
        {
            ApplyColor();
        }
    }

    private void ApplyColor()
    {
        // Standard Renderer (MeshRenderer, etc.)
        if (TryGetComponent<Renderer>(out var renderer) && renderer.sharedMaterial != null)
        {
            propertyBlock ??= new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propertyBlock);

            // Try both _Color (Built-in RP) and _BaseColor (URP/HDRP)
            propertyBlock.SetColor(ColorPropertyID, color);
            propertyBlock.SetColor(BaseColorPropertyID, color);

            renderer.SetPropertyBlock(propertyBlock);
        }

        // SpriteRenderer uses direct color property (no MaterialPropertyBlock needed)
        if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            spriteRenderer.color = color;
        }

        // UI Components use direct color property
        if (TryGetComponent<UnityEngine.UI.Image>(out var image))
        {
            image.color = color;
        }

        if (TryGetComponent<UnityEngine.UI.Text>(out var text))
        {
            text.color = color;
        }
    }
}
