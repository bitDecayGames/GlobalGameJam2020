using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeButton : MonoBehaviour {
    public float Price = 0.0f;
    public bool IsAvailable = true;
    public UnityEvent OnPurchase;

    private void Start() {
        SetPrice(Price);
        SetIsAvailable(IsAvailable);
    }

    public void SetPrice(float price) {
        GetComponentInChildren<TextMeshPro>().text = MoneyConverter.FloatToMoney(price);
    }

    public void OnMouseUpAsButton() {
        if (ShopPurchaseNotifier.NotifyOfPurchase(gameObject, InventoryType.EMPTY, Price)) {
            if (OnPurchase != null) OnPurchase.Invoke();
            SetIsAvailable(false);
        }
    }

    public void SetIsAvailable(bool isAvailable) {
        IsAvailable = isAvailable;
        gameObject.SetActive(true);
    }

    public void Update() {
        if (!IsAvailable) {
            gameObject.SetActive(false);
        }
    }
}