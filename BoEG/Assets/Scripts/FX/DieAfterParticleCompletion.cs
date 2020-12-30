using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAfterParticleCompletion : MonoBehaviour
{
    [SerializeField] private bool _canDie = true;
    private ParticleSystem _particleSystem;

    public void AllowDeath() => _canDie = true;
    public void PreventDeath() => _canDie = false;

    private void Awake() => _particleSystem = GetComponent<ParticleSystem>();

    private void Update()
    {
        if (_canDie)
            if (_particleSystem != null)
                if (!_particleSystem.IsAlive())
                    Destroy(gameObject);
    }
}