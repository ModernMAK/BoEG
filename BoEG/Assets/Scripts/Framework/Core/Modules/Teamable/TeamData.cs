using UnityEngine;

namespace Modules.Teamable
{
    [CreateAssetMenu(fileName = "TeamData.asset", menuName = "Module/TeamData")]
    public class TeamData : ScriptableObject
    {
        [SerializeField] private Color _teamTint = Color.white;
        public Color GetTint() => _teamTint;
    }
}