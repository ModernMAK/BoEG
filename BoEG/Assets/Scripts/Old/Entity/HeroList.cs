using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Old.Entity
{
    [CreateAssetMenu(fileName = "HeroList.asset", menuName = "Entity/HeroList")]
    public class HeroList : ScriptableObject, IEnumerable<HeroData>
    {
        [SerializeField] private HeroData[] _heroes;

        public HeroData[] Heroes
        {
            get { return _heroes; }
        }

        public IEnumerator<HeroData> GetEnumerator()
        {
            foreach (var hero in _heroes)
                yield return hero;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var hero in _heroes)
                yield return hero;
        }
    }
}