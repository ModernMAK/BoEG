using System.Collections.Generic;
using MobaGame.Framework.Core;
using UnityEngine;

namespace MobaGame.UI
{
    public class ModifiablePanel : DebugActorUI
    {
#pragma warning disable 0649
        [SerializeField] private GameObject _panelPrefab;
        [SerializeField] private Transform _container;
        private IModifiable _modifiable;
        private List<ModifierPanel> _subPanels;
        private Actor _go;
#pragma warning restore 0649

        private void Awake()
        {
            _subPanels = new List<ModifierPanel>();
        }

        private void UpdatePanels()
        {
            var modifierCount = _modifiable?.Modifiers.Count ?? 0;
            if (_subPanels.Count != modifierCount)
            {
                for (var i = _subPanels.Count; i < modifierCount; i++)
                {
                    var inst = Instantiate(_panelPrefab, _container, false);
                    inst.SetActive(true);
                    _subPanels.Add(inst.GetComponent<ModifierPanel>());
                }

                for (var i = modifierCount; i < _subPanels.Count; i++)
                {
                    Destroy(_subPanels[i].gameObject);
                    _subPanels.RemoveAt(i);
                    i--;
                }
            }

            if (_modifiable != null)
                for (var i = 0; i < _subPanels.Count; i++)
                {
                    //BUG aspect ratio not resizing icon?
                    //Toggling on and off fixes this, but its not a good solution imo
                    _subPanels[i].gameObject.SetActive(false);
                    _subPanels[i].SetTarget(_modifiable.Modifiers[i]);
                    _subPanels[i].gameObject.SetActive(true);
                }
        }


        public override void SetTarget(Actor target)
        {
            if (_modifiable != null)
            {
                _modifiable.OnModifierAdded -= OnModifierListChanged;
                _modifiable.OnModifierRemoved -= OnModifierListChanged;
            }

            _go = target;
            _modifiable = _go != null ? _go.GetModule<IModifiable>() : null;
            UpdatePanels();

            if (_modifiable != null)
            {
                _modifiable.OnModifierAdded += OnModifierListChanged;
                _modifiable.OnModifierRemoved += OnModifierListChanged;
            }
        }

        private void OnModifierListChanged(object sender, IModifier e)
        {
            UpdatePanels();
        }
    }
}