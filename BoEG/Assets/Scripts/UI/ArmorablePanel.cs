using Framework.Core;
using MobaGame.Framework.Core.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace MobaGame.UI
{
    public class ArmorablePanel : DebugUI
    {
        private IArmorable _armorable;

        // Start is called before the first frame update
        private GameObject _go;

        public override void SetTarget(GameObject go)
        {
            _go = go;
            _armorable = _go != null ? _go.GetModule<IArmorable>() : null;
        }

        // Update is called once per frame
        private void Update()
        {
            var physBlock = 0f;
            var magBlock = 0f;
            var physResist = 0f;
            var magResist = 0f;
            if (_armorable != null)
            {
                physBlock = _armorable.Physical.Block.Total;
                physResist = _armorable.Physical.Resistance.Total * 100f;
                magBlock = _armorable.Magical.Block.Total;
                magResist = _armorable.Magical.Resistance.Total * 100f;
            }

            UpdateText(physBlock, _physicalBlockText);
            UpdateText(magBlock, _magicalBlockText);
            UpdateText(physResist, _physicalResistText);
            UpdateText(magResist, _magicalResistText);
        }

#pragma warning disable 649
        [SerializeField] private Text _physicalBlockText;
        [SerializeField] private Text _physicalResistText;
        [SerializeField] private Text _magicalBlockText;

        [SerializeField] private Text _magicalResistText;
#pragma warning restore 649
    }
}