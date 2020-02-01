using UnityEngine;

public interface InventoryDropListener {
    bool OnInventoryDrop(GameObject source, Vector2 worldPosition, InventoryType item);
}