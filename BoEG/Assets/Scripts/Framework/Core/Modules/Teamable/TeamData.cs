using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    [CreateAssetMenu(fileName = "TeamData.asset", menuName = "Module/TeamData")]
    public class TeamData : ScriptableObject
    {
        [SerializeField] private Color _teamTint = Color.white;
        public Color GetTint() => _teamTint;
    }
}