using System.Collections.Generic;
using UnityEngine;

public static class InventoryDropNotifier {
    private static List<InventoryDropListener> listeners = new List<InventoryDropListener>();

    public static void AddListener(InventoryDropListener listener) {
        listeners.Add(listener);
    }

    public static void RemoveListener(InventoryDropListener listener) {
        listeners.Remove(listener);
    }

    public static bool NotifyOfDrop(GameObject source, Vector2 worldPosition, InventoryType item) {
        return listeners.Exists(l => l.OnInventoryDrop(source, worldPosition, item));
    }
}