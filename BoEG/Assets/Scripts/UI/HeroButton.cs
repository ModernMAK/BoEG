using Entity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class HeroButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {

        [SerializeField] private Text _text;
        [SerializeField] private HeroData _entity;
        private HeroDetails _details;

        public void SetHeroPanel(HeroDetails details)
        {
            _details = details;
        }
        
        public void SetData(HeroData entity)
        {
            _entity = entity;
        }


        private void Update()
        {
            _text.text = _entity.Name;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _details.SetData(_entity);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _details.SetHoverData(_entity);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _details.ClearHoverData();
        }
    }
}