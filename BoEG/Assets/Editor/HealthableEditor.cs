//using Components;
//using UnityEditor;
//using UnityEngine;
//
//[CustomEditor(typeof(Healthable))]
//public class HealthableEditor : Editor
//{
//        private SerializedProperty 
//            _healthRatio,
//            _healthData,
//            _baseHealthCapacity,
//            _baseHealthGen,
//            _gainHealthCapacity,
//            _gainHealthGen;
//
//        private static int tab = 0;
//        private static readonly string[] tabs = new[] {"No Filter", "Core", "Capacity", "Generation"};
//
//        void OnEnable()
//        {
//            _healthRatio = serializedObject.FindProperty("_healthRatio");
//            _healthData = serializedObject.FindProperty("_data");
////            _baseHealthCapacity = serializedObject.FindProperty("_baseHealthCapacity");
////            _baseHealthGen = serializedObject.FindProperty("_baseHealthGen");
////            _gainHealthCapacity = serializedObject.FindProperty("_gainHealthCapacity");
////            _gainHealthGen = serializedObject.FindProperty("_gainHealthGen");
//        }
//
//        public override void OnInspectorGUI()
//        {
////            tab = GUILayout.Toolbar(tab, tabs);
//            serializedObject.Update();
//            switch (tab)
//            {
//                case 0:
//                    EditorGUILayout.PropertyField(_healthRatio);
//                    EditorGUILayout.PropertyField(_healthData,true);
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
////        public float HealthGeneration
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