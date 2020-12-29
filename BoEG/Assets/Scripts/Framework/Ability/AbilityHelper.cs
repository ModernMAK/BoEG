using System;
using Framework.Core;
using Framework.Core.Modules;
using Triggers;
using UnityEngine;

namespace Framework.Ability
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
            var _ = Controls; //Initializes controls
        }

        [Obsolete("Use ManaHelper class")]
        public static bool CanSpendMana(this IMagicable magicable, float mana)
        {
            if (magicable == null)
                return false;
            return magicable.Magic > mana;
        }

        [Obsolete("Use ManaHelper class")]
        public static void SpendMana(this IMagicable magicable, float mana)
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

        public static Actor GetActor(RaycastHit hit) => GetActor(hit.collider);

        public static Actor GetActor(Collider collider)
        {
            return TryGetActor(collider, out var actor) ? actor : null;
        }

        public static bool TryGetActor(Collider collider, out Actor actor)
        {
            var go = collider.gameObject;
            if (go.TryGetComponent(out actor))
                return true;

            //This should almost always be present, but who knows
            if (collider.TryGetComponent<Rigidbody>(out var rb))
            {
                if (rb.gameObject.TryGetComponent(out actor))
                    return true;
            }

            //
            // if (actor == null)
            //     actor = go.GetComponentInChildren<Actor>();
            //
            // if (actor == null)
            //     actor = go.GetComponentInParent<Actor>();

//            if (actor != null)
//                return actor.gameObject;
//            else return null;
            // return actor;
            return false;
        }

        private const float DefaultMaxDistance = 100f;

        public static bool TryGetWorldOrEntity(Ray ray, out RaycastHit hit) => Physics.Raycast(ray, out hit,
            DefaultMaxDistance, (int) (LayerMaskHelper.World | LayerMaskHelper.Entity));

        public static bool TryGetEntity(Ray ray, out RaycastHit hit) =>
            Physics.Raycast(ray, out hit, DefaultMaxDistance, (int) (LayerMaskHelper.Entity));

        public static bool TryGetWorld(Ray ray, out RaycastHit hit) =>
            Physics.Raycast(ray, out hit, DefaultMaxDistance, (int) (LayerMaskHelper.World));
    }
}