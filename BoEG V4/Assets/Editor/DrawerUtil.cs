using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class DrawerUtil
    {
        public static Rect DrawSpace(Rect position, float percentage = 0.5f)
        {
            position.y += position.height * percentage;
            return position;
        }

        public static Rect DrawLine(Rect position)
        {
            //    Stolen from
            //    https://gist.github.com/MattRix/5972828

            var rect = position;

            if (Event.current.type == EventType.Repaint) //draw the divider
            {
                rect.x = 14.0f;
                rect.y += 5.0f;
                rect.height = 1.0f;
                rect.width -= 14.0f;

                GUI.skin.box.Draw(rect, GUIContent.none, 0);
            }
            position.y += position.height;
            return position;
        }

        public static Rect DisplayBaseGain(Rect position, SerializedProperty baseVal, SerializedProperty gainVal,
            GUIContent label, bool allowEmpty = false)
        {
            var indent = EditorGUI.indentLevel;
            EditorGUI.LabelField(position, label);
            EditorGUI.indentLevel++;
            position.y += position.height;
            EditorGUI.PropertyField(position, baseVal, new GUIContent("Base"));
            position.y += position.height;
            EditorGUI.PropertyField(position, gainVal, new GUIContent("Gain"));
            position.y += position.height;
            EditorGUI.indentLevel = indent;
            return position;
        }

        public static Rect DisplayBase(Rect position, SerializedProperty baseVal, GUIContent label,
            bool allowEmpty = false)
        {
            var indent = EditorGUI.indentLevel;
            EditorGUI.LabelField(position, label);
            EditorGUI.indentLevel++;
            position.y += position.height;
            EditorGUI.PropertyField(position, baseVal, new GUIContent("Base"));
            position.y += position.height;
            EditorGUI.indentLevel = indent;
            return position;
        }

//
//        [Obsolete]
//        public static Rect DisplayBaseGainRowLabel(Rect position, GUIContent label, bool allowEmpty = false)
//        {
//            var indent = EditorGUI.indentLevel;
//            if (!allowEmpty && (label == GUIContent.none || label.text == ""))
//                label = new GUIContent(" "); //Forces the label to be present; aligns the labels below
//
//            var tempPos = EditorGUI.PrefixLabel(position, label, EditorStyles.boldLabel);
//            tempPos.width /= 2f;
//            EditorGUI.indentLevel = 0;
//            EditorGUI.LabelField(tempPos, new GUIContent("Base"));
//            tempPos.x += tempPos.width;
//            EditorGUI.LabelField(tempPos, new GUIContent("Gain"));
//            position.y += position.height;
//            EditorGUI.indentLevel = indent;
//            return position;
//        }
//
//        [Obsolete]
//        public static Rect DisplayBaseRowGrid(Rect position, SerializedProperty baseVal, SerializedProperty gainVal,
//            GUIContent label, bool allowEmpty = false)
//        {
//            var indent = EditorGUI.indentLevel;
//            if (!allowEmpty && (label == GUIContent.none || label.text == ""))
//                label = new GUIContent(" "); //Forces the label to be present; aligns the labels below
//
//            var tempPos = EditorGUI.PrefixLabel(position, label);
//            tempPos.width /= 2f;
//            EditorGUI.indentLevel = 0;
//            EditorGUI.PropertyField(tempPos, baseVal, GUIContent.none);
//            tempPos.x += tempPos.width;
//            EditorGUI.PropertyField(tempPos, gainVal, GUIContent.none);
//            position.y += position.height;
//            EditorGUI.indentLevel = indent;
//            return position;
//        }

        public static Rect DisplayProperty(Rect position, SerializedProperty prop, GUIContent label)
        {
            var indent = EditorGUI.indentLevel;
            var tempPos = EditorGUI.PrefixLabel(position, label);
            if (label != GUIContent.none && label.text != "") //If label is empty
                EditorGUI.indentLevel = 0;
            EditorGUI.PropertyField(tempPos, prop, GUIContent.none);
            position.y += position.height;
            EditorGUI.indentLevel = indent;
            return position;
        }

        public static Rect DisplayBoldLabel(Rect position, GUIContent guiContent)
        {
            EditorGUI.LabelField(position, guiContent, EditorStyles.boldLabel);
            position.y += position.height;
            return position;
        }
    }
}