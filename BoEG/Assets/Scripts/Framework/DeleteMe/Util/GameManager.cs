using System;
using UnityEngine;

namespace Util
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null)
                throw new Exception("Cannot allow two game managers!");
            Instance = this;
        }
    }
}