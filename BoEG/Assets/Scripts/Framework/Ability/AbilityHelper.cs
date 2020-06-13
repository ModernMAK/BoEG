using System;
using Framework.Core;
using Framework.Core.Modules;
using UnityEngine;

namespace Entity.Abilities.FlameWitch
{
    public static class AbilityHelper
    {
        private static PlayerControls _controls;

        private static Camera _cached;

        private static PlayerControls Controls
        {
            get
            {
                if (_controls == null)
                {
                    _controls = new PlayerControls();
                    _controls.Enable();
                }

                return _controls;
            }
        }

        public static void Initialize()
        {
            var _ = Controls;
        }

        [Obsolete("Use ManaHelper class")]
        public static bool CanSpendMana(IMagicable magicable, float mana)
        {
            if (magicable == null)
                return false;
            return magicable.Magic > mana;
        }

        [Obsolete("Use ManaHelper class")]
        public static void SpendMana(IMagicable magicable, float mana)
        {
            magicable.Magic -= mana;
        }

        public static bool InRange(Transform self, Vector3 target, float range)
        {
            return (self.position - target).sqrMagnitude <= range * range;
        }

        public static Ray GetScreenRay(bool updateCamera = false)
        {
            if (_cached == null || updateCamera)
                _cached = Camera.main;
            Controls.Enable();
            var point = Controls.Cursor.Pos.ReadValue<Vector2>();
            // Debug.Log("GetScreenRay:\t" + point);
            var result = _cached.ScreenPointToRay(point);
            return result;
        }

        public static Actor GetActor(RaycastHit hit)
        {
            var go = hit.collider.gameObject;
            var rb = hit.collider.GetComponent<Rigidbody>();

            if (rb != null)
                go = rb.gameObject;

            var actor = go.GetComponent<Actor>();


            if (actor == null)
                actor = go.GetComponentInChildren<Actor>();

            if (actor == null)
                actor = go.GetComponentInParent<Actor>();

//            if (actor != null)
//                return actor.gameObject;
//            else return null;
            return actor;
        }
    }
}