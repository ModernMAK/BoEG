using System;
using System.Collections.Generic;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using UnityEngine;

namespace MobaGame.UI
{
    public class AbilitiablePanel : DebugActorUI
    {
#pragma warning disable 0649
        [SerializeField] private GameObject _abilityPanelPrefab;
        [SerializeField] private Transform _container;
        private IAbilitiable _abilitiable;
        private List<AbilityPanel> _abilityPanels;
        private Actor _go;
#pragma warning restore 0649

        private void Awake()
        {
            _abilityPanels = new List<AbilityPanel>();
        }

        private void UpdatePanels()
        {
            var abilityCount = _abilitiable?.Abilities.Count ?? 0;
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
                    _abilityPanels[i].Dispose();
                    Destroy(_abilityPanels[i].gameObject);
                    _abilityPanels.RemoveAt(i);
                    i--;
                }
            }

            if (_abilitiable != null)
                for (var i = 0; i < _abilityPanels.Count; i++)
                {
                    //BUG aspect ratio not resizing icon?
                    //Toggling on and off fixes this, but its not a good solution imo
                    _abilityPanels[i].gameObject.SetActive(false);
                    _abilityPanels[i].SetTarget(_abilitiable.Abilities[i]);
                    _abilityPanels[i].gameObject.SetActive(true);
                }
        }


        public override void SetTarget(Actor target)
        {
            if (_abilitiable != null)
                _abilitiable.AbilitiesChanged -= OnAbilitiesChanged;

            _go = target;
            _abilitiable = _go != null ? _go.GetModule<IAbilitiable>() : null;
            UpdatePanels();

            if (_abilitiable != null)
                _abilitiable.AbilitiesChanged += OnAbilitiesChanged;
        }

        private void OnAbilitiesChanged(object sender, EventArgs e)
        {
            UpdatePanels();
        }
    }
}