//using System.Collections;
//using System.Collections.Generic;
//using Core.OrderSystem;
//using Core.OrderSystem.Order;
//using Entity;
//using Modules.Abilityable;
//using Triggers;
//using UnityEngine;
//
//[RequireComponent(typeof(Hero))]
//public class HeroController : MonoBehaviour
//{
//    private IJobSystem _jobSystem;
//    private IAbilitiable _abilitiable;
//
//    private void Awake()
//    {
//        _abilitiable = GetComponent<IAbilitiable>();
//        _jobSystem = GetComponent<IJobSystem>();
//    }
//
//    private void AddOrSetJob(IJob job, bool enqueue)
//    {
//        if (enqueue)
//            _jobSystem.AddJob(job);
//        else
//            _jobSystem.SetJob(job);
//    }
//
//    private void CastOrLevelUp(int index, bool levelUp)
//    {
//        if (levelUp)
//            _abilitiable.LevelUp(index);
//        else
//            _abilitiable.Cast(index);
//    }
//
//    public void MoveTo(Vector3 destenation, bool enqueue = false)
//    {
//        var job = new MoveToJob(destenation);
//        AddOrSetJob(job, enqueue);
//    }
//
//    public void AttackMoveTo(Vector3 destenation, bool enqueue = false)
//    {
//        var job = new AttackMoveToJob(destenation);
//        AddOrSetJob(job, enqueue);
//    }
//
//    public void Stop()
//    {
//        _jobSystem.StopJobs();
//    }
//
//    private const KeyCode StopKey = KeyCode.S;
//    private const KeyCode AttackKey = KeyCode.A;
//    private const KeyCode QueueKey = KeyCode.LeftControl;
//    private const KeyCode LevelKey = KeyCode.LeftShift;
//    private const KeyCode ASpellKey = KeyCode.Q;
//    private const KeyCode BSpellKey = KeyCode.W;
//    private const KeyCode CSpellKey = KeyCode.E;
//    private const KeyCode DSpellKey = KeyCode.R;
//
//    private static readonly KeyCode[] SpellKeys = {ASpellKey, BSpellKey, CSpellKey, DSpellKey};
//
//    
//
//    private const int MouseLeftClick = 1;
//    private bool _attackMoveEnabled = false;
//
//    void Update()
//    {
//        if (Input.GetKeyDown(StopKey))
//        {
//            _attackMoveEnabled = false;
//            Stop();
//        }
//        else if (Input.GetKeyDown(AttackKey))
//        {
//            _attackMoveEnabled = !_attackMoveEnabled;
//        }
//        else if (Input.GetMouseButtonDown(MouseLeftClick))
//        {
//            var queue = Input.GetKey(QueueKey);
//            RaycastHit hitinfo;
//            if (!Physics.Raycast(MouseRay, out hitinfo, (int) LayerMaskHelper.World)) return;
//            var point = hitinfo.point;
//            if (_attackMoveEnabled)
//            {
//                AttackMoveTo(point, queue);
//            }
//            else
//            {
//                MoveTo(point, queue);
//            }
//        }
//        else
//        {
//            //Does nothing as of yet
////			var queue = Input.GetKey(QueueKey);
//            var level = Input.GetKey(LevelKey);
//            for(var i = 0; i < SpellKeys.Length; i++)
//                if (Input.GetKeyDown(SpellKeys[i]))
//                {
//                    CastOrLevelUp(i, level);
//                    break;
//                }
//        }
//    }
//
//    public static Ray MouseRay
//    {
//        get { return Camera.main.ScreenPointToRay(Input.mousePosition); }
//    }
//}