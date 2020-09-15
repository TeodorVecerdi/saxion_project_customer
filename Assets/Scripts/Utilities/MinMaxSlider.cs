using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public struct MinMax
{

    [SerializeField]
    private float min;
    [SerializeField]
    private float max;

    public float Min
    {
        get
        {
            return this.min;
        }
        set
        {
            this.min = value;
        }
    }

    public float Max
    {
        get
        {
            return this.max;
        }
        set
        {
            this.max = value;
        }
    }

    public float RandomValue
    {
        get
        {
            return UnityEngine.Random.Range(this.min, this.max);
        }
    }

    public MinMax(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public float Clamp(float value)
    {
        return Mathf.Clamp(value, this.min, this.max);
    }

}

public class MinMaxSliderAttribute : PropertyAttribute
{

    public readonly float Min;
    public readonly float Max;

    public MinMaxSliderAttribute(float min, float max)
    {
        Min = min;
        Max = max;
    }

}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(MinMax))]
[CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
public class MinMaxDrawer : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.serializedObject.isEditingMultipleObjects) return 0f;
        return base.GetPropertyHeight(property, label) + 16f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.serializedObject.isEditingMultipleObjects) return;
        
        var indentedPosition = EditorGUI.IndentedRect(position);

        var minProperty = property.FindPropertyRelative("min");
        var maxProperty = property.FindPropertyRelative("max");
        var minmax = attribute as MinMaxSliderAttribute ?? new MinMaxSliderAttribute(-1f, 1f);
        indentedPosition.height -= 16f;

        label = EditorGUI.BeginProperty(indentedPosition, label, property);
        indentedPosition = EditorGUI.PrefixLabel(indentedPosition, GUIUtility.GetControlID(FocusType.Passive), label);
        var min = minProperty.floatValue;
        var max = maxProperty.floatValue;
        var left = new Rect(indentedPosition.x, indentedPosition.y, indentedPosition.width / 2 - 11f, indentedPosition.height);
        var right = new Rect(indentedPosition.x + indentedPosition.width - left.width, indentedPosition.y, left.width, indentedPosition.height);
        var mid = new Rect(left.x + left.width, indentedPosition.y, indentedPosition.width, indentedPosition.height);
        min = Mathf.Clamp(EditorGUI.FloatField(left, min), minmax.Min, max);
        EditorGUI.LabelField(mid, "to");
        max = Mathf.Clamp(EditorGUI.FloatField(right, max), min, minmax.Max);

        indentedPosition.y += 16f;
        EditorGUI.MinMaxSlider(indentedPosition, GUIContent.none, ref min, ref max, minmax.Min, minmax.Max);

        minProperty.floatValue = min;
        maxProperty.floatValue = max;
        EditorGUI.EndProperty();
    }

}
#endif