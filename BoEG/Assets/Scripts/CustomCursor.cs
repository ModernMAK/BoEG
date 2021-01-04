using System;
using System.Collections;
using System.Collections.Generic;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Types;
using UnityEngine;

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
#pragma warning disable 0649
        // [SerializeField] private CursorPack _cursor;
        [SerializeField] private Texture2D _default;

        [SerializeField] private Texture2D _hoverUnit;
#pragma warning restore 0649

        // ReSharper disable twice PossibleLossOfFraction
        private Vector2 GetCenter(Texture tex) => new Vector2(tex.width / 2, tex.height / 2);

        public const float MaxDistance = 128;

        private Texture2D _prev;

        private void Awake()
        {
            SetCursor(_default, CursorMode.Auto);
            _prev = _default;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private const int Mask = (int) (LayerMaskHelper.Entity | LayerMaskHelper.World);

        private void FixedUpdate()
        {
            var desired = _default;
            var ray = AbilityHelper.GetScreenRay();
            if (Physics.Raycast(ray, out var hit, MaxDistance, Mask))
            {
                if (AbilityHelper.TryGetActor(hit.collider, out _))
                    desired = _hoverUnit;
            }

            //We cant set the cursor every frame, because it doesnt move when you do
            if (desired != _prev)
            {
                SetCursor(desired, CursorMode.Auto);
                _prev = desired;
            }
        }

        private void SetCursor(Texture2D sprite, CursorMode mode)
        {
            Cursor.SetCursor(sprite, GetCenter(sprite), mode);
        }
    }
}