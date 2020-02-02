using TMPro;
using UnityEngine;

public class InventorySlot : MonoBehaviour, InventoryDropListener {
    private const float INVENTORY_DROP_RADIUS = 4.0f;
    private static Vector3 INVENTORY_DROP_RADIUS_VECTOR = new Vector3(INVENTORY_DROP_RADIUS, INVENTORY_DROP_RADIUS, INVENTORY_DROP_RADIUS);

    public SpriteRenderer Icon;
    public InventoryType Item;
    [HideInInspector] public InventoryItemToSprites ItemMap; // this gets set in the Inventory object
    public bool locked = false;
    public bool isShop = false;
    public bool isTrash = false;
    public float price = 0.0f;

    private bool isDragging = false;
    private SpriteRenderer DragIcon;
    private Camera cam;
    private Color normalColor;
    private Color draggingColor;
    private Color fadedColor;
    private Color textColor;
    private SpriteMask _mask;
    private Collider2D _collider;

    private void Start() {
        cam = FindObjectOfType<Camera>();
        normalColor = Icon.color;
        draggingColor = new Color(1, 1, 1, 0.9f);
        fadedColor = new Color(.8f, .8f, .8f, 0.9f);
        textColor = new Color(71 / 255.0f, 170 / 255.0f, 44 / 255.0f);
        _mask = GetComponent<SpriteMask>();
        _collider = GetComponent<Collider2D>();
        if (!isShop) InventoryDropNotifier.AddListener(this);
    }

    public bool Set(ItemKey itemKey) {
        if (itemKey != null) {
            Icon.sprite = itemKey.image;
            Item = itemKey.inventoryType;
            return true;
        }

        return false;
    }

    public void OnMouseDrag() {
        if (!isDragging && Item != InventoryType.EMPTY && !locked) {
            // on mouse drag start
            var drag = new GameObject();
            drag.transform.SetParent(transform);
            DragIcon = drag.AddComponent<SpriteRenderer>();
            DragIcon.sprite = Icon.sprite;
            DragIcon.sortingLayerName = Icon.sortingLayerName;
            DragIcon.color = draggingColor;
            Icon.color = fadedColor;
            if (isShop) {
                var textObj = new GameObject();
                textObj.transform.SetParent(drag.transform);
                var text = textObj.AddComponent<TextMeshPro>();
                var textLocPos = text.transform.localPosition;
                textLocPos.y = 0.4f;
                text.transform.localPosition = textLocPos;
                text.text = $"${price}";
                text.alignment = TextAlignmentOptions.Center;
                text.fontSize = 4;
                text.color = textColor;
                text.sortingLayerID = DragIcon.sortingLayerID;
            }

            isDragging = true;
        } else if (isDragging) {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            var mePos = transform.parent.position;
            mousePos.z = mePos.z;
            var pos = mousePos - mePos;
            if (pos.magnitude > INVENTORY_DROP_RADIUS) {
                pos.Normalize();
                pos.Scale(INVENTORY_DROP_RADIUS_VECTOR);
            }
            DragIcon.transform.position = pos + mePos;
        }
    }

    public void OnMouseUp() {
        if (isDragging) {
            isDragging = false;
            Icon.color = normalColor;
            var pos = DragIcon.transform.position;
            Destroy(DragIcon.gameObject);
            var pos2 = new Vector2(pos.x, pos.y);
            if (isShop) {
                if (InventoryDropNotifier.NotifyOfCheckDrop(gameObject, pos2)) {
                    if (ShopPurchaseNotifier.NotifyOfPurchase(gameObject, Item, price)) {
                        InventoryDropNotifier.NotifyOfDrop(gameObject, pos2, Item);
                        Debug.Log($"Purchased {Item}");
                        // TODO: FX play successful purchase ca-ching! because user bought the Item
                    } else {
                        Debug.Log($"Failed to purchase {Item}");
                        // TODO: FX play "can't purchase this object" because user tried to purchase something they can't afford
                    }
                }
            } else if (InventoryDropNotifier.NotifyOfDrop(gameObject, pos2, Item)) {
                Debug.Log($"Moved item {Item}");
                // TODO: FX: play moved item (Item) into some other inventory
                Set(ItemMap.Get(InventoryType.EMPTY));
            }
        }
    }

    public bool IsOverlappingPoint(Vector2 point) {
        return _collider.OverlapPoint(point);
    }

    public bool OnInventoryDrop(GameObject source, Vector2 worldPosition, InventoryType item) {
        if (OnInventoryCheckDrop(source, worldPosition)) {
            if (isTrash) {
                // TODO: FX: play item went into trash can noise
                return true;
            } else {
                var itemKey = ItemMap.Get(item);
                return Set(itemKey);
            }
        }

        return false;
    }

    public bool OnInventoryCheckDrop(GameObject source, Vector2 worldPosition) {
        return !locked && source != gameObject && IsOverlappingPoint(worldPosition) && Item == InventoryType.EMPTY;
    }

    private void OnDestroy() {
        if (!isShop) InventoryDropNotifier.RemoveListener(this);
    }
}

public enum InventoryType {
    EMPTY,
    PLUNGER,
    WRENCH,
    PAINT,
    LIGHT_BULB,
    BATTERY,
    HAMMER,
    JOB_PANEL, // this is a hack, just to get the sprite at a weird point in the job cycle
}