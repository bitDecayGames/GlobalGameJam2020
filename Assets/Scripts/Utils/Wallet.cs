using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (Money < 0) {
            SceneManager.LoadScene("GameOver");
        }
    }

    public void AddMoney(int money) {
        Money += money;
        TotalMoney += money;
    }

    public void SubtractMoney(int money) {
        Money -= money;
    }

    public void SubtractFromTotalMoney(int money) {
        Money -= money;
        TotalMoney -= money; // this is to keep the player in the correct difficulty level
    }

    public bool OnPurchase(GameObject source, InventoryType item, float price) {
        if (Money >= price) {
            SubtractMoney((int) price);
            return true;
        }

        return false;
    }
}