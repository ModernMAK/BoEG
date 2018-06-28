using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HeroPanel : MonoBehaviour
{
    [SerializeField] private HeroList _heroes;
    [SerializeField] private GameObject _heroButton;
    [SerializeField] private Transform _heroGrid;
    [SerializeField] private HeroDetails _heroDetails;

    private void Awake()
    {
        foreach (var hero in _heroes)
        {
            var go = Instantiate(_heroButton);
            var hb = go.GetComponent<HeroButton>();
            var b = go.GetComponent<Button>();
            go.transform.SetParent(_heroGrid);
            hb.SetData(hero);
            var temp = hero;
            UnityAction func = (() => _heroDetails.SetData(temp));
            b.onClick.AddListener(func);
        }
    }
}