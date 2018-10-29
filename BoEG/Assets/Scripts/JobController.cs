using System;
using Core.OrderSystem;
using Core.OrderSystem.Order;
using Entity;
using UnityEngine;

public enum JobControllerMode
{
    Move = 0,
    AttackMove = 1
}

public class JobControllerUtil
{
    static Texture2D _whiteTexture;

    public static Texture2D WhiteTexture
    {
        get
        {
            if (_whiteTexture == null)
            {
                _whiteTexture = new Texture2D(1, 1);
                _whiteTexture.SetPixel(0, 0, Color.white);
                _whiteTexture.Apply();
            }

            return _whiteTexture;
        }
    }

    public static void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, WhiteTexture);
        GUI.color = Color.white;
    }

    public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        // Top
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Left
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Right
        DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        // Bottom
        DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

    public static Bounds GetViewportBounds(Camera camera, Vector3 screenPosition1, Vector3 screenPosition2)
    {
        var v1 = Camera.main.ScreenToViewportPoint(screenPosition1);
        var v2 = Camera.main.ScreenToViewportPoint(screenPosition2);
        var min = Vector3.Min(v1, v2);
        var max = Vector3.Max(v1, v2);
        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;

        var bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }

    public static bool IsWithinSelectionBounds(Vector3 mouse1, Vector3 mouse2, GameObject gameObject)
    {
        var camera = Camera.main;
        var viewportBounds =
            GetViewportBounds(camera, mouse1, mouse2);

        return viewportBounds.Contains(
            camera.WorldToViewportPoint(gameObject.transform.position));
    }

    public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        // Move origin from bottom left to top left
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        // Calculate corners
        var topLeft = Vector3.Min(screenPosition1, screenPosition2);
        var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
        // Create Rect
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }
}

public class JobController : MonoBehaviour
{
    private IJobSystem _jobs;

    [SerializeField] private JobControllerMode _mode;
    private Vector3 _startDrag;
    private bool _isSelected;

    void OnGUI()
    {
        if (_isSelected)
        {
            // Create a rect from both mouse positions
            var rect = JobControllerUtil.GetScreenRect(_startDrag, Input.mousePosition);
            JobControllerUtil.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            JobControllerUtil.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isSelected = true;
            _startDrag = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0) && _isSelected)
        {
            _isSelected = false;
            _jobs = null;
            foreach (var entity in EntityRegistry.Registered)
            {
                var endDrag = Input.mousePosition;
                if (!JobControllerUtil.IsWithinSelectionBounds(_startDrag, endDrag, entity))
                    continue;
                _jobs = entity.GetComponent<IJobSystem>();
                if (_jobs != null)
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
            _mode = JobControllerMode.AttackMove;
        else if (Input.GetKeyDown(KeyCode.S))
            _mode = JobControllerMode.Move;

        if (_jobs != null && Input.GetMouseButtonDown(1))
        {
            ApplyJob();
        }
    }

    void ApplyMoveJob()
    {
        RaycastHit hitinfo;
        if (Physics.Raycast(ScreenRay, out hitinfo))
        {
            _jobs.SetJob(new MoveToJob(hitinfo.point));
        }
    }

    void ApplyAttackMoveJob()
    {
        RaycastHit hitinfo;
        if (Physics.Raycast(ScreenRay, out hitinfo))
        {
            _jobs.SetJob(new AttackMoveToJob(hitinfo.point));
        }
    }

    void ApplyJob()
    {
        switch (_mode)
        {
            case JobControllerMode.Move:
                ApplyMoveJob();
                break;
            case JobControllerMode.AttackMove:
                ApplyAttackMoveJob();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    Ray ScreenRay
    {
        get { return Camera.main.ScreenPointToRay(Input.mousePosition); }
    }
}