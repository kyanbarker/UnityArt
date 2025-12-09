using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Script3 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float minX = -30f;
        float maxX = 30f;
        float minY = -20f;
        float maxY = 20f;
        float minZ = 0f;
        float maxZ = 0f;

        int numVertices = 21;
        float movementSpeed = 5f;

        GameObject verticesParent = new("VerticesParent");

        List<GameObject> vertices = new();

        // Create bounds for DVD screen movement
        Vector3 boundsCenter = new((minX + maxX) / 2f, (minY + maxY) / 2f, (minZ + maxZ) / 2f);
        Vector3 boundsSize = new(maxX - minX, maxY - minY, maxZ - minZ);
        Bounds movementBounds = new(boundsCenter, boundsSize);

        for (int i = 0; i < numVertices; i++)
        {
            Vector3 randomPosition = new(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY),
                Random.Range(minZ, maxZ)
            );
            GameObject vertex = new($"Vertex {i}");
            vertex.transform.position = randomPosition;
            vertex.transform.parent = verticesParent.transform;

            // Add DVD screen movement component
            DVDScreenMovement movement = vertex.AddComponent<DVDScreenMovement>();
            movement.Speed = movementSpeed;
            movement.Bounds = movementBounds;

            vertices.Add(vertex);
        }

        GameObject linesParent = new("LinesParent");

        for (int i = 0; i < vertices.Count; i++)
        {
            for (int j = i + 1; j < vertices.Count; j++)
            {
                GameObject line = new($"Line ({i}, {j})");
                LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, vertices[i].transform.position);
                lineRenderer.SetPosition(1, vertices[j].transform.position);
                lineRenderer.startWidth = 0.1f;
                lineRenderer.endWidth = 0.1f;
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.startColor = Color.white;
                lineRenderer.endColor = Color.white;

                // Add DynamicLineRenderer to update line positions as vertices move
                DynamicLineRenderer dynamicLine = line.AddComponent<DynamicLineRenderer>();
                dynamicLine.Points = new List<GameObject> { vertices[i], vertices[j] };

                line.transform.parent = linesParent.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
