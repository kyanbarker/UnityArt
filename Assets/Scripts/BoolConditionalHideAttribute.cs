using UnityEngine;
using System;
using UnityEditor;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class BoolConditionalHideAttribute : PropertyAttribute
{
    public string ConditionalSourceField = "";
    public bool HideInInspector = false;
    public bool Inverse = false;

    public BoolConditionalHideAttribute(string boolSourceField)
    {
        this.ConditionalSourceField = boolSourceField;
        this.HideInInspector = false;
        this.Inverse = false;
    }

    public BoolConditionalHideAttribute(string conditionalSourceField, bool hideInInspector)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = hideInInspector;
        this.Inverse = false;
    }

    public BoolConditionalHideAttribute(string conditionalSourceField, bool hideInInspector, bool inverse)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = hideInInspector;
        this.Inverse = inverse;
    }
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(BoolConditionalHideAttribute))]
public class BoolConditionalHideDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        BoolConditionalHideAttribute conditionalHideAttribute = (BoolConditionalHideAttribute)attribute;
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
        BoolConditionalHideAttribute conditionalHideAttribute = (BoolConditionalHideAttribute)attribute;
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

    private bool GetConditionalHideAttributeResult(BoolConditionalHideAttribute conditionalHideAttribute, SerializedProperty property)
    {
        bool enabled = true;
        string propertyPath = property.propertyPath;
        string conditionPath = propertyPath.Replace(property.name, conditionalHideAttribute.ConditionalSourceField);
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

        if (sourcePropertyValue != null)
        {
            enabled = sourcePropertyValue.boolValue;
        }
        else
        {
            Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + conditionalHideAttribute.ConditionalSourceField);
        }

        if (conditionalHideAttribute.Inverse) enabled = !enabled;

        return enabled;
    }
}
#endif
