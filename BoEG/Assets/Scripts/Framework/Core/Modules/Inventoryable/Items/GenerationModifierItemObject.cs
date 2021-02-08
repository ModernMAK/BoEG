using UnityEngine;
namespace MobaGame.Framework.Core.Modules
{

	[CreateAssetMenu(menuName = "Item/GenModItem")]
    public class GenerationModifierItemObject : ItemObject, IHealthGenerationModifier, IMagicGenerationModifier, IListener<Actor>, IListener<IModifiable>
    {
        [SerializeField] private Sprite _icon;
        [SerializeField]
        private FloatModifier _healthGen;
        [SerializeField]
        private FloatModifier _manaGen;

        private GenerationModifierItem _item;
        private GenerationModifierItem Item => _item ??= new GenerationModifierItem(_icon,_healthGen, _manaGen);

        public FloatModifier HealthGeneration => Item.HealthGeneration;

        public FloatModifier MagicGeneration => Item.MagicGeneration;
        public void Register(Actor source) => Item.Register(source);

        public void Unregister(Actor source)=> Item.Unregister(source);

        public void Register(IModifiable source)=> Item.Register(source);

        public void Unregister(IModifiable source)=> Item.Unregister(source);
        public IModifierView View => Item.View;
    }
}