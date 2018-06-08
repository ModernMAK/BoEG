using Components.Attackerable;
using Components.Healthable;
using Components.Magicable;
using Components.Movable;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(HealthableData))]
    public class HealthableDataDrawer : PropertyDrawer
    {
        public const float Scale = 7f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = base.GetPropertyHeight(property, label);
            if (property.isExpanded)
                return height * Scale;
            return height;
        }


        private static readonly string[] PropertyStrings =
        {
            "_baseHealthCapacity",
            "_gainHealthCapacity",
            "_baseHealthGen",
            "_gainHealthGen",
        };

        private static SerializedProperty[] GetProperties(SerializedProperty property)
        {
            var properties = new SerializedProperty[PropertyStrings.Length];
            for (var i = 0; i < properties.Length; i++)
                properties[i] = property.FindPropertyRelative(PropertyStrings[i]);
            return properties;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            if (property.isExpanded)
                position.height /= Scale;
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true); //1
            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                position.y += position.height;
                var props = GetProperties(property);

                position = DrawerUtil.DisplayBaseGain(position, props[0], props[1], new GUIContent("Capacity")); //3
                position = DrawerUtil.DisplayBaseGain(position, props[2], props[3], new GUIContent("Generation")); //3

                EditorGUI.indentLevel--;
            }
            EditorGUI.EndProperty();
        }
    }
    [CustomPropertyDrawer(typeof(AttackerableData))]
    public class AttackerableDataDrawer : PropertyDrawer
    {
        public const float Scale = 10f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = base.GetPropertyHeight(property, label);
            if (property.isExpanded)
                return height * Scale;
            return height;
        }


        private static readonly string[] PropertyStrings =
        {
            "_baseDamage",
            "_gainDamage",
            "_baseAttackRange",
            "_gainAttackRange",
            "_baseAttackSpeed",
            "_gainAttackSpeed",
        };

        private static SerializedProperty[] GetProperties(SerializedProperty property)
        {
            var properties = new SerializedProperty[PropertyStrings.Length];
            for (var i = 0; i < properties.Length; i++)
                properties[i] = property.FindPropertyRelative(PropertyStrings[i]);
            return properties;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            if (property.isExpanded)
                position.height /= Scale;
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true); //1
            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                position.y += position.height;
                var props = GetProperties(property);

                position = DrawerUtil.DisplayBaseGain(position, props[0], props[1], new GUIContent("Damage")); //3
                position = DrawerUtil.DisplayBaseGain(position, props[2], props[3], new GUIContent("Attack Range")); //3
                position = DrawerUtil.DisplayBaseGain(position, props[4], props[5], new GUIContent("Attack Speed")); //3

                EditorGUI.indentLevel--;
            }
            EditorGUI.EndProperty();
        }
    }
    
    
    [CustomPropertyDrawer(typeof(MovableData))]
    public class MovableDataDrawer : PropertyDrawer
    {
        public const float Scale = 5f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = base.GetPropertyHeight(property, label);
            if (property.isExpanded)
                return height * Scale;
            return height;
        }


        private static readonly string[] PropertyStrings =
        {
            "_baseMoveSpeed",
            "_baseTurnSpeed",
        };

        private static SerializedProperty[] GetProperties(SerializedProperty property)
        {
            var properties = new SerializedProperty[PropertyStrings.Length];
            for (var i = 0; i < properties.Length; i++)
                properties[i] = property.FindPropertyRelative(PropertyStrings[i]);
            return properties;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            if (property.isExpanded)
                position.height /= Scale;
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true); //1
            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                position.y += position.height;
                var props = GetProperties(property);

                position = DrawerUtil.DisplayBase(position, props[0], new GUIContent("Move Speed")); //2
                position = DrawerUtil.DisplayBase(position, props[1], new GUIContent("Turn Speed")); //2

                EditorGUI.indentLevel--;
            }
            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(HealthableInstance))]
    public class HealthableInstanceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, new GUIContent("Health %"));
            EditorGUI.PropertyField(position, property.FindPropertyRelative("_healthRatio"), GUIContent.none);
            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(MagicableInstance))]
    public class MagicableInstanceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, new GUIContent("Mana %"));
            EditorGUI.PropertyField(position, property.FindPropertyRelative("_manaRatio"), GUIContent.none);
            EditorGUI.EndProperty();
        }
    }
}