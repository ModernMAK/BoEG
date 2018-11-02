using Modules.Abilityable;
using Modules.Abilityable.Ability;
using Modules.MiscEvents;
using Modules.Teamable;
using UnityEngine;
using Util;

namespace Entity.Abilities.DarkHeart
{
    
    [CreateAssetMenu(fileName = "DarkHeart_Necromancy.asset", menuName = "Ability/DarkHeart/Necromancy")]
    public class Necromancy : Ability
    {
        //Necromancy Chance
        //Skeleton Data
        [SerializeField] private GameObject _skeletonPrefab = null;
        [SerializeField][Range(0f,1f)] private float _skeletonSpawnChance = 0.1f;
        private RandomProc _necromancyProc = null;
        private ITeamable _teamable;
        private IMiscEvent _eventable;
        protected override void Initialize()
        {
            _necromancyProc = new GradualProc(_skeletonSpawnChance);
            _eventable = Self.GetComponent<IMiscEvent>();
            _teamable = Self.GetComponent<ITeamable>();
            if (_eventable == null)
                throw new MissingModuleException();
            _eventable.KilledEntity += KilledEntityCallback;
        }
        public override void Terminate()
        {
            _eventable.KilledEntity -= KilledEntityCallback;
        }

        private void KilledEntityCallback(KillEventArgs args)
        {
            //Should we check for errors in the KillEventArgs?
            //No, we should assume it is working and check for errors if something is wrong
            if (_necromancyProc.Proc())
            {
                var location = args.Target.transform.position;

                var spawned = AbiltiyUtility.Spawn(location, _skeletonPrefab, 1);
                foreach (var spawn in spawned)
                {
                    var teamable = spawn.GetComponent<ITeamable>();
                    teamable.SetTeam(_teamable.Team);
                }
            }
        }

    }
}