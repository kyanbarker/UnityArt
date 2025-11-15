using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ClonePattern))]
public class ClonePatternEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();
        var clonePattern = (ClonePattern)target;
        if (GUILayout.Button("Clear"))
        {
            clonePattern.Clear();
            // Mark the target dirty so the editor knows it changed
            EditorUtility.SetDirty(clonePattern);
        }
        if (GUILayout.Button("Init"))
        {
            clonePattern.Init();
            // Mark the target dirty so the editor knows it changed
            EditorUtility.SetDirty(clonePattern);
        }
    }
}
