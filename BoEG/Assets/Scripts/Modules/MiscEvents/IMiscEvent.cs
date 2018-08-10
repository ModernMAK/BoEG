using UnityEngine;

namespace Modules.MiscEvents
{
    public interface IMiscEvent
    {
        event KilledHandler KilledEntity;
        void Kill(GameObject go);
    }
}