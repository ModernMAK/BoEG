using System;
using System.Linq;
using Framework.Core;
using Framework.Core.Modules;
using Modules.Teamable;
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

        public static Func<GameObject, bool> HasAllComponentsPredicate(params Type[] components)
        {
            bool Predicate(GameObject gameObject)
            {
                return HasAllComponents(gameObject, components);
            }

            return Predicate;
        }

        public static bool HasAllComponents(GameObject gameObject, params Type[] components)
        {
            return components.All(comp => gameObject.TryGetComponent(comp, out _));
        }


        public static bool TrySpendMagic(IStatCostAbility ability, IMagicable magicable) =>
            magicable.TrySpendMagic(ability.Cost);

        public static Func<GameObject, bool> InRangePredicate(Transform transform, float range)
        {
            bool Predicate(GameObject gameObject)
            {
                return InRange(gameObject.transform, transform, range);
            }

            return Predicate;
        }

        public static bool InRange(Vector3 a, Vector3 b, float range) =>
            (a - b).sqrMagnitude < range * range;

        public static bool InRange(Transform a, Vector3 b, float range) =>
            InRange(a.position, b, range);

        public static bool InRange(Vector3 a, Transform b, float range) =>
            InRange(a, b.position, range);

        public static bool InRange(Transform a, Transform b, float range) =>
            InRange(a.position, b.position, range);


        public static Func<GameObject, bool> SameTeamPredicate(ITeamable teamable, bool nullValue = false)
        {
            bool Predicate(GameObject gameObject) => SameTeam(teamable, gameObject, out _, nullValue);
            return Predicate;
        }

        public static bool SameTeam(ITeamable teamable, GameObject gameObject, bool nullValue = false) =>
            SameTeam(teamable, gameObject, out _, nullValue);
        public static bool SameTeam(ITeamable teamable, GameObject gameObject, out ITeamable otherTeamable,
            bool nullValue = false)
        {
            return gameObject.TryGetComponent(out otherTeamable)
                ? SameTeam(teamable, otherTeamable, nullValue)
                : nullValue;
        }

        public static bool SameTeam(ITeamable teamable, Component component, bool nullValue = false) =>
            SameTeam(teamable, component, out _, nullValue);
        public static bool SameTeam(ITeamable teamable, Component component, out ITeamable otherTeamable,
            bool nullValue = false) =>
            SameTeam(teamable, component.gameObject, out otherTeamable, nullValue);

        public static bool SameTeam(ITeamable teamable, ITeamable otherTeamable, bool nullValue)
        {
            if (teamable == null)
                return nullValue;
            return teamable.SameTeam(otherTeamable);
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