using UnityEngine;

namespace Util
{
    public class ArenaGameManager : GameManager
    {
        [SerializeField] private LaneWaypointManager _laneWaypointManager;

        public LaneWaypointManager Lanes => _laneWaypointManager;
    }
}