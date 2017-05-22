using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour,IDropHandler, IPointerClickHandler
{
	public int id;

	public void OnDrop (PointerEventData eventData)
	{
		ItemData droppedItem = eventData.pointerDrag.GetComponent <ItemData> ();
		if (Inventory.m_instance.items [id].ID == -1) {
			Inventory.m_instance.items [droppedItem.slot] = new MyItem ();
			Inventory.m_instance.items [id] = droppedItem.item;
			droppedItem.slot = id;
		} else {
			Transform item = null;
			foreach (Transform transforms in this.transform) {
				if (transforms.CompareTag ("Item")) {
					item = transforms;
				}
			}
			if (item != null) {
				item.GetComponent <ItemData> ().slot = droppedItem.slot;
				item.transform.SetParent (Inventory.m_instance.slots [droppedItem.slot].transform);
				item.transform.position = Inventory.m_instance.slots [droppedItem.slot].transform.position;

				Inventory.m_instance.items [droppedItem.slot] = item.GetComponent <ItemData> ().item;
				Inventory.m_instance.items [id] = droppedItem.item;

				droppedItem.slot = id;
				droppedItem.transform.SetParent (this.transform);
				droppedItem.transform.position = this.transform.position;
			}
		}
		SelectSlot ();
	}

	public void OnPointerClick (PointerEventData eventData)
	{		
		SelectSlot ();
	}

	public void SelectSlot ()
	{
		Inventory.m_instance.slotSelectedImage.transform.parent = this.transform;
		//	Inventory.m_instance.slotSelectedImage.GetComponent <RectTransform> ().SetAsFirstSibling ();
		Inventory.m_instance.slotSelectedImage.GetComponent <RectTransform> ().anchoredPosition = Vector3.zero;
		//Inventory.m_instance.PrintItems ();
//		print (id + " " + transform.GetChild (0).GetComponent <ItemData> ().item.ID + " " + transform.GetChild (0).GetComponent <ItemData> ().item.Name);
	}
}
