using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Imposes an inclusively minimum value on a serialized int or float.
/// </summary>
[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class MinAttribute : PropertyAttribute
{
    public float MinValue { get; }

    public MinAttribute(float minValue)
    {
        MinValue = minValue;
    }

    public MinAttribute(int minValue)
    {
        MinValue = minValue;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(MinAttribute))]
public class MinAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        MinAttribute minAttribute = (MinAttribute)attribute;

        EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField(position, property, label);

        if (EditorGUI.EndChangeCheck())
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Float:
                    property.floatValue = Mathf.Max(property.floatValue, minAttribute.MinValue);
                    break;
                case SerializedPropertyType.Integer:
                    property.intValue = Mathf.Max(property.intValue, (int)minAttribute.MinValue);
                    break;
                default:
                    Debug.LogWarning(
                        $"MinAttribute is not supported on {property.propertyType} properties"
                    );
                    break;
            }
        }
    }
}
#endif
