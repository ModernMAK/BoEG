using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UI;

public class HeroDetails : MonoBehaviour
{
    [SerializeField] private HeroData _data;
    [SerializeField] private Text _text;

    public void SetData(HeroData data)
    {
        _data = data;
    }

    private void Update()
    {
        if (_data == null)
            return;


        _text.text = _data.Name;
    }
}