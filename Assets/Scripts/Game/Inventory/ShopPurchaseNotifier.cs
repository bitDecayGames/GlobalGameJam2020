using System.Collections.Generic;
using UnityEngine;

public static class ShopPurchaseNotifier {
    private static List<ShopPurchaseListener> listeners = new List<ShopPurchaseListener>();

    public static void AddListener(ShopPurchaseListener listener) {
        listeners.Add(listener);
    }

    public static void RemoveListener(ShopPurchaseListener listener) {
        listeners.Remove(listener);
    }

    public static bool NotifyOfPurchase(GameObject source, InventoryType item, float price) {
        return listeners.Exists(l => l.OnPurchase(source, item, price));
    }
}