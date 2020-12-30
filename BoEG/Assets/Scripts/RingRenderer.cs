using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Utility;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public static class Vector3Extensions
{
    public static Vector3 Inverse(this Vector3 v) => new Vector3(1f / v.x, 1f / v.y, 1f / v.z);
}

[RequireComponent(typeof(LineRenderer))]
public class RingRenderer : MonoBehaviour
{
    public enum Axis
    {
        X,
        Y,
        Z
    }

    public enum Space
    {
        World,
        Local,
    }

    [SerializeField] private int _initialPoints = 16;
    [SerializeField] private float _initialRadius = 0f;
    [SerializeField] private float _initialWidth = 0.1f;
    [SerializeField] private Color _initialColor = Color.white;

    [SerializeField] private Axis _initialAxis = Axis.Y;
    [SerializeField] private Space _initialSpace = Space.World;

    private bool _prevUseAxis;
    private Axis _prevAxis;
    private Space _prevSpace;
    private float _prevRad;

    [SerializeField] private LineRenderer _renderer;


    private void Awake()
    {
        _renderer = GetComponent<LineRenderer>();
        SetPoints(_initialPoints, false);
        SetRadius(_initialRadius, false);
        SetWidth(_initialWidth); //Doesn't require redraw, so it doesn't ask
        SetColor(_initialColor); //Doesn't require redraw, so it doesn't ask
        SetSpace(_initialSpace, false);
        SetAxis(_initialAxis, false);

        Redraw();
    }

    public void SetWidth(float width) => _renderer.startWidth = _renderer.endWidth = width;
    public void SetColor(Color color) => _renderer.startColor = _renderer.endColor = color;

    Vector3 ApplyAxis(Vector2 pos) //Assumes XY
    {
        return _prevAxis switch
        {
            Axis.X => new Vector3(0, pos.x, pos.y),
            Axis.Y => new Vector3(pos.x, 0, pos.y),
            Axis.Z => new Vector3(pos.x, pos.y, 0),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    Vector3 ApplySpace(Vector3 pos) //Assumes XY
    {
        switch (_prevSpace)
        {
            case Space.Local:
                return pos;
            case Space.World:
                pos = Quaternion.Inverse(transform.rotation) * pos;
                return pos;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


    public void Redraw()
    {
        var radius = _prevRad;
        var count = _renderer.positionCount;
        for (var i = 0; i < count; i++)
        {
            var theta = (float) i / count * Mathf.PI * 2f;
            var x = Mathf.Cos(theta);
            var z = Mathf.Sin(theta);
            var circlePos = new Vector2(x, z) * radius;
            var finalPos = ApplyAxis(circlePos);
            finalPos = ApplySpace(finalPos);
            finalPos = Vector3.Scale(finalPos, transform.lossyScale.Inverse());
            _renderer.SetPosition(i, finalPos);
        }
    }

    public void SetRadius(float radius, bool redraw = false)
    {
        if (radius.SafeEquals(_prevRad))
            return;
        _prevRad = radius;
        if (redraw)
            Redraw();
    }

    public void SetPoints(int points, bool redraw = false)
    {
        if (points == _renderer.positionCount)
            return;
        _renderer.positionCount = points;
        if (redraw)
            Redraw();
    }

    public void SetAxis(Axis axis, bool redraw = false)
    {
        _prevUseAxis = true;
        _prevAxis = axis;
        if (redraw)
            Redraw();
    }

    public void SetSpace(Space space, bool redraw = false)
    {
        _prevSpace = space;
        if (redraw)
            Redraw();
    }
}