using UnityEngine;
using UnityEngine.Assertions;

public class GradientCloneEffect : MonoBehaviour, ICloneEffect
{
    public int GradientLength { get; set; } = 1;
    public Gradient Gradient { get; set; } = new();

    public void Apply(int index, GameObject clone)
    {
        Assert.IsTrue(GradientLength > 0, "GradientLength must be positive");

        float denominator = Mathf.Max(GradientLength - 1, 1);
        float t = (float)(index % GradientLength) / denominator;
        Color color = Gradient.Evaluate(t);
        ApplyColor(clone, color);
    }

    private void ApplyColor(GameObject clone, Color color)
    {
        ColorController[] colorController = clone.GetComponentsInChildren<ColorController>();
        foreach (var controller in colorController)
        {
            controller.Color = color;
        }
    }
}
