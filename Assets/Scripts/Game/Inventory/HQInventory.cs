using System;
using UnityEngine;

public class HQInventory : MonoBehaviour {
    public Sprite FourSlotUpgradeSprite;
    public Sprite EightSlotUpgradeSprite;
    public Sprite TwelveSlotUpgradeSprite;
    public SpriteRenderer SlotRenderer;

    private Inventory inventory;

    private void Start() {
        inventory = GetComponentInChildren<Inventory>();
        UpgradeTo4Slots();
    }

    // private void Update() {
    //     if (Input.GetKeyDown(KeyCode.Alpha1)) {
    //         UpgradeTo4Slots();
    //     }
    //     if (Input.GetKeyDown(KeyCode.Alpha2)) {
    //         UpgradeTo8Slots();
    //     }
    //     if (Input.GetKeyDown(KeyCode.Alpha3)) {
    //         UpgradeTo12Slots();
    //     }
    // }

    public void UpgradeTo4Slots() {
        SlotRenderer.sprite = FourSlotUpgradeSprite;
        UpgradeToANumberOfSlots(4);
    }

    public void UpgradeTo8Slots() {
        SlotRenderer.sprite = EightSlotUpgradeSprite;
        UpgradeToANumberOfSlots(8);
    }

    public void UpgradeTo12Slots() {
        SlotRenderer.sprite = TwelveSlotUpgradeSprite;
        UpgradeToANumberOfSlots(12);
    }

    private void UpgradeToANumberOfSlots(int number) {
        for (int i = 0; i < inventory.Slots.Count; i++) {
            var inventorySlot = inventory.Slots[i];
            var inventorySlotSpriteRenderer = inventorySlot.GetComponent<SpriteRenderer>();
            if (i < number) {
                inventorySlot.locked = false;
                inventorySlotSpriteRenderer.enabled = true;
            } else {
                inventorySlot.locked = true;
                inventorySlotSpriteRenderer.enabled = false;
            }
        }
    }
}