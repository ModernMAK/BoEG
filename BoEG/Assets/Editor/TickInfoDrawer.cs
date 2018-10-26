using System;
using Entity.Abilities;
using Modules.Abilityable;
using UnityEditor;
using UnityEngine;

namespace EditorSpace
{
    [CustomPropertyDrawer(typeof(TickData))]
    public class TickInfoDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 16;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var requiredTicks = property.FindPropertyRelative("_required");
            var totalDuration = property.FindPropertyRelative("_duration");

//            EditorGUI.BeginProperty(position, label, property);

            var indent = EditorGUI.indentLevel;
            var labelWidth = EditorGUIUtility.labelWidth;
            var contentPosition = EditorGUI.PrefixLabel(position, label);
            var left = new Rect(contentPosition.x, contentPosition.y, contentPosition.width / 3f,
                contentPosition.height);
            var middle = left;
            middle.x += middle.width;
            var right = middle;
            right.x += right.width;

            EditorGUI.indentLevel = 0;
            EditorGUIUtility.labelWidth = 15f;
            EditorGUI.PropertyField(left, requiredTicks, new GUIContent("T"));
            EditorGUI.PropertyField(middle, totalDuration, new GUIContent("D"));
            var interval = totalDuration.floatValue / requiredTicks.intValue;
            GUI.enabled = false;
            EditorGUI.FloatField(right, new GUIContent("I"), interval);
            GUI.enabled = true;
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUI.indentLevel = indent;
        }
    }
}