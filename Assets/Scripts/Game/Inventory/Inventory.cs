using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public InventoryItemToSprites ItemMap;
    public List<InventorySlot> Slots;

    private void Start() {
        Slots.ForEach(s => {
            s.ItemMap = ItemMap;
            s.Set(ItemMap.Get(s.Item));
        });
    }

    public void SetSlot(int index, InventoryType itemType) {
        if (index >= 0 && index < Slots.Count) {
            Slots[index].Set(ItemMap.Get(itemType));
        } else Debug.LogWarning($"Tried to set out of bounds inventory slot: length: {Slots.Count} index: {index}");
    }

    public InventoryType GetItemAt(int index) {
        if (index >= 0 && index < Slots.Count) {
            return Slots[index].Item;
        } else {
            Debug.LogWarning($"Tried to get out of bounds inventory slot: length: {Slots.Count} index: {index}");
            return InventoryType.EMPTY;
        }
    }

    public Sprite GetSpriteAt(int index) {
        if (index >= 0 && index < Slots.Count) {
            return Slots[index].Icon.sprite;
        } else {
            Debug.LogWarning($"Tried to get out of bounds inventory slot: length: {Slots.Count} index: {index}");
            return null;
        }
    }

    /// <summary>
    /// Use this method to remove items from a car's inventory when they reach a customer's house
    /// </summary>
    /// <param name="itemType">the type of inventory item</param>
    /// <returns>true if the item was in the players inventory</returns>
    public bool RemoveItemType(InventoryType itemType) {
        var index = Slots.FindIndex(s => s.Item == itemType);
        if (index >= 0) {
            Slots[index].Set(ItemMap.Get(InventoryType.EMPTY));
            return true;
        } else {
            Debug.LogWarning($"There was no item type of {itemType} in this inventory");
            return false;
        }
    }

    /// <summary>
    /// Use this method to check a car's inventory for the presence of an item
    /// </summary>
    /// <param name="itemType">the type of inventory item</param>
    /// <returns>true if the item was in the players inventory</returns>
    public bool CheckForItemType(InventoryType itemType) {
        var index = Slots.FindIndex(s => s.Item == itemType);
        if (index >= 0) {
            return true;
        } else {
            return false;
        }
    }

    public InventoryType RemoveItemAt(int index) {
        if (index >= 0 && index < Slots.Count) {
            var item = Slots[index].Item;
            Slots[index].Set(ItemMap.Get(InventoryType.EMPTY));
            return item;
        } else {
            Debug.LogWarning($"Tried to remove out of bounds inventory slot: length: {Slots.Count} index: {index}");
            return InventoryType.EMPTY;
        }
    }
}