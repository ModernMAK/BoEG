using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Developer.PlayTesting
{
    [ExecuteInEditMode]
    public class DisplaySpawner : MonoBehaviour
    {
#pragma warning disable 0649

        [SerializeField] private GameObject _prefab;
        [SerializeField] private bool _spawn;
        [SerializeField] private bool _clear;
#pragma warning restore 0649


        private void Update()
        {
            if (_spawn && _prefab != null)
            {
                _spawn = false;
                SpawnPrefabs();
            }

            if (_clear)
            {
                _clear = false;
                ClearChildren();
            }
        }

        private void ClearChildren()
        {
            foreach (Transform child in transform)
            {
                foreach (Transform subchild in child)
                {
                    DestroyImmediate(subchild.gameObject);
                }
            }
        }

        private void SpawnPrefabs()
        {
            foreach (Transform child in transform)
            {
                foreach (Transform subchild in child)
                {
                    DestroyImmediate(subchild.gameObject);
                }

                var prefab = PrefabUtility.InstantiatePrefab(_prefab) as GameObject;

                prefab.transform.SetParent(child, false);
                prefab.transform.localPosition = Vector3.zero;

//                prefab.transform.localPosition = Vector3.zero;
            }
        }
    }
}