using UnityEngine;

public class CarInventory : MonoBehaviour {
    public Sprite ThreeSlotUpgradeSprite;
    public Sprite SixSlotUpgradeSprite;
    public SpriteRenderer SlotRenderer;

    private Inventory inventory;

    private void Start() {
        inventory = GetComponentInChildren<Inventory>();
        UpgradeTo3Slots();
    }
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            UpgradeTo3Slots();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            UpgradeTo6Slots();
        }
    }

    public void UpgradeTo3Slots() {
        SlotRenderer.sprite = ThreeSlotUpgradeSprite;
        UpgradeToANumberOfSlots(3);
    }

    public void UpgradeTo6Slots() {
        SlotRenderer.sprite = SixSlotUpgradeSprite;
        UpgradeToANumberOfSlots(6);
        // TODO: switch the truck visuals to be the van visuals 
        // TODO: SFX For upgrading vehicle
        transform.parent.GetComponent<VehicleRenderer>().upgraded = true;
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