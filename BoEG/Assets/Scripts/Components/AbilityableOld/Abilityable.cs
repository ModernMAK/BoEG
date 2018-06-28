//using System;
//using UnityEngine;
//using UnityEngine.Networking;
//
//namespace Components.Abilityable
//{
//    public class Abilityable : BaseComponent
//    {
//        public KeyCode[] AbilityCodes;
//        public Ability[] Abilities;
////
////        protected override void Awake()
////        {
////            if (Abilities.Length > AbilityCodes.Length)
////                Array.Resize(ref AbilityCodes, Abilities.Length);
////            foreach(var ability in Abilities)
////                ability.Initialize(gameObject);
////        }
////
////        protected override void Update()
////        {
////            for(int i = 0; i < Abilities.Length; i++)
////                if(Input.GetKeyDown(AbilityCodes[i]))
////                    Abilities[i].Trigger();
////        }
//    }
//}