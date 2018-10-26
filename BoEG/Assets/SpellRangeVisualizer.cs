using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellRangeVisualizer : MonoBehaviour
{
    [SerializeField] private LineRenderer _mainRenderer;
    [SerializeField] private LineRenderer _excessRenderer;
    [SerializeField] private Material _inRangeMat;
    [SerializeField] private Material _outOfRangeMat;
    [SerializeField] private Material _excessRangeMat;

    void Awake()
    {
    }

    private float _range;
    private Transform _start;
    private Transform _end;
    private Vector3 _startPoint;
    private Vector3 _endPoint;


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

    private Vector3 LineStart
    {
        get { return _start != null ? _start.transform.position : _startPoint; }
    }

    private Vector3 LineEnd
    {
        get { return _end != null ? _end.transform.position : _endPoint; }
    }

    private bool HasExcess
    {
        get
        {
            var delta = LineEnd - LineStart;
            return (delta.sqrMagnitude > (_range * _range));
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

    private void Update()
    {

        _mainRenderer.material = HasExcess ? _outOfRangeMat : _inRangeMat;
        _mainRenderer.SetPositions(new Vector3[] {LineStart, ExcessPoint});

        _excessRenderer.material = _excessRangeMat;
        _excessRenderer.SetPositions(new Vector3[] {ExcessPoint, LineEnd});
    }
}