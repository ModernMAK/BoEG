using System.Collections.Generic;
using Framework.Core;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using UnityEngine;

namespace MobaGame
{
    [RequireComponent(typeof(Actor))]
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
        }

        void Start()
        {
            var actor = GetComponent<Actor>();
            _teamable = actor.GetModule<ITeamable>();
            if (_teamable != null)
            {
                _teamable.TeamChanged += TeamableOnChanged;
                if (_teamable.Team != null)
                    Apply(_teamable.Team.GetTint());
            }
        }

        private void Apply(Color color)
        {
            foreach (var mat in _materials)
                mat.SetColor(TeamTint, color);
        }

        private void TeamableOnChanged(object sender, ChangedEventArgs<TeamData> e)
        {
            Apply(e?.After.GetTint() ?? Color.white);
        }
    }
}