using System;
using System.Collections;
using System.Collections.Generic;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using UnityEngine;

namespace MobaGame
{
    public class InventoryTester : MonoBehaviour
    {
        [Header("Initialize Inventory")] [SerializeField]
        private ItemObject[] InitialItems;
        [Header("Add/Remove Item")]
        [SerializeField] private bool AddItem;
        [SerializeField] private bool RemoveItem;
        [SerializeField] private ItemObject Item;
        
        private Actor _actor;
        private IInventoryable<IItem> _inventoryable;
        private void Start()
        {
            _actor = GetComponent<Actor>();
            _inventoryable = _actor.GetModule<IInventoryable<IItem>>();
            foreach(var item in InitialItems)
                _inventoryable.Inventory.Add(item);
        }

        // Update is called once per frame
        void Update()
        {
            try
            {
                if (AddItem)
                {
                    _inventoryable.Inventory.Add(Item);
                    AddItem = !AddItem;
                }

                if (RemoveItem)
                {
                    _inventoryable.Inventory.Remove(Item);
                    RemoveItem = !RemoveItem;
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning(e);
                AddItem = false;
                RemoveItem = false;
            }
        }
    }
}
