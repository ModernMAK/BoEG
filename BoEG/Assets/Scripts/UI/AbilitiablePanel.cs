using System.Collections.Generic;
using UI;
using UnityEngine;

public class AbilitiablePanel : DebugUI
{
    [SerializeField] private GameObject _abilityPanelPrefab;
    [SerializeField] private Transform _container;
    private IAbilitiable _abilitiable;
    private List<AbilityPanel> _abilityPanels;

    // Start is called before the first frame update
    private GameObject _go;

    public override void SetTarget(GameObject go)
    {
        _go = go;
        _abilitiable = _go != null ? _go.GetComponent<IAbilitiable>() : null;
    }

    private void Awake()
    {
        _abilityPanels = new List<AbilityPanel>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(_abilitiable == null)
            return;
        if (_abilityPanels.Count != _abilitiable.AbilityCount)
        {
            for (var i = _abilityPanels.Count; i < _abilitiable.AbilityCount; i++)
            {
                var inst = Instantiate(_abilityPanelPrefab, _container, false);
                inst.SetActive(true);
                _abilityPanels.Add(inst.GetComponent<AbilityPanel>());
            }

            for (var i = _abilitiable.AbilityCount; i < _abilityPanels.Count; i++)
            {
                Destroy(_abilityPanels[i].gameObject);
                _abilityPanels.RemoveAt(i);
                i--;
            }

            for (var i = 0; i < _abilityPanels.Count; i++)
                _abilityPanels[i].SetAbility(_abilitiable.GetAbility(i));
        }


    }
}