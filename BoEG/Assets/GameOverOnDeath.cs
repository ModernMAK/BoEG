using System;
using System.Collections;
using System.Collections.Generic;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using UnityEngine;


public class GameOverOnDeath : MonoBehaviour
{
    private void Start()
    {
        var actor = GetComponent<Actor>();
        var killable = actor.GetModule<IKillable>();
        killable.Died += KillableOnDied;
    }

    private void KillableOnDied(object sender, DeathEventArgs e)
    {
        //TODO
        Debug.Log("GAME OVER");
    }
}
