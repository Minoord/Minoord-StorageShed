using System.Collections.Generic;
using UnityEngine;

namespace Project.Inventory
{
    public abstract class BaseInventory : MonoBehaviour
    {
        private int _currentInventorySize;
        
        private readonly Dictionary<string, (Item item, int amount)> _storage = new();

        protected abstract int MaxInventorySize { get; }

        public bool TryGetItem(string itemId, int amount, out Item item)
        {
            item = null;
            
            if(!_storage.TryGetValue(itemId, out (Item item, int amount) itemData))
            {
                return false;
            }

            if(itemData.amount <= 0)
            {
                _storage.Remove(itemId);
                return false;
            }
            
            
            if(amount > itemData.amount)
            { 
                amount = itemData.amount;
            }

            itemData.amount -= amount;
            _storage[itemId] = (itemData.item, itemData.amount);
            item = itemData.item;

            return true;
        }


        public bool TryAddItem(string itemId, int amount)
        {
            if(_currentInventorySize + amount > MaxInventorySize)
            {
                return false;   
            }

            if (_storage.TryGetValue(itemId, out (Item item, int amount) itemData))
            {
                itemData.amount += amount;
                _currentInventorySize += amount;
                return true;
            }

            if (!ItemFinder.Instance.TryGetItem(itemId, out Item item))
            {
                return false;
            }
            
            _storage.Add(itemId, (item, amount));
            _currentInventorySize += amount;
            return true;
        }
    }
}
