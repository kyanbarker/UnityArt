using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Dynamically updates a LineRenderer to follow the positions of specified GameObjects.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class DynamicLineRenderer : MonoBehaviour
{
    public List<GameObject> Points { get; set; }

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
        Assert.IsNotNull(Points);
        Assert.IsNotNull(lineRenderer);
        lineRenderer.positionCount = Points.Count;
        UpdateLinePositions();
    }

    void Update()
    {
        UpdateLinePositions();
    }

    private void UpdateLinePositions()
    {
        Assert.IsNotNull(Points);
        Assert.IsNotNull(lineRenderer);

        // Update position count in case the list changed
        if (lineRenderer.positionCount != Points.Count)
        {
            lineRenderer.positionCount = Points.Count;
        }

        // Update each position to match the GameObject positions
        for (int i = 0; i < Points.Count; i++)
        {
            Assert.IsNotNull(Points[i]);
            lineRenderer.SetPosition(i, Points[i].transform.position);
        }
    }
}
