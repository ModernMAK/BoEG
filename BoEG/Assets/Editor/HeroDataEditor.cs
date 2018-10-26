//using System;
//using System.Collections.Generic;
//using Core;
//using UnityEditor;
//using UnityEngine;
//
//namespace Editor
//{
//    [CustomEditor(typeof(HeroData))]
//    public class HeroDataEditor : UnityEditor.Editor
//    {
//        /*
//        
//        [SerializeField] private AbilitiableData _abilityableData;
//        [SerializeField] private ArmorableData _armorableData;
//        [SerializeField] private AttackerableData _attackerableData;
//        [SerializeField] private HealthableData _healthableData;
//        [SerializeField] private LevelableData _levelableData;
//        [SerializeField] private MagicableData _magicableData;
//        [SerializeField] private MovableData _movableData;
//        */
//        private static readonly string[] PropertyStrings =
//        {
//            //Abilitiable
//            "_abilityableData",
//            //Armorable
//            "_armorableData",
//            //Attackerable
//            "_attackerableData",
//            //Bufable
//            "",
//            //Controllable
//            "",
//            //Healthable
//            "_healthableData",
//            //Levelable
//            "_levelableData",
//            //Magicable
//            "_magicableData",
//            //Movable
//            "_movableData",
//            //Teamable
//            "",
//            //Visable
//            ""
//        };
//
//        private SerializedProperty[] _properties;
//
//
//        void OnEnable()
//        {
//            _properties = EntityDataUtility.GetProperties(serializedObject, PropertyStrings);
//        }
//
//        private static int _tab = 0;
//
//        public override void OnInspectorGUI()
//        {
//            _tab = EntityDataUtility.Toolbar(_tab, _properties);
//            EntityDataUtility.Display(_tab, _properties);
//        }
//    }
//
//    public static class EntityDataUtility
//    {
//        private static readonly string[] ToolbarStrings =
//        {
//            "Default", "Ability", "Armor", "Atk", "Buff", "CNTRL", "HP", "LVL",
//            "MP", "Move", "TEAM", "VIS"
//        };
//
//        private static string[] GetToolbarStrings(SerializedProperty[] active)
//        {
//            if (active.Length != (ToolbarStrings.Length - 1))
//                throw new ArgumentException(
//                    string.Format("Active ({0} does not match Requested ({1})!", active.Length, ToolbarStrings.Length),
//                    "active");
//            //Adds "Default to the list
//            var tbs = new List<string> {ToolbarStrings[0]};
//            for (var i = 0; i < ToolbarStrings.Length - 1; i++)
//                if (active[i] != null)
//                    tbs.Add(ToolbarStrings[i + 1]);
//            return tbs.ToArray();
//        }
//
//        private static SerializedProperty[] FilterActive(SerializedProperty[] active)
//        {
//            var tbs = new List<SerializedProperty>();
//            for (var i = 0; i < active.Length; i++)
//                if (active[i] != null)
//                    tbs.Add(active[i]);
//            return tbs.ToArray();
//        }
//
//        public static int Toolbar(int tab, SerializedProperty[] active)
//        {
//            return GUILayout.Toolbar(tab, GetToolbarStrings(active));
//        }
//
//        public static bool Display(int tab, SerializedProperty[] active)
//        {
//            switch (tab)
//            {
//                case 0:
//                    return false;
//                default:
//                    return EditorGUILayout.PropertyField(FilterActive(active)[tab - 1], true);
//            }
//        }
//
//        public static bool Display(int tab, SerializedProperty def, SerializedProperty[] active)
//        {
//            switch (tab)
//            {
//                case 0:
//                    return EditorGUILayout.PropertyField(def, true);
//                default:
//                    return EditorGUILayout.PropertyField(active[tab - 1], true);
//            }
//        }
//
//
//        public static SerializedProperty[] ToArray(SerializedProperty abilityable = null,
//            SerializedProperty armorable = null, SerializedProperty attackerable = null,
//            SerializedProperty buffable = null, SerializedProperty controllable = null,
//            SerializedProperty healthable = null, SerializedProperty levelable = null,
//            SerializedProperty magicable = null, SerializedProperty movable = null,
//            SerializedProperty teamable = null, SerializedProperty visable = null)
//        {
//            return new[]
//            {
//                abilityable, armorable, attackerable, buffable, controllable,
//                healthable, levelable, magicable, movable, teamable, visable
//            };
//        }
//
//        public static SerializedProperty[] GetProperties(SerializedObject property, string[] strings)
//        {
//            var properties = new SerializedProperty[strings.Length];
//            for (var i = 0; i < properties.Length; i++)
//                if (strings[i] == "")
//                    properties[i] = null;
//                else
//                    properties[i] = property.FindProperty(strings[i]);
//            return properties;
//        }
//
//        public static SerializedProperty[] GetProperties(SerializedProperty property, string[] strings)
//        {
//            var properties = new SerializedProperty[strings.Length];
//            for (var i = 0; i < properties.Length; i++)
//                if (strings[i] == "")
//                    properties[i] = null;
//                else
//                    properties[i] = property.FindPropertyRelative(strings[i]);
//            return properties;
//        }
//    }
//}