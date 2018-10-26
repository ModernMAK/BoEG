using UnityEditor;
using UnityEngine;

namespace EditorSpace
{
    public static class DrawFancyArray
    {
        public static void DrawArray(Rect position, SerializedProperty property)
        {
            var size = property.arraySize;
            var label = new GUIContent(property.displayName);
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);
            position.y += EditorGUI.GetPropertyHeight(property, label, false);
            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                for (var i = 0; i < size; i++)
                {
                    position = DrawArrayItem(position, property, i);
                }

                EditorGUI.indentLevel--;
            }
        }

        public static float GetArrayHeight(SerializedProperty property)
        {
            var height = 0f;
            var size = property.arraySize;
            var label = new GUIContent(property.displayName);
            height += EditorGUI.GetPropertyHeight(property, label, false);
            if (property.isExpanded)
            {
                for (var i = 0; i < size; i++)
                {
                    height += GetArrayItemHeight(property, i);
                }
            }

            return height;
        }

        private static Rect DrawArrayItem(Rect position, SerializedProperty array, int index)
        {
            var buttonRect = position;
            buttonRect.width = 80f;
            position.width -= buttonRect.width;
            buttonRect.x += position.width;
            SerializedProperty property = array.GetArrayElementAtIndex(index);
            var label = new GUIContent("Level " + (index + 1));
            EditorGUI.PropertyField(position, property, label, true);
            position.y += EditorGUI.GetPropertyHeight(property, label, true);

            ShowButtons(buttonRect, array, index);
            if (array.arraySize == index + 1)
            {
                position = DrawAddButton(position, array, new GUIContent("(Level " + (index + 2) + ")"));
            }

            return position;
        }

        private static float GetArrayItemHeight(SerializedProperty array, int index)
        {
            var height = 0f;
            SerializedProperty property = array.GetArrayElementAtIndex(index);
            var label = new GUIContent("Level " + (index + 1));
            height += EditorGUI.GetPropertyHeight(property, label, true);

            if (array.arraySize == index + 1)
            {
                height += 16f;
            }

            return height;
        }

        private static Rect DrawAddButton(Rect position, SerializedProperty list)
        {
            return DrawAddButton(position, list, new GUIContent(list.displayName));
        }

        private static Rect DrawAddButton(Rect position, SerializedProperty list, GUIContent label)
        {
            var controlPos = EditorGUI.PrefixLabel(position, label);
            EditorGUI.LabelField(controlPos, "");


            if (GUI.Button(controlPos, addButtonContent, EditorStyles.miniButton))
            {
                list.arraySize += 1;
            }

            position.y += 16f;
            return position;
        }


        private static void ShowButtons(Rect position, SerializedProperty list, int index)
        {
            position.width /= 4;
            GUI.enabled = index > 0;
            if (GUI.Button(position, moveUpButtonContent, EditorStyles.miniButtonLeft))
            {
                list.MoveArrayElement(index, index - 1);
            }

            position.x += position.width;
            GUI.enabled = index < list.arraySize - 1;
            if (GUI.Button(position, moveDownButtonContent, EditorStyles.miniButtonMid))
            {
                list.MoveArrayElement(index, index + 1);
            }

            position.x += position.width;
            GUI.enabled = true;
            if (GUI.Button(position, duplicateButtonContent, EditorStyles.miniButtonMid))
            {
                list.InsertArrayElementAtIndex(index);
            }

            position.x += position.width;
            if (GUI.Button(position, deleteButtonContent, EditorStyles.miniButtonRight))
            {
                int oldSize = list.arraySize;
                list.DeleteArrayElementAtIndex(index);
                if (list.arraySize == oldSize)
                {
                    list.DeleteArrayElementAtIndex(index);
                }
            }
        }


        private static GUIContent
            moveDownButtonContent = new GUIContent("\u2193", "move down"),
            moveUpButtonContent = new GUIContent("\u2191", "move up"),
            duplicateButtonContent = new GUIContent("+", "duplicate"),
            deleteButtonContent = new GUIContent("-", "delete"),
            addButtonContent = new GUIContent("+", "add element");
    }
}