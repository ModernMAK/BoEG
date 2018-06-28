using UnityEngine;

namespace Core
{
    public class NamedEntity : Entity
    {
        [SerializeField] private NamedEntityData _data;

        public string Name
        {
            get { return _data.name; }
        }


        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}