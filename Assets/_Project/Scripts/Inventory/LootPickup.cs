using UnityEngine;

namespace FinalDrop.Inventory
{
    [RequireComponent(typeof(Collider))]
    public class LootPickup : MonoBehaviour
    {
        [SerializeField] private ItemData item;
        [SerializeField] private int quantity = 1;

        private bool _playerInRange;
        private InventorySystem _nearbyInventory;

        private void Reset()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            var inv = other.GetComponent<InventorySystem>();
            if (inv == null) return;

            _playerInRange = true;
            _nearbyInventory = inv;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<InventorySystem>() == _nearbyInventory)
            {
                _playerInRange = false;
                _nearbyInventory = null;
            }
        }

        public void TryPickup()
        {
            if (!_playerInRange || _nearbyInventory == null) return;

            bool success = _nearbyInventory.AddItem(item, quantity);
            if (success)
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory full — cannot pick up " + item.itemName);
            }
        }
    }
}
