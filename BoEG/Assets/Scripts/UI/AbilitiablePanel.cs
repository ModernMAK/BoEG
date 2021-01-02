using System.Collections.Generic;
using Framework.Ability;
using UI;
using UnityEngine;

public class AbilitiablePanel : DebugUI
{
#pragma warning disable 0649
    [SerializeField] private GameObject _abilityPanelPrefab;
    [SerializeField] private Transform _container;
    private IAbilitiable _abilitiable;
    private List<AbilityPanel> _abilityPanels;
    private GameObject _go;
#pragma warning restore 0649
    
    private void Awake()
    {
        _abilityPanels = new List<AbilityPanel>();
    }

    // Update is called once per frame
    private void Update()
    {
        var abilityCount = _abilitiable == null ? 0 : _abilitiable.AbilityCount;
        if (_abilityPanels.Count != abilityCount)
        {
            for (var i = _abilityPanels.Count; i < abilityCount; i++)
            {
                var inst = Instantiate(_abilityPanelPrefab, _container, false);
                inst.SetActive(true);
                _abilityPanels.Add(inst.GetComponent<AbilityPanel>());
            }

            for (var i = abilityCount; i < _abilityPanels.Count; i++)
            {
                Destroy(_abilityPanels[i].gameObject);
                _abilityPanels.RemoveAt(i);
                i--;
            }

            if (_abilitiable != null)
                for (var i = 0; i < _abilityPanels.Count; i++)
                {
                    //BUG aspect ratio not resizing icon?
                    //Toggling on and off fixes this, but its not a good solution imo
                    _abilityPanels[i].gameObject.SetActive(false);
                    _abilityPanels[i].SetAbility(_abilitiable.GetAbility(i));
                    _abilityPanels[i].gameObject.SetActive(true);
                }
        }
    }
    public override void SetTarget(GameObject go)
    {
        _go = go;
        _abilitiable = _go != null ? _go.GetComponent<IAbilitiable>() : null;
    }
}