using UnityEngine;

public class InventorySlot : MonoBehaviour, InventoryDropListener {
    private const float INVENTORY_DROP_RADIUS = 2.0f;
    private static Vector3 INVENTORY_DROP_RADIUS_VECTOR = new Vector3(INVENTORY_DROP_RADIUS, INVENTORY_DROP_RADIUS, INVENTORY_DROP_RADIUS);
    
    public SpriteRenderer Icon;
    public InventoryType Item;
    [HideInInspector] public InventoryItemToSprites ItemMap; // this gets set in the Inventory object
    public bool locked = false;

    private bool isDragging = false;
    private SpriteRenderer DragIcon;
    private Camera cam;
    private Color normalColor;
    private Color draggingColor;
    private Color fadedColor;
    private SpriteMask _mask;
    private Collider2D _collider;

    private void Start() {
        cam = FindObjectOfType<Camera>();
        normalColor = Icon.color;
        draggingColor = new Color(1, 1, 1, 0.9f);
        fadedColor = new Color(.8f, .8f, .8f, 0.9f);
        _mask = GetComponent<SpriteMask>();
        _collider = GetComponent<Collider2D>();
        InventoryDropNotifier.AddListener(this);
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
            Debug.Log($"Mouse drag on inventory slot: {Item}");
            var drag = new GameObject();
            drag.transform.SetParent(transform);
            DragIcon = drag.AddComponent<SpriteRenderer>();
            DragIcon.sprite = Icon.sprite;
            DragIcon.sortingLayerName = Icon.sortingLayerName;
            DragIcon.color = draggingColor;
            DragIcon.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            var mask = drag.AddComponent<SpriteMask>();
            mask.sprite = _mask.sprite;
            Icon.color = fadedColor;
            isDragging = true;
        } else if (isDragging) {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            var mePos = transform.position;
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
            Debug.Log($"Mouse up on inventory slot: {Item}");
            isDragging = false;
            Icon.color = normalColor;
            var pos = DragIcon.transform.position;
            Destroy(DragIcon.gameObject);
            if (InventoryDropNotifier.NotifyOfDrop(gameObject, new Vector2(pos.x, pos.y), Item)) {
                Set(ItemMap.Get(InventoryType.EMPTY));
            }
        }
    }

    public bool isOverlappingPoint(Vector2 point) {
        return _collider.OverlapPoint(point);
    }

    public bool OnInventoryDrop(GameObject source, Vector2 worldPosition, InventoryType item) {
        if (source != gameObject && isOverlappingPoint(worldPosition) && Item == InventoryType.EMPTY) {
            var itemKey = ItemMap.Get(item);
            return Set(itemKey);
        }

        return false;
    }

    private void OnDestroy() {
        InventoryDropNotifier.RemoveListener(this);
    }
}

public enum InventoryType {
    EMPTY,
    PLUNGER,
    WRENCH,
    DUCTTAPE,
    DRAIN_SNAKE,
    PAINT,
    LIGHT_BULB,
}