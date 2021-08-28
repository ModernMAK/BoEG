using System;
using System.Linq;
using Framework.Core;
using MobaGame.Framework.Types;
//using MobaGame.Input;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules.Ability
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


        [Obsolete]
        public static bool HasAllComponents(GameObject gameObject, params Type[] components)
        {
            return components.All(comp => gameObject.TryGetComponent(comp, out _));
        }

        [Obsolete]
        public static bool HasModule<T>(GameObject gameObject)
        {
            return gameObject.TryGetComponent<T>(out _) || gameObject.TryGetComponent<IProxy<T>>(out _);
        }

        public static bool AllowSpellTargets(Actor actor, bool defaultResult = true) =>
            actor.TryGetModule<ITargetable>(out var targetable) ? targetable.AllowSpellTargets : defaultResult;

        public static bool AllowAttackTargets(Actor actor, bool defaultResult = true) =>
            actor.TryGetModule<ITargetable>(out var targetable) ? targetable.AllowAttackTargets : defaultResult;

        [Obsolete]
        public static bool TrySpendMagic(IStatCostAbilityView abilityView, IMagicable magicable) =>
            magicable.TrySpendMagic(abilityView.Cost);



        public static Actor GetActor(RaycastHit hit) => GetActor(hit.collider);

        public static Actor GetActor(Collider collider)
        {
            return TryGetActor(collider, out var actor) ? actor : null;
        }

        public static bool TryGetActor(RaycastHit hit, out Actor actor) => TryGetActor(hit.collider, out actor);

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
        public static bool TryGetWorldOrEntity(Ray ray, out RaycastHit hit) => Physics.Raycast(ray, out hit,
            DefaultMaxDistance, (int) (LayerMaskFlag.World | LayerMaskFlag.Entity));

        public static bool TryGetEntity(Ray ray, out RaycastHit hit) =>
            Physics.Raycast(ray, out hit, DefaultMaxDistance, (int) (LayerMaskFlag.Entity));

        public static bool TryGetWorld(Ray ray, out RaycastHit hit) =>
            Physics.Raycast(ray, out hit, DefaultMaxDistance, (int) (LayerMaskFlag.World));

        public static Quaternion GetRotation(Vector3 start, Vector3 target) => Quaternion.LookRotation(target - start);

        public static Vector3 GetBoxCenter(Vector3 origin, Vector3 halfSize, Quaternion rotation) =>
            origin + rotation * halfSize;

        public static float GetLineLength(Vector3 start, Vector3 target) => (target - start).magnitude;

        public static Vector3 GetLineBox(Vector3 start, Vector3 target, Vector2 size)
        {
            var z = (target - start).magnitude;
            return new Vector3(size.x, size.y, z);
        }

        public static bool TryRaycastActor(out Actor actor)
        {
            var ray = GetScreenRay();
            if (TryGetEntity(ray, out var hit))
                return TryGetActor(hit, out actor);
            actor = default;
            return false;
        }
    }
}