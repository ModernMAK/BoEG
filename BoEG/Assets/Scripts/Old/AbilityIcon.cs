//using System.Collections;
//using System.Collections.Generic;
//using Modules.Abilityable;
//using UnityEngine;
//using UnityEngine.UI;
//
//public class AbilityIcon : MonoBehaviour
//{
//    [SerializeField] private Ability _target;
//
//    [SerializeField] private Image _bg;
//    [SerializeField] private Image _fg;
//
//    [SerializeField] private Image _border;
//    [SerializeField] private Material _notLeveled;
//    [SerializeField] private Material _canLevel;
//    [SerializeField] private Material _onCooldown;
//    [SerializeField] private Material _isCasting;
//    [SerializeField] private Material _noMana;
////    [SerializeField] private Material _unleveledMat;
//
//
//    public void SetTarget(Ability rawAbility)
//    {
//        _target = rawAbility;
//    }
//    private void Update()
//    {
//        if( _target == null)
//            return;
//
//        if (_fg != null)
//        {
//            _fg.sprite = _target.Icon;
//            if (!_target.IsLeveled)
//                _fg.material = _notLeveled;
//            else if (!_target.OffCooldown)
//                _fg.material = _onCooldown;
//            else if (!_target.HasEnoughMana)
//                _fg.material = _noMana;
//            else
//                _fg.material = null;
//        }
//
//        if (_border != null)
//        {
//            if (_target.Preparing)
//            {
//                _border.enabled = true;
//                _border.material = _isCasting;
//            }
//            else if (_target.CanLevelUp)
//            {
//                _border.enabled = true;
//                _border.material = _canLevel;
//            }
//            else
//            {
//                _border.enabled = false;
//            }
//                
//                
//        }
//
//    }
//}