using Old.Entity;
using UnityEngine;
using UnityEngine.UI;

namespace Old
{
    public class HeroButton : MonoBehaviour
    {

        [SerializeField] private Text _text;
        [SerializeField] private HeroData _entity;

        public void SetData(HeroData entity)
        {
            _entity = entity;
        }


        private void Update()
        {
            _text.text = _entity.Name;
        }
    }
}