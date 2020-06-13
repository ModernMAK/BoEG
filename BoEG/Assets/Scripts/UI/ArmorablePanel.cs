using System.Collections;
using System.Collections.Generic;
using Framework.Core.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ArmorablePanel : DebugUI
    {
        
#pragma warning disable 649
        [SerializeField] private Text _physicalBlockText;
        [SerializeField] private Text _physicalResistText;
        [SerializeField] private Text _magicalBlockText;

        [SerializeField] private Text _magicalResistText;
#pragma warning restore 649
        
        // Start is called before the first frame update
        private GameObject _go;
        private IArmorable _armorable;

        public override void SetTarget(GameObject go)
        {
            _go = go;
            _armorable = (_go != null) ? _go.GetComponent<IArmorable>() : null;
        }
        // Update is called once per frame
        void Update()
        {
            float physBlock = 0f;
            float magBlock = 0f;
            float physResist = 0f;
            float magResist = 0f;
            if (_armorable != null)
            {
                physBlock = _armorable.Physical.Block;
                physResist = _armorable.Physical.Resistance * 100f;
                magBlock = _armorable.Magical.Block;
                magResist = _armorable.Magical.Resistance * 100f;
            }

            UpdateText(physBlock, _physicalBlockText);
            UpdateText(magBlock, _magicalBlockText);
            UpdateText(physResist, _physicalResistText);
            UpdateText(magResist, _magicalResistText);
        }

    }
}