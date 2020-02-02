using TMPro;
using UnityEngine;

public class Wallet : MonoBehaviour, ShopPurchaseListener {
    public int Money;
    public TextMeshProUGUI TextMeshProUI;

    private void Start() {
        ShopPurchaseNotifier.AddListener(this);
    }

    private void OnDestroy() {
        ShopPurchaseNotifier.RemoveListener(this);
    }

    private void Update() {
        TextMeshProUI.text = MoneyConverter.IntToMoney(Money);
    }

    public void AddMoney(int money) {
        Money += money;
    }

    public void SubtractMoney(int money) {
        Money -= money;
    }

    public bool OnPurchase(GameObject source, InventoryType item, float price) {
        if (Money >= price) {
            SubtractMoney((int) price);
            return true;
        }

        return false;
    }
}