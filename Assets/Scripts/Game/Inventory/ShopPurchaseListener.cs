using UnityEngine;

public interface ShopPurchaseListener {
    bool OnPurchase(GameObject source, InventoryType item, float price);
}