using UnityEditor;
using UnityEngine;

public class AnimationCurveViewOnlyAttribute : PropertyAttribute { }

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(AnimationCurveViewOnlyAttribute))]
public class AnimationCurveViewOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Prevent editing
        GUI.enabled = false;

        // Draw the curve field
        EditorGUI.CurveField(position, property, Color.green, new Rect(0, 0, 1, 1), label);

        // Re-enable GUI for following controls
        GUI.enabled = true;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
#endif
