using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Networking.Tracking;
using UnityEngine;

namespace MobaGame.Entity.UnitArchtypes
{
    [RequireComponent(typeof(NetworkId))]
    [RequireComponent(typeof(NetworkSerializer))]
    [RequireComponent(typeof(NetworkedHealthable))]
    public class NetworkedUnit : Unit
    {
    }

}