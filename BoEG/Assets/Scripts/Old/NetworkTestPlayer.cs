//using UnityEngine;
//using UnityEngine.Networking;
//
//public class NetworkTestPlayer : NetworkBehaviour
//{
//    [SyncVar] private Color _color;
//
//    [SerializeField] [SyncVar] private float _speed = 4f;
//    [SerializeField] private GameObject _camera;
//    private Material _material;
//    [SerializeField] private bool bypassClient;
//
//    void Awake()
//    {
//        var mr = GetComponent<MeshRenderer>();
//        _material = new Material(mr.material);
//    }
//
//    // Use this for initialization
//    void Start()
//    {
//        _camera.SetActive(false);
//        if (!isClient && !bypassClient)
//            return;
//        _camera.SetActive(true);
//        _color = Random.ColorHSV();
//    }
//
//    // Update is called once per frame
//    void Update()
//    {
//        _material.color = _color;
//        if (!isClient && !bypassClient)
//            return;
//        var x = Input.GetAxis("Horizontal");
//        var y = Input.GetAxis("Vertical");
//        var delta = (transform.forward * y + transform.right * x) * _speed;
//        if (delta.sqrMagnitude > _speed * _speed)
//            delta = delta.normalized * _speed;
//
//        transform.Translate(delta, Space.World);
//    }
//}