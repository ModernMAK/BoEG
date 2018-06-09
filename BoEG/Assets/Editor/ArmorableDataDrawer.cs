//using System.Runtime.InteropServices;
//using Components;
//using UnityEditor;
//using UnityEngine;
//

using System;
using Components.Armorable;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(ArmorableData))]
    public class ArmorableDataDrawer : PropertyDrawer
    {
        private const float Scale = 17f + SpaceScale;
        private const float SpaceScale = 0.1f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = base.GetPropertyHeight(property, label);
            if (property.isExpanded)
                return height * Scale;
            return height;
        }

        private static Rect Offset(Rect r, int x, int y)
        {
            return new Rect(r.x + r.width * x, r.y + r.height * y, r.width, r.height);
        }

        private static readonly string[] PropertyStrings =
        {
            "_basePhysicalBlock",
            "_gainPhysicalBlock",
            "_basePhysicalResist",
            "_gainPhysicalResist",
            "_hasPhysicalImmunity",

            "_baseMagicalBlock",
            "_gainMagicalBlock",
            "_baseMagicalResist",
            "_gainMagicalResist",
            "_hasMagicalImmunity",
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

                position = DrawerUtil.DisplayBoldLabel(position, new GUIContent("Physical")); //1
                EditorGUI.indentLevel++;
                position = DrawerUtil.DisplayBaseGain(position, props[0], props[1], new GUIContent("Block")); //3
                position = DrawerUtil.DisplayBaseGain(position, props[2], props[3], new GUIContent("Resist")); //3
                position = DrawerUtil.DisplayProperty(position, props[4], new GUIContent("Immunity")); //1
                EditorGUI.indentLevel--;

                position = DrawerUtil.DrawSpace(position, SpaceScale); //SpaceScale (automatically added to scale)

                position = DrawerUtil.DisplayBoldLabel(position, new GUIContent("Magical")); //1
                EditorGUI.indentLevel++;
                position = DrawerUtil.DisplayBaseGain(position, props[5], props[6],
                    new GUIContent("Magical Block")); //3
                position = DrawerUtil.DisplayBaseGain(position, props[7], props[8],
                    new GUIContent("Magical Resist")); //3
                position = DrawerUtil.DisplayProperty(position, props[9], new GUIContent("Immunity")); //1
                EditorGUI.indentLevel--;

                EditorGUI.indentLevel--;
            }
            EditorGUI.EndProperty();
        }
    }
}
////[CustomPropertyDrawer(typeof(HealthableDataAsset))]
////public class HealthableDataDrawer : PropertyDrawer
////{
////    private SerializedProperty 
////        _script,
////        _baseCapacity,
////        _gainCapacity,
////        _baseGen,
////        _gainGen;
////
////    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
////    {
////        if (!property.hasChildren)
////            return 0f;
////            
////        _baseCapacity = property.FindPropertyRelative("_baseHealthCapacity");
////        _gainCapacity = property.FindPropertyRelative("_gainHealthCapacity");
////        _baseGen = property.FindPropertyRelative("_baseHealthGen");
////        _gainGen = property.FindPropertyRelative("_gainHealthGen");
////
////
////        return
////            EditorGUI.GetPropertyHeight(_baseCapacity) +
////            EditorGUI.GetPropertyHeight(_gainCapacity) +
////            EditorGUI.GetPropertyHeight(_baseGen) +
////            EditorGUI.GetPropertyHeight(_gainGen);
////        
////    }
////
////    private void QuickProperty(ref Rect position, SerializedProperty p)
////    {
////        position.height = EditorGUI.GetPropertyHeight(p);
////        EditorGUI.PropertyField(position, p);
////        position.y += position.height;
////    }
////
////    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
////    {
////        if (!property.hasChildren)
////
////        _script = property.FindPropertyRelative("m_Script");
////        _baseCapacity = property.FindPropertyRelative("_baseHealthCapacity");
////        _gainCapacity = property.FindPropertyRelative("_gainHealthCapacity");
////        _baseGen = property.FindPropertyRelative("_baseHealthGen");
////        _gainGen = property.FindPropertyRelative("_gainHealthGen");
////
////
////        QuickProperty(ref position, _baseCapacity);
////        QuickProperty(ref position, _gainCapacity);
////        QuickProperty(ref position, _baseGen);
////        QuickProperty(ref position, _gainGen);
////
////    }
////}
//
//[CustomEditor(typeof(HealthableDataAsset))]
//public class HealthableDataEditor : Editor
//{
//        private SerializedProperty 
//            _script,
//            _baseCapacity,
//            _gainCapacity,
//            _baseGen,
//            _gainGen;
//
//        private static int tab = 0;
//        private static readonly string[] tabs = new[] {"No Filter", "Core", "Capacity", "Generation"};
//
//        void OnEnable()
//        {
//            _script = serializedObject.FindProperty("m_Script");
//            _baseCapacity = serializedObject.FindProperty("_baseHealthCapacity");
//            _gainCapacity = serializedObject.FindProperty("_gainHealthCapacity");
//            _baseGen = serializedObject.FindProperty("_baseHealthGen");
//            _gainGen = serializedObject.FindProperty("_gainHealthGen");
//        }
//
//        public override void OnInspectorGUI()
//        {
////            tab = GUILayout.Toolbar(tab, tabs);
//            
//            GUI.enabled = false;
//            EditorGUILayout.PropertyField(_script);
//            GUI.enabled = true;
//            serializedObject.Update();
//            switch (tab)
//            {
//                case 0:
//                    EditorGUILayout.PropertyField(_baseCapacity);
//                    EditorGUILayout.PropertyField(_gainCapacity);
//                    EditorGUILayout.PropertyField(_baseGen);
//                    EditorGUILayout.PropertyField(_gainGen);
////                    EditorGUILayout.PropertyField(_baseHealthCapacity);
////                    EditorGUILayout.PropertyField(_gainHealthCapacity);
////                    EditorGUILayout.PropertyField(_baseHealthGen);
////                    EditorGUILayout.PropertyField(_gainHealthGen);
//                    break;
//                default:
//                    EditorGUILayout.LabelField(new GUIContent("Not Implimented"));
//                    break;
//            }
//
//            serializedObject.ApplyModifiedProperties();
//        }
//    
//}
//
////using Module.ComponentModel;
////using Module.Threading;
////using UnityEditor;
////using UnityEngine;
////
//
//
/////*
////using UnityEngine;
////
////namespace Components
////{
////    public class Healthable : BuffLevelComponent, IHealthable
////    {
////
////
////        //CORE
////        private float _healthRatio;
////
////        public float HealthPoints
////        {
////            get { return _healthRatio * HealthCapacity; }
////            set { _healthRatio = value / HealthCapacity; }
////        }
////
////        public float HealthCapacity
////        {
////            get { return _baseHealthCapacity + _gainHealthCapacity * (Level - 1); }
////        }
////
////        public float HealthGen
////        {
////            get { return _baseHealthGen + _gainHealthGen * (Level - 1); }
////        }
////    }
////}
////*/
////namespace Components
////{
////    [CustomEditor(typeof(Healthable))]
////    public class HealthableEditor : Editor
////    {
////        private SerializedProperty _healthRatio,
////            _baseHealthCapacity,
////            _baseHealthGen,
////            _gainHealthCapacity,
////            _gainHealthGen;
////
////        private static int tab = 0;
////        private static readonly string[] tabs = new[] {"No Filter", "Core", "Capacity", "Generation"};
////
////        void OnEnable()
////        {
////            _healthRatio = serializedObject.FindProperty("_healthRatio");
////            _baseHealthCapacity = serializedObject.FindProperty("_baseHealthCapacity");
////            _baseHealthGen = serializedObject.FindProperty("_baseHealthGen");
////            _gainHealthCapacity = serializedObject.FindProperty("_gainHealthCapacity");
////            _gainHealthGen = serializedObject.FindProperty("_gainHealthGen");
////        }
////
////        public override void OnInspectorGUI()
////        {
////            tab = GUILayout.Toolbar(tab, tabs);
////            serializedObject.Update();
////            switch (tab)
////            {
////                case 0:
////                    EditorGUILayout.PropertyField(_healthRatio);
////                    EditorGUILayout.PropertyField(_baseHealthCapacity);
////                    EditorGUILayout.PropertyField(_gainHealthCapacity);
////                    EditorGUILayout.PropertyField(_baseHealthGen);
////                    EditorGUILayout.PropertyField(_gainHealthGen);
////                    break;
////                default:
////                    EditorGUILayout.LabelField(new GUIContent("Not Implimented"));
////                    break;
//////                                   
//////                case 1: //Core
//////                    EditorGUILayout.PropertyField(_healthRatio);
//////                    EditorGUILayout.LabelField(new GUIContent("Health Capacity"),new GUIContent(_baseHealthCapacity.floatValue + " (+" + _gainHealthCapacity.floatValue +"/lvl)"));
//////                    
//////                    EditorGUILayout.LabelField(new GUIContent("Health Generation"),new GUIContent(_baseHealthGen.floatValue + " (+" + _gainHealthGen.floatValue +"/lvl)"));
//////                    
//////                    break;
//////                case 2:
//////                    EditorGUILayout.PropertyField(_baseHealthCapacity,new GUIContent("Base Capacity"));
//////                    EditorGUILayout.PropertyField(_gainHealthCapacity,new GUIContent("Gain Capacity"));
//////                    EditorGUILayout.LabelField(new GUIContent("Health Capacity"),new GUIContent(_baseHealthCapacity.floatValue + " (+" + _gainHealthCapacity.floatValue +"/lvl)"));
//////                    break;
//////                 case 3:
//////                     EditorGUILayout.PropertyField(_baseHealthGen,new GUIContent("Base Generation"));
//////                     EditorGUILayout.PropertyField(_gainHealthGen,new GUIContent("Gain Generation"));
//////                     EditorGUILayout.LabelField(new GUIContent("Health Generation"),new GUIContent(_baseHealthGen.floatValue + " (+" + _gainHealthGen.floatValue +"/lvl)"));
//////                     break;
////            }
////
////            serializedObject.ApplyModifiedProperties();
////        }
////    }
////}