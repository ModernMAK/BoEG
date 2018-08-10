using UnityEngine;

namespace Modules.MiscEvents
{
    public class MiscEvent : IMiscEvent
    {
        public event KilledHandler KilledEntity;

        /// <summary>
        /// This does not 'kill' the unit, but notifies anyone that you have killed the unit
        /// </summary>
        /// <param name="go"></param>
        public void Kill(GameObject go)
        {
            GameObject temp = null;
            OnKilledEntity(new KillEventArgs(temp, go));
        }

        private void OnKilledEntity(KillEventArgs args)
        {
            if (KilledEntity != null)
                KilledEntity(args);
        }
    }
}