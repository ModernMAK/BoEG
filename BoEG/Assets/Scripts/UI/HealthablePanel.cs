using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace MobaGame.UI
{
    public class HealthablePanel : DebugActorUI
    {
        // Start is called before the first frame update
        private Actor _actor;
        private IHealthable _healthable;

        public override void SetTarget(Actor actor)
        {
            _actor = actor;
            _healthable = _actor != null ? _actor.GetModule<IHealthable>() : null;
        }

        // Update is called once per frame
        private void Update()
        {
            var value = 0f;
            var normal = 0f;
            var generation = 0f;
            var capacity = 0f;
            if (_healthable != null)
            {
                value = _healthable.Value;               
                generation = _healthable.Generation.Total;
                capacity = _healthable.Capacity.Total;            
                normal = _healthable.Percentage;
            }
            UpdateText(value, _healthValueText, 1);
            UpdateText(generation, _healthGenerationText, 1);
            UpdateText(capacity, _healthCapacityText, 1);      
            UpdateImageFill(normal, _healthNormalImage, 3);
        }
#pragma warning disable 649
        [SerializeField] private Text _healthValueText;
        [SerializeField] private Text _healthCapacityText;
        [SerializeField] private Text _healthGenerationText;
        [SerializeField] private Image _healthNormalImage;
#pragma warning restore 649
    }
}