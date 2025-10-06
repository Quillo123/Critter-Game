using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UIElements;

public class HotbarUI : MonoBehaviour
{
    ItemDatabase itemDB => ItemDatabase.Instance;

    public Inventory inventory;
    private VisualElement root;
    private VisualElement inventoryGrid;

    [SerializeField] private int columns = 1; // Number of columns in the grid
    [SerializeField] private float slotSize = 64f; // Size of each inventory slot
    [SerializeField] private float slotPadding = 4f; // Padding between slots
    [SerializeField] private ItemHolder itemHolder; // Reference to player or drop position (e.g., player transform)
    [SerializeField] private Vector3 dropOffset = new Vector3(1f, 0f, 0f); // Offset from drop position

    private void Awake()
    {
        // Get the UIDocument component
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
    }

    private void OnEnable()
    {
        inventory.OnInventoryChanged += OnInventoryChanged;

        // Create the inventory grid when enabled
        CreateInventoryGrid();
    }

    private void Start()
    {
        UpdateInventoryUI();
    }

    private void CreateInventoryGrid()
    {
        // Create a container for the inventory grid
        inventoryGrid = new VisualElement
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                //flexWrap = Wrap.Wrap,
                width = columns * (slotSize + slotPadding * 2),
                paddingTop = slotPadding,
                paddingBottom = slotPadding,
                paddingLeft = slotPadding,
                paddingRight = slotPadding,
                backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.8f)
            }
        };
        root.Add(inventoryGrid);

        // Create slots for each inventory position
        for (int i = 0; i < inventory.capacity; i++)
        {
            var slotIndex = i; // Capture index for the click event
            var slot = new VisualElement
            {
                name = $"Slot_{i}",
                style =
                {
                    width = slotSize,
                    height = slotSize,
                    marginTop = slotPadding,
                    marginBottom = slotPadding,
                    marginLeft = slotPadding,
                    marginRight = slotPadding,
                    backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.9f),
                    borderTopLeftRadius = 4,
                    borderTopRightRadius = 4,
                    borderBottomLeftRadius = 4,
                    borderBottomRightRadius = 4
                }
            };

            // Add item image
            var itemImage = new VisualElement
            {
                style =
                {
                    width = slotSize - 8,
                    height = slotSize - 8,
                    marginTop = 4,
                    marginBottom = 4,
                    marginLeft = 4,
                    marginRight = 4
                }
            };
            slot.Add(itemImage);

            // Add item count label
            var countLabel = new Label
            {
                style =
                {
                    unityTextAlign = TextAnchor.LowerRight,
                    color = Color.white,
                    fontSize = 12,
                    position = Position.Absolute,
                    bottom = 4,
                    right = 4
                }
            };
            slot.Add(countLabel);

            // Register click event to drop item
            slot.RegisterCallback<ClickEvent>(evt => OnSlotClicked(slotIndex));

            inventoryGrid.Add(slot);
        }
    }

    // Handle slot click to drop item
    private void OnSlotClicked(int slotIndex)
    {
        if (inventory.IsSlotEmpty(slotIndex))
        {
            return; // Do nothing if the slot is empty
        }

        // Get the item ID from the slot
        string itemID = inventory.items[slotIndex].itemID;
        int itemCount = inventory.items[slotIndex].count;

        // Remove the item from the inventory
        inventory.RemoveItem(slotIndex);

        // Spawn the item in the game world
        if (itemDB != null && itemDB.itemPrefab != null)
        {
            Item item = itemDB.GetItemByID(itemID);
            if (item != null)
            {
                // Determine drop position (use player position or fallback to origin)
                Vector3 dropPosition = itemHolder.WorldHoldPosition().ToVector3() + dropOffset;

                // Instantiate the item entity
                ItemEntity itemEntity = Instantiate(itemDB.itemPrefab, dropPosition, Quaternion.identity);
                itemEntity.item = item;
                itemEntity.SetCooldown(3);
                itemEntity.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle * 30);
                // If ItemEntity has a count property, set it
                // Assuming ItemEntity has a public int count field or property
                // itemEntity.count = itemCount; // Uncomment and adjust if ItemEntity supports count
            }
            else
            {
                Logger.LogWarning($"Item with ID {itemID} not found in ItemDatabase.", gameObject);
            }
        }
        else
        {
            Logger.LogWarning("ItemDatabase or itemPrefab is not set.", gameObject);
        }

        // Update the UI
        UpdateInventoryUI();
    }

    [Button("UpdateInventoryUI")]
    public bool button_UpdateInventoryUI;
    public void UpdateInventoryUI()
    {
        for (int i = 0; i < inventory.capacity; i++)
        {
            var slot = inventoryGrid.Q<VisualElement>($"Slot_{i}");
            var itemImage = slot[0] as VisualElement;
            var countLabel = slot[1] as Label;

            if (!inventory.IsSlotEmpty(i))
            {
                var item = itemDB.GetItemByID(inventory.ReadSlot(i));
                if (item != null && item.sprite != null)
                {
                    // Set item sprite as background image
                    itemImage.style.backgroundImage = new StyleBackground(item.sprite);
                    countLabel.text = inventory.items[i].count.ToString();
                }
                else
                {
                    // Clear slot if item is invalid
                    itemImage.style.backgroundImage = null;
                    countLabel.text = "";
                }
            }
            else
            {
                // Clear empty slot
                itemImage.style.backgroundImage = null;
                countLabel.text = "";
            }
        }
    }

    // Call this when inventory changes (e.g., from Inventory.cs)
    public void OnInventoryChanged()
    {
        UpdateInventoryUI();
    }
}