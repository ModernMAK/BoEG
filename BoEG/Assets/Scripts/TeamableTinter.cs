using System;
using System.Collections;
using System.Collections.Generic;
using Modules.Teamable;
using UnityEngine;

[RequireComponent(typeof(ITeamable))]
public class TeamableTinter : MonoBehaviour
{
    private ITeamable _teamable;
    private List<Material> _materials;
    private static readonly int TeamTint = Shader.PropertyToID(ShaderKey);

    private const string ShaderKey = "_TeamTint";

    private void Awake()
    {
        _materials = new List<Material>();
        foreach (var mr in GetComponentsInChildren<MeshRenderer>())
            _materials.Add(mr.material);

        _teamable = GetComponent<ITeamable>();
        _teamable.TeamChanged += TeamableOnTeamChanged;
    }

    private void Apply(Color color)
    {
        foreach (var mat in _materials)
            mat.SetColor(TeamTint, color);
    }

    private void TeamableOnTeamChanged(object sender, TeamData e)
    {
        Apply(e != null ? e.GetTint() : Color.white);
    }
}