using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler, IPointerDownHandler
{
    public int slotId;
    public TypeOfSlot typeOfSlot;

    public void InitializeSlot(int slotId, TypeOfSlot typeOfSlot)
    {
        this.slotId = slotId;
        this.typeOfSlot = typeOfSlot;
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItemData droppedItem = eventData.pointerDrag.GetComponent<InventoryItemData>();

        switch (typeOfSlot)
        {
            case TypeOfSlot.item:
                break;
            case TypeOfSlot.armor:
                if (droppedItem.typeOfItem != TypeOfItem.armor)
                {
                    return;
                }
                break;
            case TypeOfSlot.chest:
                break;
            default:
                break;
        }

        if (Inventory.m_instance.items[slotId].ID == -1)
        {
            Inventory.m_instance.items[droppedItem.slotID] = new MyItem();
            Inventory.m_instance.items[slotId] = droppedItem.item;
            droppedItem.slotID = slotId;
        }
        else
        {
            if (droppedItem.slotID >= Inventory.m_instance.inventorySlotAmount && droppedItem.slotID < Inventory.m_instance.armourSlotAmount + Inventory.m_instance.inventorySlotAmount)
            {
                return;
            }
            Transform item = null;
            foreach (Transform transforms in this.transform)
            {
                if (transforms.CompareTag("Item"))
                {
                    item = transforms;
                }
            }
            if (item != null)
            {
                if (item.GetComponent<InventoryItemData>().item.ID == droppedItem.item.ID && droppedItem.item.Stackable)
                {   //if item is dropped on	same item
                    if (item.GetComponent<InventoryItemData>().amount + droppedItem.amount > Inventory.m_instance.maxStackAmount)
                    { //if both item sum is grater tahn max stack amount
                        droppedItem.amount = (item.GetComponent<InventoryItemData>().amount + droppedItem.amount) - Inventory.m_instance.maxStackAmount;
                        droppedItem.transform.GetChild(0).GetComponent<Text>().text = droppedItem.amount.ToString();
                        item.GetComponent<InventoryItemData>().amount = Inventory.m_instance.maxStackAmount;
                        item.GetComponent<InventoryItemData>().transform.GetChild(0).GetComponent<Text>().text = item.GetComponent<InventoryItemData>().amount.ToString();
                    }
                    else
                    {
                        item.GetComponent<InventoryItemData>().amount += droppedItem.amount;
                        item.GetComponent<InventoryItemData>().transform.GetChild(0).GetComponent<Text>().text = item.GetComponent<InventoryItemData>().amount.ToString();
                        Inventory.m_instance.items[droppedItem.slotID] = new MyItem();
                        DestroyImmediate(droppedItem.gameObject);
                        print("added and deleted down item " + droppedItem.GetComponent<InventoryItemData>().item.Name);
                    }
                }
                else
                {//swap if not same item
                    item.GetComponent<InventoryItemData>().slotID = droppedItem.slotID;
                    item.transform.SetParent(Inventory.m_instance.slotsGO[droppedItem.slotID].transform);
                    item.transform.position = Inventory.m_instance.slotsGO[droppedItem.slotID].transform.position;

                    Inventory.m_instance.items[droppedItem.slotID] = item.GetComponent<InventoryItemData>().item;
                    Inventory.m_instance.items[slotId] = droppedItem.item;

                    droppedItem.slotID = slotId;
                    droppedItem.transform.SetParent(this.transform);
                    droppedItem.transform.position = this.transform.position;
                }
            }
        }
        SelectSlot();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectSlot();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SelectSlot();
    }

    public void SelectSlot()
    {
        Inventory.m_instance.selectedSlotID = slotId;
        Inventory.m_instance.slotSelectedImage.transform.SetParent(this.transform);
        Inventory.m_instance.slotSelectedImage.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }
}

public enum TypeOfSlot
{
    item,
    armor,
    chest
}