using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image itemIcon;
    public Text stackText;
    public RectTransform durabilityBar;
    public MyItem item;
    public int amount = 1;
    public int durability = 0;
    public TypeOfItem typeOfItem;
    public int slotID;

    private float durabilityPercentage = 0.0f;
    private Vector2 offset = new Vector2(100, 100);
    private float barWidthMax = 80;
    private const float barHeight = 10f;

    private void Awake()
    {
        barWidthMax = durabilityBar.sizeDelta.x;
    }

    public void SetMaxDurability()
    {
        durabilityBar.sizeDelta = new Vector2(barWidthMax, barHeight);
    }

    public void DisableDurability()
    {
        durabilityBar.sizeDelta = new Vector2(0, barHeight);
    }

    public void DecreaseItemDurability(int damage)
    {
        durability -= damage;
        durabilityPercentage = (durability * 1.0f / item.Durability * 1.0f) * 100;
        if (durability <= 0)
        {
            Inventory.m_instance.DeleteSelectedItem();
        }
        durabilityBar.sizeDelta = new Vector2(durabilityPercentage * 0.9f, 10);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
            this.transform.SetParent(this.transform.parent.parent);
            this.transform.position = eventData.position - offset;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            SelectedItem();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            this.transform.position = eventData.position - offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(Inventory.m_instance.slotsGO[slotID].transform);
        transform.SetAsFirstSibling();
        transform.position = Inventory.m_instance.slotsGO[slotID].transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Crafting.m_instance.InspectCraftableItems();
    }

    public void SelectedItem()
    {
        Inventory.m_instance.selectedItem = item;
    }
}
