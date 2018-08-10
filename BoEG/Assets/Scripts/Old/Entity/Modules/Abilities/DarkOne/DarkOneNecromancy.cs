using Old.Entity.Core;
using Old.Entity.Modules.Abilityable;
using UnityEngine;

namespace Old.Entity.Modules.Abilities.DarkOne
{
    [CreateAssetMenu(menuName = "Entity/Ability/DarkOne/Necromancy")]
    public class DarkOneNecromancy : Ability
    {
        //Necromancy Chance
        //Skeleton Data
        [SerializeField] private GameObject _skeletonPrefab = null;
        [SerializeField][Range(0f,1f)] private float _skeletonSpawnChance = 0.1f;
        private RandomProc _necromancyProc = null;

        public override void Initialize(GameObject go)
        {
            _necromancyProc = new GradualProc(_skeletonSpawnChance);
            MiscEventable eventable = go.GetComponent<MiscEventable>();
            if (eventable == null)
                throw new MissingModuleException();
            eventable.KilledEntity += KilledEntityCallback;
        }

        private void KilledEntityCallback(KillEventArgs args)
        {
            //Should we check for errors in the KillEventArgs? No, we should assume it is working and check for errors if something is wrong
            if (_necromancyProc.Proc())
            {
                var location = args.Target.transform.position;

                var spawned = AbiltiyUtility.Spawn(location, _skeletonPrefab, 1);
            }
        }

        public override void Trigger()
        {
        }
    }
}