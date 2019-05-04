using System.Collections;
using System.Collections.Generic;
using Framework.Core.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ArmorablePanel : MonoBehaviour
    {
        [SerializeField] private Text _physicalBlockText;
        [SerializeField] private Text _physicalResistText;
        [SerializeField] private Text _magicalBlockText;

        [SerializeField] private Text _magicalResistText;

        // Start is called before the first frame update
        [SerializeField] private GameObject _targetGO;

        private IArmorable _target;

        void Start()
        {
            _target = _targetGO.GetComponent<IArmorable>();
        }

        // Update is called once per frame
        void Update()
        {
            float physBlock = 0f;
            float magBlock = 0f;
            float physResist = 0f;
            float magResist = 0f;
            if (_target != null)
            {
                physBlock = _target.Physical.Block;
                physResist = _target.Physical.Resistance * 100f;
                magBlock = _target.Magical.Block;
                magResist = _target.Magical.Resistance * 100f;
            }

            UpdateText(physBlock, _physicalBlockText);
            UpdateText(magBlock, _magicalBlockText);
            UpdateText(physResist, _physicalResistText);
            UpdateText(magResist, _magicalResistText);
        }

        private void UpdateText(float value, Text text)
        {
            text.text = Mathf.FloorToInt(value).ToString();
        }
    }
}