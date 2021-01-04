using UnityEngine;

namespace MobaGame.Framework.Core.Util
{
    public class SpellRangeVisualizer : MonoBehaviour
    {
#pragma warning disable 0649

        private Transform _end;
        private Vector3 _endPoint;
        [SerializeField] private Material _excessRangeMat;
        [SerializeField] private LineRenderer _excessRenderer;
        [SerializeField] private Material _inRangeMat;
        [SerializeField] private LineRenderer _mainRenderer;
        [SerializeField] private Material _outOfRangeMat;

        private float _range;
        private Transform _start;
        private Vector3 _startPoint;
#pragma warning restore 0649

        private Vector3 LineStart => _start != null ? _start.transform.position : _startPoint;

        private Vector3 LineEnd => _end != null ? _end.transform.position : _endPoint;

        private bool HasExcess
        {
            get
            {
                var delta = LineEnd - LineStart;
                return delta.sqrMagnitude > _range * _range;
            }
        }

        private Vector3 ExcessPoint
        {
            get
            {
                if (HasExcess)
                {
                    var delta = (LineEnd - LineStart).normalized * _range;
                    return delta + LineStart;
                }

                return LineEnd;
            }
        }

        private void Awake()
        {
        }


        public void SetRange(float range)
        {
            _range = range;
        }

        public void SetStart(Transform follow)
        {
            _start = follow;
            _startPoint = Vector3.zero;
        }

        public void SetStart(Vector3 point)
        {
            _start = null;
            _startPoint = point;
        }

        public void SetEnd(Transform follow)
        {
            _end = follow;
            _endPoint = Vector3.zero;
        }

        public void SetEnd(Vector3 point)
        {
            _end = null;
            _endPoint = point;
        }

        private void Update()
        {
            _mainRenderer.material = HasExcess ? _outOfRangeMat : _inRangeMat;
            _mainRenderer.SetPositions(new[] {LineStart, ExcessPoint});

            _excessRenderer.material = _excessRangeMat;
            _excessRenderer.SetPositions(new[] {ExcessPoint, LineEnd});
        }
    }
}