using System.Collections.Generic;
using MobaGame.Framework.Core.Modules;
using UnityEngine;

namespace MobaGame
{
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
            foreach (var smr in GetComponentsInChildren<SkinnedMeshRenderer>())
                _materials.Add(smr.material);

            _teamable = GetComponent<ITeamable>();
            _teamable.TeamChanged += TeamableOnTeamChanged;
            if (_teamable.Team != null)
                Apply(_teamable.Team.GetTint());
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
}