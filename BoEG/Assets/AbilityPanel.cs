using System.Collections;
using System.Collections.Generic;
using Modules.Abilityable;
using UnityEngine;

public class AbilityPanel : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _prefab;
    private List<GameObject> _icons;

    void Awake()
    {
        _icons = new List<GameObject>();
    }

    void Update()
    {
        if (_target != null)
        {
            Set(_target);
        }
    }

    public void Set(GameObject go)
    {
        var abilitiable = go.GetComponent<IAbilitiable>();
        if (abilitiable != null)
        {
            var counter = 0;
            foreach (var ability in abilitiable.Abilities)
            {
                if (_icons.Count <= counter)
                {
                    var icon = Instantiate(_prefab);
                    icon.transform.SetParent(this.transform);
                    _icons.Add(icon);
                }

                var ab =  ability as BetterAbility;
                if (ab != null)
                {
                    var icon = _icons[counter];
                    var abIcon = icon.GetComponent<AbilityIcon>();
                    abIcon.SetTarget(ab);
                }
                counter++;
            }

            for (var i = _icons.Count - 1; i >= counter; i--)
            {
                Destroy(_icons[i]);
                _icons.RemoveAt(i);
            }
        }

        _target = null;
    }
}