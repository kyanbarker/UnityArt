using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Shows a serialized field iff the value of `SourceField` equals `CompareValue`
/// </summary>
[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class ShowIfEqualAttribute : PropertyAttribute
{
    public string SourceField { get; }
    public object CompareValue { get; }

    public ShowIfEqualAttribute(string sourceField, object compareValue)
    {
        SourceField = sourceField;
        CompareValue = compareValue;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ShowIfEqualAttribute))]
public class ShowIfEqualDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfEqualAttribute showAttribute = (ShowIfEqualAttribute)attribute;
        bool shouldShow = ShouldShow(showAttribute, property);

        if (shouldShow)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfEqualAttribute showAttribute = (ShowIfEqualAttribute)attribute;
        bool shouldShow = ShouldShow(showAttribute, property);

        if (shouldShow)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        // Return 0 to completely hide the field without taking up space
        return 0;
    }

    private bool ShouldShow(ShowIfEqualAttribute showAttribute, SerializedProperty property)
    {
        string propertyPath = property.propertyPath;
        string conditionPath = propertyPath.Replace(property.name, showAttribute.SourceField);
        SerializedProperty sourceProperty = property.serializedObject.FindProperty(conditionPath);

        if (sourceProperty == null)
        {
            // Try to find the property at the root level if not found as sibling
            sourceProperty = property.serializedObject.FindProperty(showAttribute.SourceField);
        }

        if (sourceProperty != null)
        {
            return AreValuesEqual(sourceProperty, showAttribute.CompareValue);
        }

        Debug.LogWarning(
            $"Cannot find property {showAttribute.SourceField} for ShowIfEqual attribute on {property.propertyPath}"
        );
        return true; // Default to showing if we can't find the source
    }

    private bool AreValuesEqual(SerializedProperty property, object compareValue)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.Boolean:
                return property.boolValue == Convert.ToBoolean(compareValue);
            case SerializedPropertyType.Integer:
                return property.intValue == Convert.ToInt32(compareValue);
            case SerializedPropertyType.Float:
                return Mathf.Approximately(property.floatValue, Convert.ToSingle(compareValue));
            case SerializedPropertyType.String:
                return property.stringValue == compareValue.ToString();
            case SerializedPropertyType.Enum:
                return property.enumValueIndex == Convert.ToInt32(compareValue);
            case SerializedPropertyType.ObjectReference:
                return property.objectReferenceValue == compareValue as UnityEngine.Object;
            default:
                Debug.LogWarning(
                    $"ShowIfEqual does not support {property.propertyType} type comparisons"
                );
                return true;
        }
    }
}
#endif
