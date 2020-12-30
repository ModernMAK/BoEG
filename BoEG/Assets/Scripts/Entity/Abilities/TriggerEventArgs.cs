using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    public class TriggerEventArgs : EventArgs
    {
        public Collider Collider { get; set; }
    }
}