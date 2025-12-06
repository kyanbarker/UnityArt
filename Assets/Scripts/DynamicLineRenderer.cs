using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Dynamically updates a LineRenderer to follow the positions of specified GameObjects.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class DynamicLineRenderer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> points = new();

    public List<GameObject> Points
    {
        get => points;
        set => points = value;
    }

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
        Assert.IsNotNull(points);
        Assert.IsNotNull(lineRenderer);
        lineRenderer.positionCount = points.Count;
        UpdateLinePositions();
    }

    void Update()
    {
        UpdateLinePositions();
    }

    private void UpdateLinePositions()
    {
        Assert.IsNotNull(points);
        Assert.IsNotNull(lineRenderer);

        // Update position count in case the list changed
        if (lineRenderer.positionCount != points.Count)
        {
            lineRenderer.positionCount = points.Count;
        }

        // Update each position to match the GameObject positions
        for (int i = 0; i < points.Count; i++)
        {
            Assert.IsNotNull(points[i]);
            lineRenderer.SetPosition(i, points[i].transform.position);
        }
    }
}
