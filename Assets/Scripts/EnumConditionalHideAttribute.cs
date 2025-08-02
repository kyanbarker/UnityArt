using UnityEngine;
using System;
using UnityEditor;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class EnumConditionalHideAttribute : PropertyAttribute
{
    public string ConditionalSourceField = "";
    public int EnumValue = 0;
    public bool HideInInspector = false;
    public bool Inverse = false;

    public EnumConditionalHideAttribute(string conditionalSourceField, int enumValue)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.EnumValue = enumValue;
        this.HideInInspector = false;
        this.Inverse = false;
    }

    public EnumConditionalHideAttribute(string conditionalSourceField, int enumValue, bool hideInInspector)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.EnumValue = enumValue;
        this.HideInInspector = hideInInspector;
        this.Inverse = false;
    }

    public EnumConditionalHideAttribute(string conditionalSourceField, int enumValue, bool hideInInspector, bool inverse)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.EnumValue = enumValue;
        this.HideInInspector = hideInInspector;
        this.Inverse = inverse;
    }
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(EnumConditionalHideAttribute))]
public class EnumConditionalHideDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EnumConditionalHideAttribute conditionalHideAttribute = (EnumConditionalHideAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(conditionalHideAttribute, property);

        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;
        if (!conditionalHideAttribute.HideInInspector || enabled)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
        GUI.enabled = wasEnabled;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        EnumConditionalHideAttribute conditionalHideAttribute = (EnumConditionalHideAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(conditionalHideAttribute, property);

        if (!conditionalHideAttribute.HideInInspector || enabled)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    private bool GetConditionalHideAttributeResult(EnumConditionalHideAttribute conditionalHideAttribute, SerializedProperty property)
    {
        bool enabled = false;
        string propertyPath = property.propertyPath;
        string conditionPath = propertyPath.Replace(property.name, conditionalHideAttribute.ConditionalSourceField);
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

        if (sourcePropertyValue != null)
        {
            enabled = sourcePropertyValue.enumValueIndex == conditionalHideAttribute.EnumValue;
        }
        else
        {
            Debug.LogWarning("Attempting to use a ConditionalHideEnumAttribute but no matching SourcePropertyValue found in object: " + conditionalHideAttribute.ConditionalSourceField);
        }

        if (conditionalHideAttribute.Inverse) enabled = !enabled;

        return enabled;
    }
}
#endif

// Example usage with ColorMode enum:
/*
public enum ColorMode { None, ColorList, ColorGradient }

[SerializeField]
private ColorMode colorMode = ColorMode.None;

[SerializeField]
[ConditionalHideEnum("colorMode", (int)ColorMode.ColorList)] // Show only when ColorList is selected
private List<Color> colors;

[SerializeField]
[ConditionalHideEnum("colorMode", (int)ColorMode.ColorGradient)] // Show only when ColorGradient is selected
private Color minColor;

[SerializeField]
[ConditionalHideEnum("colorMode", (int)ColorMode.ColorGradient)] // Show only when ColorGradient is selected
private Color maxColor;

[SerializeField]
[ConditionalHideEnum("colorMode", (int)ColorMode.None, true, true)] // Hide when None is selected
private bool recolorOriginalGameObject;
*/