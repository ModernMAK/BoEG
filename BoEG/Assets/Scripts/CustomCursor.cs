using System;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Types;
using UnityEngine;
using static MobaGame.CursorPack;

namespace MobaGame
{
    [Serializable]
    public class CursorPack : ScriptableObject
    {
        [Serializable]
        public class VariantInfo
        {
            [SerializeField] private Texture2D _default;
            [SerializeField] private Texture2D _hover;
            public Texture2D Default => _default;
            public Texture2D Hover => _hover;
        }

#pragma warning disable 0649
        [SerializeField] private VariantInfo _plain;
        public VariantInfo Plain => _plain;
        [SerializeField] private VariantInfo _enemy;
        public VariantInfo Enemy => _enemy;
        [SerializeField] private VariantInfo _ally;
        public VariantInfo Ally => _ally;
#pragma warning restore 0649
    }

    public class CustomCursor : MonoBehaviour
    {

        public enum CursorState
        {
            Default,
            Attacking,
        }

#pragma warning disable 0649
        // [SerializeField] private CursorPack _cursor;
        [SerializeField] private VariantInfo _default;

        [SerializeField] private VariantInfo _attacking;
#pragma warning restore 0649


        public CursorState Mode { get; set; }

        // ReSharper disable twice PossibleLossOfFraction
        private Vector2 GetCenter(Texture tex) => new Vector2(tex.width / 2, tex.height / 2);

        public const float MaxDistance = 128;

        private Texture2D _prev;

        private void Awake()
        {
            SetCursor(_default.Default, CursorMode.Auto);
            _prev = _default.Default;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private const int Mask = (int)(LayerMaskHelper.Entity | LayerMaskHelper.World);

        Texture2D GetCursorIcon(VariantInfo info, bool isHovering) => isHovering ? info.Hover : info.Default;
        VariantInfo GetVariant()
        {
			switch (Mode)
            {
                case CursorState.Default:
                    return _default;
                case CursorState.Attacking:
                    return _attacking;
                default:
                    throw new Exception();
            }
        }

        private void FixedUpdate()
        {
            var variant = GetVariant();
            var ray = AbilityHelper.GetScreenRay();
            bool isHovering = false;
            if (Physics.Raycast(ray, out var hit, MaxDistance, Mask))
            {
                isHovering = AbilityHelper.TryGetActor(hit.collider, out _);
            }
            var desired = GetCursorIcon(variant, isHovering);
            //We cant set the cursor every frame, because it doesnt move when you do
            if (desired != _prev)
            {
                SetCursor(desired, CursorMode.Auto);
                _prev = desired;
            }
        }

        private void SetCursor(Texture2D sprite, UnityEngine.CursorMode mode)
        {
            Cursor.SetCursor(sprite, GetCenter(sprite), mode);
        }
    }
}