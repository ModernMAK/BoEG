using System;
using UnityEngine;

namespace Util
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        public static GameManager Instance
        {
            get { return _instance; }
        }

        protected virtual void Awake()
        {
            if (_instance != null)
                throw new Exception("Cannot allow two game managers!");
            _instance = this;
        }
    }
}