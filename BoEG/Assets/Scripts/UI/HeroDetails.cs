using Entity;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HeroDetails : MonoBehaviour
    {
        public HeroData Data { get; private set; }
        [SerializeField] private Text _nameText;

        public void SetData(HeroData data)
        {
            Data = data;
        }

        private void Update()
        {
            if (Data == null)
                return;


            _nameText.text = Data.Name;
        }
    }
}