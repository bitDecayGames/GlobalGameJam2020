using TMPro;
using UnityEngine;

public class Wallet : MonoBehaviour, ShopPurchaseListener {
    public int Money;
    public TextMeshProUGUI TextMeshProUI;
    public int TotalMoney;

    private void Start() {
        ShopPurchaseNotifier.AddListener(this);
        TotalMoney = Money;
    }

    private void OnDestroy() {
        ShopPurchaseNotifier.RemoveListener(this);
    }

    private void Update() {
        TextMeshProUI.text = MoneyConverter.IntToMoney(Money);
    }

    public void AddMoney(int money) {
        Money += money;
        TotalMoney += money;
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