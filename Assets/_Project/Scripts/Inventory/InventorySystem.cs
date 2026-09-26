using System.Collections.Generic;
using UnityEngine;

namespace FinalDrop.Inventory
{
    [System.Serializable]
    public class InventorySlot
    {
        public ItemData item;
        public int quantity;

        public bool IsEmpty => item == null || quantity <= 0;
    }

    public class InventorySystem : MonoBehaviour
    {
        [SerializeField] private int slotCount = 8;
        private List<InventorySlot> _slots;

        public IReadOnlyList<InventorySlot> Slots => _slots;

        private void Awake()
        {
            _slots = new List<InventorySlot>(slotCount);
            for (int i = 0; i < slotCount; i++)
                _slots.Add(new InventorySlot());
        }

        public bool AddItem(ItemData item, int quantity = 1)
        {
            if (item.isStackable)
            {
                foreach (var slot in _slots)
                {
                    if (slot.item == item && slot.quantity < item.maxStackSize)
                    {
                        int space = item.maxStackSize - slot.quantity;
                        int toAdd = Mathf.Min(space, quantity);
                        slot.quantity += toAdd;
                        quantity -= toAdd;
                        if (quantity <= 0) return true;
                    }
                }
            }

            foreach (var slot in _slots)
            {
                if (slot.IsEmpty)
                {
                    int toAdd = item.isStackable ? Mathf.Min(item.maxStackSize, quantity) : 1;
                    slot.item = item;
                    slot.quantity = toAdd;
                    quantity -= toAdd;
                    if (quantity <= 0) return true;
                }
            }

            return quantity <= 0;
        }

        public void RemoveItem(int slotIndex, int quantity = 1)
        {
            if (slotIndex < 0 || slotIndex >= _slots.Count) return;
            var slot = _slots[slotIndex];
            if (slot.IsEmpty) return;

            slot.quantity -= quantity;
            if (slot.quantity <= 0)
            {
                slot.item = null;
                slot.quantity = 0;
            }
        }
    }
}
