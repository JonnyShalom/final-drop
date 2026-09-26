using UnityEngine;

namespace FinalDrop.Inventory
{
    public enum ItemType { Ammo, HealingItem, Armor, Attachment, Weapon, Misc }

    [CreateAssetMenu(fileName = "NewItem", menuName = "FinalDrop/Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("Identity")]
        public string itemName = "Bandage";
        public ItemType type = ItemType.HealingItem;
        public Sprite icon;

        [Header("Stacking")]
        public bool isStackable = true;
        public int maxStackSize = 10;

        [Header("Effect (for consumables)")]
        public float healAmount = 0f;
        public float useTime = 3f;
    }
}
