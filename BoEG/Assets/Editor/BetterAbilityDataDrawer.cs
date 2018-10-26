using UnityEditor;
using UnityEngine;

namespace EditorSpace
{
    public abstract class BetterAbilityDataDrawer : AbilityDataDrawer
    {
        protected abstract string[] SubPropertyPaths { get; }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
            var subProperties = GetProperties(property, SubPropertyPaths);

            if (subProperties[0].arraySize == 0)
            {
                if (ShowAddButton(subProperties[0], new GUIContent(property.displayName)))
                {
                    Resize(subProperties[0], subProperties);
                    property.isExpanded = true;
                }
            }
            else
            {
                var str = property.displayName + " (" + (subProperties[0].arraySize);
                if (subProperties[0].arraySize == 1)
                    str += " Level)";
                else
                    str += " Levels)";
                if (DrawFoldout(property, new GUIContent(str)))
                {
                    EditorGUI.indentLevel++;
                    foreach (var sub in subProperties)
                    {
                        var size = sub.arraySize;
                        DrawArray(sub, subProperties);
                        if (size != sub.arraySize)
                            Resize(sub, subProperties);
                    }

                    EditorGUI.indentLevel--;
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var subProperties = GetProperties(property, SubPropertyPaths);

            var height = GetFoldoutHeight(property);
            if (subProperties[0].arraySize == 0)
            {
                height += (GetAddButtonHeight(subProperties[0]));
            }
            else
            {
                if (property.isExpanded)
                    foreach (var sub in subProperties)
                        height += GetArrayHeight(sub);
            }

            return height;
        }
    }
}