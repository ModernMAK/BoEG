//using UnityEngine;
//using UnityEngine.Networking;
//using UnityStandardAssets.Characters.FirstPerson;
//
//public class CameraFollowScript : NetworkBehaviour
//{
//    private Camera m_Camera;
//
//    [SerializeField] private MouseLook m_MouseLook;
//
//    private bool LockCamera
//    {
//        get { return !m_MouseLook.lockCursor; }
//        set { m_MouseLook.lockCursor = !value; }
//    }
//
//    private void Awake()
//    {
//        m_Camera = GetComponent<Camera>();
//    }
//
//    void Start()
//    {
//        if (!isClient)
//            return;
//        m_MouseLook.Init(transform, m_Camera.transform);
//    }
//
//    void Update()
//    {
//        if (!isClient)
//            return;
//        if (Input.GetMouseButtonDown(2))
//            LockCamera = !LockCamera;
//
//        RotateView();
//    }
//
//    void FixedUpdate()
//    {
//        if (!isClient)
//            return;
//        m_MouseLook.UpdateCursorLock();
//    }
//
//    private void RotateView()
//    {
//        if (!isClient)
//            return;
//        m_MouseLook.LookRotation(transform.parent, m_Camera.transform);
//    }
//}