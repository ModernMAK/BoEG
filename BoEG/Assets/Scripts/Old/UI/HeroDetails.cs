//using Entity;
//using UnityEngine;
//using UnityEngine.UI;
//
//namespace UI
//{
//    public class HeroDetails : MonoBehaviour
//    {
//        public HeroData Data { get; private set; }
//        public HeroData HoverData { get; private set; }
//        [SerializeField] private Text _nameText;
//        [SerializeField] private Image _iconImage;
//
//        public void SetData(HeroData data)
//        {
//            Data = data;
//            ClearHoverData();
//        }
//
//        public void SetHoverData(HeroData data)
//        {
//            HoverData = data;
//        }
//
//        public void ClearHoverData()
//        {
//            HoverData = null;
//        }
//
//        private void Update()
//        {
//            
//            if (HoverData != null)
//                Update(HoverData);
//            else
//                Update(Data);
//        }
//
//        private void Update(HeroData data)
//        {
//
//            var heroName = "???";
//            Sprite heroIcon = null;
//            if (data != null)
//            {
//                heroName = data.Name;
//                heroIcon = data.Icon;
//            }
//
//            _nameText.text = heroName;
//            _iconImage.sprite = heroIcon;
//        }
//    }
//}