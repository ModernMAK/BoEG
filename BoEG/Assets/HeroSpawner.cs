using System;
using System.Collections.Generic;
using MobaGame.Entity.UnitArchtypes;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame
{
    public interface IRespawnable
    {
        void Respawn();
    }

    public class HeroSpawner : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private float _deathCooldown;

        [SerializeField] private Transform[] _spawnPoints;
#pragma warning restore 0649
        private Transform GetRandomTransform() => _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length)];
        private List<Tuple<Actor, DurationTimer>> _deadTracker;

        private List<Actor> _heroTracker;

        private void Awake()
        {
            _heroTracker = new List<Actor>();
            _deadTracker = new List<Tuple<Actor, DurationTimer>>();

            _notDead = new Queue<Tuple<Actor, DurationTimer>>();
            var heroes = FindObjectsOfType<Hero>();
            _heroTracker.AddRange(heroes);
            foreach (var hero in heroes)
            {
                var healthable = hero.GetComponent<IHealthable>();
                healthable.Died += HealthableOnDied;
            }
        }

        private void HealthableOnDied(object sender, DeathEventArgs e)
        {
            var cd = new DurationTimer(_deathCooldown);
            _deadTracker.Add(new Tuple<Actor, DurationTimer>(e.Self, cd));
        }

        private Queue<Tuple<Actor, DurationTimer>> _notDead;

        // Update is called once per frame
        void Update()
        {
            foreach (var dead in _deadTracker)
            {
                if (dead.Item2.AdvanceTime(Time.deltaTime))
                {
                    _notDead.Enqueue(dead);
                }
            }

            while (_notDead.Count > 0)
            {
                var dead = _notDead.Dequeue();
                _deadTracker.Remove(dead);
                var actor = dead.Item1;
                var respawnables = actor.GetComponents<IRespawnable>();

                foreach (var respawnable in respawnables)
                {
                    respawnable.Respawn();
                }

                var spawn = GetRandomTransform();
                var pos = spawn.position;
                actor.transform.position = pos;
                actor.gameObject.SetActive(true);
                if (actor.TryGetComponent<IMovable>(out var movable))
                {
                    var slightOffset = pos + UnityEngine.Random.onUnitSphere * 0.01f;
                    movable.WarpTo(slightOffset);
                }
            }
        }
    }
}