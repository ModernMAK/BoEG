using System;
using System.Collections;
using System.Collections.Generic;
using Modules.Teamable;
using UnityEngine;

[RequireComponent(typeof(ITeamable))]
public class TeamableTinter : MonoBehaviour
{
    private ITeamable _teamable;
    private Material _material;
    private static readonly int TeamTint = Shader.PropertyToID(ShaderKey);

    private const string ShaderKey = "_TeamTint";

    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;

        _teamable = GetComponent<ITeamable>();
        _teamable.TeamChanged += TeamableOnTeamChanged;
    }

    private void TeamableOnTeamChanged(object sender, TeamData e)
    {
        _material.SetColor(TeamTint, e != null ? e.GetTint() : Color.white);
    }
}