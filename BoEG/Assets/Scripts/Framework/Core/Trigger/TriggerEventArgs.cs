using System;
using UnityEngine;

namespace MobaGame.Framework.Core.Trigger
{
    public class TriggerEventArgs : EventArgs
    {
        public Collider Collider { get; set; }
    }
}