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
            var interval = property.FindPropertyRelative("_interval");
            var duration = property.FindPropertyRelative("_duration");

//            EditorGUI.BeginProperty(position, label, property);

            var indent = EditorGUI.indentLevel;
            var labelWidth = EditorGUIUtility.labelWidth;
            var contentPosition = EditorGUI.PrefixLabel(position, label);
            var left = new Rect(contentPosition.x, contentPosition.y, contentPosition.width / 2f,
                contentPosition.height);
            var right = left;
            right.x += right.width;

            EditorGUI.indentLevel = 0;
            EditorGUIUtility.labelWidth = 15f;
            EditorGUI.PropertyField(left, duration, new GUIContent("D"));
            EditorGUI.PropertyField(right,  interval,new GUIContent("I"));
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUI.indentLevel = indent;
        }
    }
}