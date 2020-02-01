using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryMap", menuName = "Inventory", order = 0)]
public class InventoryItemToSprites : ScriptableObject {
    public List<ItemKey> items;

    public ItemKey Get(InventoryType inventoryType) {
        return items.Find(i => i.inventoryType == inventoryType);
    }
}

[Serializable]
public class ItemKey {
    public Sprite image;
    public InventoryType inventoryType;

    public ItemKey(Sprite image, InventoryType inventoryType) {
        this.image = image;
        this.inventoryType = inventoryType;
    }
}