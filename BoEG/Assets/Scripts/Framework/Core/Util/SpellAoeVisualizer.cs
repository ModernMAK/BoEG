using UnityEngine;

namespace MobaGame.Framework.Core.Util
{
    public class SpellAoeVisualizer : MonoBehaviour
    {
        public void SetAoeSize(float radius)
        {
            transform.localScale = Vector3.one * radius;
        }

        public void SetPoint(Vector3 point)
        {
            transform.position = point + Vector3.up / 100f;
        }
    }
}