//using Old.Entity.Modules.Magicable;
//using UnityEditor;
//using UnityEngine;
//
//namespace Editor
//{
//    [CustomPropertyDrawer(typeof(MagicableData))]
//    public class MagicableDataDrawer : PropertyDrawer
//    {
//        public const float Scale = 7f;
//        
//        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//        {
//            var height = base.GetPropertyHeight(property, label);
//            if (property.isExpanded)
//                return height * Scale;
//            return height;
//        }
//
//
//        private static readonly string[] PropertyStrings =
//        {
//            "_baseManaCapacity",
//            "_gainManaCapacity",
//            "_baseManaGen",
//            "_gainManaGen",
//        };
//
//        private static SerializedProperty[] GetProperties(SerializedProperty property)
//        {
//            var properties = new SerializedProperty[PropertyStrings.Length];
//            for (var i = 0; i < properties.Length; i++)
//                properties[i] = property.FindPropertyRelative(PropertyStrings[i]);
//            return properties;
//        }
//
//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//        {
//            EditorGUI.BeginProperty(position, label, property);
//            if (property.isExpanded)
//                position.height /= Scale;
//            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true); //1
//            if (property.isExpanded)
//            {
//                EditorGUI.indentLevel++;
//
//                position.y += position.height;
//                var props = GetProperties(property);
//
//                position = DrawerUtil.DisplayBaseGain(position, props[0], props[1], new GUIContent("Capacity")); //3
//                position = DrawerUtil.DisplayBaseGain(position, props[2], props[3], new GUIContent("Generation")); //3
//
//                EditorGUI.indentLevel--;
//            }
//            EditorGUI.EndProperty();
//        }
//    }
//}