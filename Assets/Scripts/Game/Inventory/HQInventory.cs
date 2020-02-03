using System;
using UnityEngine;

public class HQInventory : MonoBehaviour {
    public Sprite FourSlotUpgradeSprite;
    public Sprite EightSlotUpgradeSprite;
    public Sprite TwelveSlotUpgradeSprite;
    public SpriteRenderer SlotRenderer;
    public UpgradeButton FirstTruckUpgrade;
    public UpgradeButton SecondTruckUpgrade;
    public UpgradeButton ThirdTruckUpgrade;
    public UpgradeButton FirstExpandUpgrade;
    public UpgradeButton SecondExpandUpgrade;

    public Sprite firstDanUpgrade;
    public Sprite secondDanUpgrade;

    private Inventory inventory;
    private SpriteRenderer renderer;
    private int numOfTrucks = 1;

    private void Start() {
        inventory = GetComponentInChildren<Inventory>();
        UpgradeTo4Slots();
        var dans = FindObjectOfType<HQMarker>();
        if (dans != null) renderer = dans.GetComponentInChildren<SpriteRenderer>();
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
        renderer.sprite = firstDanUpgrade;
        renderer.transform.localScale = new Vector3(1.25f, 1.25f, 1); // because scaling is weird when coming from tiled
        SecondExpandUpgrade.SetIsAvailable(true);
    }

    public void UpgradeTo12Slots() {
        SlotRenderer.sprite = TwelveSlotUpgradeSprite;
        renderer.sprite = secondDanUpgrade;
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

    public void PurchaseNewTruck() {
        GameObject.Find("Pro2DCamera").GetComponent<MapLoader>().CreateTruckDefaultSpawn();
        numOfTrucks += 1;
        if (numOfTrucks == 2) {
            FMODSoundEffectsPlayer.Instance.PlaySoundEffect(SFX.NewTruck);
            SecondTruckUpgrade.SetIsAvailable(true);
        } else if (numOfTrucks == 3)
        {
            FMODSoundEffectsPlayer.Instance.PlaySoundEffect(SFX.NewTruck);
            ThirdTruckUpgrade.SetIsAvailable(true);
        }
    }
}