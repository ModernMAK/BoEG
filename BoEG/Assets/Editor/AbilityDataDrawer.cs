using UnityEditor;
using UnityEngine;

namespace EditorSpace
{
    public class AbilityDataDrawer : PropertyDrawer
    {
        protected Rect _internalRect;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = 16f;
            _internalRect = position;
        }

        protected SerializedProperty[] GetProperties(SerializedProperty property, string[] relPaths)
        {
            var subProperties = new SerializedProperty[relPaths.Length];
            for (var i = 0; i < subProperties.Length; i++)
                subProperties[i] = property.FindPropertyRelative(relPaths[i]);
            return subProperties;
        }

        protected void Resize(SerializedProperty property, SerializedProperty[] properties)
        {
            foreach (var prop in properties)
            {
                if (prop.isArray)
                    Resize(prop, property.arraySize);
            }
        }

        protected void Resize(SerializedProperty property, int newSize)
        {
            while (property.arraySize > newSize)
            {
                var index = property.arraySize - 1;
                int oldSize = property.arraySize;
                property.DeleteArrayElementAtIndex(index);
                if (property.arraySize == oldSize)
                {
                    property.DeleteArrayElementAtIndex(index);
                }
            }

            property.arraySize = newSize;
        }

        protected float GetLengthHeight(SerializedProperty property)
        {
            return GetDataHeight(property);
        }

        protected void DrawArray(SerializedProperty property, SerializedProperty[] allProperties)
        {
            var size = property.arraySize;
            if (DrawFoldout(property))
            {
                EditorGUI.indentLevel++;
                for (var i = 0; i < size; i++)
                {
                    DrawArrayItem(property, i);
                }

                EditorGUI.indentLevel--;
            }
        }

        const bool showButtons = true;
        const bool showElementLabels = false;

        protected void DrawArrayItem(SerializedProperty array, int index)
        {
            var position = _internalRect;
            var buttonRect = position;
            buttonRect.width = 80f;
            position.width -= buttonRect.width;
            buttonRect.x += position.width;
            SerializedProperty property = array.GetArrayElementAtIndex(index);
            _internalRect = position;
//            if (property.hasChildren)
//            {
            DrawData(property, new GUIContent("Level " + (index + 1)));
//            }
//            else
//            {
//                DrawData(property, GUIContent.none);
//            }
            _internalRect.width += buttonRect.width;

            if (showButtons)
            {
                ShowButtons(buttonRect, array, index);
//                EditorGUILayout.EndHorizontal();
            }

//            if (showButtons && property.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton)) {
            if (showButtons && array.arraySize == index + 1)
            {
                ShowAddButton(array, new GUIContent("(Level " + (index + 2) + ")"));
            }
        }

        protected bool ShowAddButton(SerializedProperty listlabel)
        {
            return ShowAddButton(listlabel, new GUIContent(listlabel.displayName));
        }

        protected bool ShowAddButton(SerializedProperty list, GUIContent label)
        {
            var controlPos = EditorGUI.PrefixLabel(_internalRect, label);
            EditorGUI.LabelField(controlPos, "");

            var updated = false;

            if (GUI.Button(controlPos, addButtonContent, EditorStyles.miniButton))
            {
                list.arraySize += 1;
                updated = true;
            }

            _internalRect.y += GetAddButtonHeight(list);
            return updated;
        }

        protected float GetAddButtonHeight(SerializedProperty list)
        {
            return 16f;
        }

        protected static void ShowButtons(Rect position, SerializedProperty list, int index)
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


        protected float GetArrayHeight(SerializedProperty property)
        {
            var height = GetFoldoutHeight(property);

            var size = property.arraySize;
            if (property.isExpanded)
            {
                for (var i = 0; i < size; i++)
                {
                    height += GetArrayItemHeight(property, i);
                }
            }

            return height;
        }

        protected float GetArrayItemHeight(SerializedProperty array, int index)
        {
            SerializedProperty property = array.GetArrayElementAtIndex(index);
            var height = GetDataHeight(property);
            if (showButtons && array.arraySize == index + 1)
            {
                height += GetAddButtonHeight(array);
            }

            return height;
        }

        protected void DrawData(SerializedProperty property)
        {
//            PropertyDrawerUtil.PrintProperty(property);
            DrawData(property, new GUIContent(property.displayName));
        }

        protected void DrawData(SerializedProperty property, GUIContent label)
        {
//            PropertyDrawerUtil.PrintProperty(property);
            EditorGUI.PropertyField(_internalRect, property, label, true);
            _internalRect.y += EditorGUI.GetPropertyHeight(property, label, true);
        }

        protected bool DrawFoldout(SerializedProperty property, GUIContent content)
        {
//            PropertyDrawerUtil.PrintProperty(property);
            property.isExpanded = EditorGUI.Foldout(_internalRect, property.isExpanded, content);
            _internalRect.y += EditorGUI.GetPropertyHeight(property, false);
            return property.isExpanded;
        }

        protected bool DrawFoldout(SerializedProperty property)
        {
//            PropertyDrawerUtil.PrintProperty(property);
            return DrawFoldout(property, new GUIContent(property.displayName));
        }

        protected float GetFoldoutHeight(SerializedProperty property)
        {
//            PropertyDrawerUtil.PrintProperty(property);
            return EditorGUI.GetPropertyHeight(property, false);
        }

        protected float GetDataHeight(SerializedProperty property)
        {
            return EditorGUI.GetPropertyHeight(property, GUIContent.none, true);
        }
    }
}