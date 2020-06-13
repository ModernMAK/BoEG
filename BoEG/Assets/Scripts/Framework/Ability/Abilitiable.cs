using System;
using System.Collections;
using System.Collections.Generic;
using Entity.Abilities.FlameWitch;
using Framework.Core;
using Framework.Core.Modules;
using UnityEngine;


public interface IAbilitiable
{
    bool FindAbility<T>(out T ability);
    IAbility GetAbility(int index);
    int AbilityCount { get; }
}

public class Abilitiable : MonoBehaviour, IAbilitiable, IInitializable<IReadOnlyList<IAbility>>
{
    private IAbility[] _abilities;

    public bool FindAbility<T>(out T ability)
    {
        foreach (var temp in _abilities)
        {
            if (temp is T result)
            {
                ability = result;
                return true;
            }
        }

        ability = default;
        return false;
    }

    public IAbility GetAbility(int index)
    {
        return _abilities[index];
    }

    public int AbilityCount => _abilities.Length;

    public void Initialize(IReadOnlyList<IAbility> module)
    {
        var self = GetComponent<Actor>();
        _abilities = new IAbility[module.Count];
        for (var i = 0; i < _abilities.Length; i++)
        {
            _abilities[i] = module[i];
        }

        foreach (var ab in _abilities)
        {
            ab.Initialize(self);
        }
    }
}