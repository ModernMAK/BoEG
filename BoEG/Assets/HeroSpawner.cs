using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Core;
using Framework.Core.Modules;
using Framework.Types;
using UnityEngine;
using Random = System.Random;

public interface IRespawnable
{
    void Respawn();
}

public class HeroSpawner : MonoBehaviour
{
    [SerializeField] private float _deathCooldown;

    [SerializeField] private Transform[] _spawnPoints;
    private Transform GetRandomTransform() => _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length)];
    private List<Tuple<Actor, DurationTimer>> _deadTracker;

    private List<Actor> _heroTracker;

    private void Awake()
    {
        _heroTracker = new List<Actor>();
        _deadTracker = new List<Tuple<Actor, DurationTimer>>();

        _notDead = new Queue<Tuple<Actor, DurationTimer>>();
        var heroes = GameObject.FindObjectsOfType<Hero>();
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
            if(actor.TryGetComponent<IMovable>(out var movable))
                movable.WarpTo(spawn.position);
            actor.transform.position = spawn.transform.position;
            actor.gameObject.SetActive(true);
        }
    }
}