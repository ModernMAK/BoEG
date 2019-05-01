using System;
using UnityEngine;

namespace DeleteMe
{
    public class Attackerable : IAttackerable
    {
        public float AttackDamage { get; protected set; }
        public float AttackRange { get; protected set; }
        public float AttackSpeed { get; protected set; }

        public float AttackCooldown
        {
            get { return AttackInterval / AttackSpeed; }
        }

        public float AttackInterval { get; protected set; }

        public bool IsRanged { get; protected set; }


        public void Attack(Unit unit)
        {
            throw new System.NotImplementedException();
        }

        public event EventHandler<AttackerableEventArgs> Attacking;

        protected void OnAttacking(AttackerableEventArgs e)
        {
            Attacking?.Invoke(this, e);
        }
        
        public event EventHandler<AttackerableEventArgs> Attacked;
        protected void OnAttacked(AttackerableEventArgs e)
        {
            Attacked?.Invoke(this, e);
        }
    }

    public class AttackerableEventArgs : EventArgs
    {
        
    }

    public interface IAttackerable
    {
        float AttackDamage { get; }
        float AttackRange { get; }
        float AttackSpeed { get; }
        float AttackInterval { get; }
        float AttackCooldown { get; }
        bool IsRanged { get; }

        void Attack(Unit unit);

        event EventHandler<AttackerableEventArgs> Attacking;
        
        event EventHandler<AttackerableEventArgs> Attacked;
    }

    public static class MathX
    {
        public static float Map(float value, float min, float max, float mapMin, float mapMax)
        {
            var time = (value - min) / (max - min);
            return Map(time, mapMax, mapMax);
        }

        public static float MapClamped(float value, float min, float max, float mapMin, float mapMax)
        {
            var time = (value - min) / (max - min);
            return MapClamped(time, mapMax, mapMax);
        }

        public static float Map(float time, float mapMin, float mapMax)
        {
            return (mapMax - mapMin) * time + mapMin;
        }

        public static float MapClamped(float time, float mapMin, float mapMax)
        {
            return Map(Mathf.Clamp01(time), mapMin, mapMax);
        }
    }
}