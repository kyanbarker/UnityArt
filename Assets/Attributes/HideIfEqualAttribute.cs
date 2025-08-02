using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class HideIfEqualAttribute : PropertyAttribute
{
    public string SourceField { get; }
    public object CompareValue { get; }

    public HideIfEqualAttribute(string sourceField, object compareValue)
    {
        SourceField = sourceField;
        CompareValue = compareValue;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(HideIfEqualAttribute))]
public class HideIfEqualDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        HideIfEqualAttribute hideAttribute = (HideIfEqualAttribute)attribute;
        bool shouldHide = ShouldHide(hideAttribute, property);

        if (!shouldHide)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        HideIfEqualAttribute hideAttribute = (HideIfEqualAttribute)attribute;
        bool shouldHide = ShouldHide(hideAttribute, property);

        if (!shouldHide)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        
        return -EditorGUIUtility.standardVerticalSpacing;
    }

    private bool ShouldHide(HideIfEqualAttribute hideAttribute, SerializedProperty property)
    {
        string propertyPath = property.propertyPath;
        string conditionPath = propertyPath.Replace(property.name, hideAttribute.SourceField);
        SerializedProperty sourceProperty = property.serializedObject.FindProperty(conditionPath);

        if (sourceProperty == null)
        {
            // Try to find the property at the root level if not found as sibling
            sourceProperty = property.serializedObject.FindProperty(hideAttribute.SourceField);
        }

        if (sourceProperty != null)
        {
            return AreValuesEqual(sourceProperty, hideAttribute.CompareValue);
        }

        Debug.LogWarning($"Cannot find property {hideAttribute.SourceField} for HideIfEqual attribute on {property.propertyPath}");
        return false;
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
                Debug.LogWarning($"HideIfEqual does not support {property.propertyType} type comparisons");
                return false;
        }
    }
}
#endif