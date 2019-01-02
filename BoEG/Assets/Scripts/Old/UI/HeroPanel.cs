//using System;
//using System.Collections.Generic;
//using Entity;
//using Core;
//using UnityEngine;
//using UnityEngine.AI;
//using UnityEngine.Events;
//using UnityEngine.Networking;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//
//namespace UI
//{
//    public class HeroPanel : MonoBehaviour
//    {
//        [SerializeField] private HeroList _heroes;
//        [SerializeField] private GameObject _heroButton;
//        [SerializeField] private Transform _heroGrid;
//        [SerializeField] private Button _selectButton;
//        [SerializeField] private HeroDetails _heroDetails;
//
////    [SerializeField] private GameObject
//
//        private void Awake()
//        {
//            var sorted = new List<HeroData>(_heroes.Heroes);
//            sorted.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal));
//            foreach (var hero in _heroes)
//            {
//                var go = Instantiate(_heroButton);
//                var hb = go.GetComponent<HeroButton>();
////                var b = go.GetComponent<Button>();
////                var img = go.GetComponent<Image>();
//                go.transform.SetParent(_heroGrid);
//                hb.SetData(hero);
//                hb.SetHeroPanel(_heroDetails);
////                var temp = hero;
////                UnityAction func = (() => _heroDetails.SetData(temp));
////                b.onClick.AddListener(func);
//            }
//
//            _selectButton.onClick.AddListener(SelectButtonClick);
//
////        UnityAction action = (() => );
////        _selectButton.onClick.AddListener();
//        }
//
//        private void SelectButtonClick()
//        {
//            var selected = _heroDetails.Data;
//            if (selected == null)
//                return;
//            var comps = new[] {typeof(NavMeshAgent)};
//            SpawnEntity<Hero>(selected, components:comps);
//        }
//
//        public static void SpawnEntity<T>(IEntityData data, Vector3 position = default(Vector3),
//            Quaternion rotation = default(Quaternion), Transform parent = null, params System.Type[] components)
//            where T : Entity.Entity
//        {
//            var go = new GameObject("Entity", components);
//            var transform = go.transform;
//            transform.parent = parent;
//            transform.position = position;
//            transform.rotation = rotation;
//            var entity = go.AddComponent<T>();
//            entity.SetData(data);
//            
//            NetworkServer.Spawn(go);
//        }
//    }
//}