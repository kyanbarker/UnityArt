using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class MaxAttribute : PropertyAttribute
{
    public float MaxValue { get; }

    public MaxAttribute(float maxValue)
    {
        MaxValue = maxValue;
    }

    public MaxAttribute(int maxValue)
    {
        MaxValue = maxValue;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(MaxAttribute))]
public class MaxAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        MaxAttribute maxAttribute = (MaxAttribute)attribute;
        
        EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField(position, property, label);
        
        if (EditorGUI.EndChangeCheck())
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Float:
                    property.floatValue = Mathf.Min(property.floatValue, maxAttribute.MaxValue);
                    break;
                case SerializedPropertyType.Integer:
                    property.intValue = Mathf.Min(property.intValue, (int)maxAttribute.MaxValue);
                    break;
                default:
                    Debug.LogWarning($"MaxAttribute is not supported on {property.propertyType} properties");
                    break;
            }
        }
    }
}
#endif