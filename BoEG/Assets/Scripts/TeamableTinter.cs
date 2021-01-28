using System.Collections.Generic;
using Framework.Core;
using MobaGame.Framework.Core.Modules;
using UnityEngine;
using MobaGame.Framework.Core;
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
            Apply(new Color(1,1,1,0));
        }

        void Start()
        {
            
            _teamable = this.GetComponent<Actor>().GetModule<ITeamable>();
            _teamable.TeamChanged += TeamableOnChanged;
            if (_teamable.Team != null)
                Apply(_teamable.Team.GetTint());
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