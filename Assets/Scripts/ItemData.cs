using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public MyItem item;
	public int id = 0;
	public int amount = 1;
	public ItemType type;
	public int slotID;
	Vector2 offset = new Vector2 (10, 0);

	void Start ()
	{
		id = item.ID;
	}
	//remove id variable later

	public void OnBeginDrag (PointerEventData eventData)
	{
		if (item != null) {
			offset = eventData.position - new Vector2 (this.transform.position.x, this.transform.position.y);
			this.transform.SetParent (this.transform.parent.parent);
			this.transform.position = eventData.position - offset;
			GetComponent <CanvasGroup> ().blocksRaycasts = false;
			SelectedItem ();
		}
	}

	public void OnDrag (PointerEventData eventData)
	{
		if (item != null) {
			this.transform.position = eventData.position - offset;
		}
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		this.transform.SetParent (Inventory.m_instance.slots [slotID].transform);
		this.transform.SetAsFirstSibling ();	
		this.transform.position = Inventory.m_instance.slots [slotID].transform.position;
		GetComponent <CanvasGroup> ().blocksRaycasts = true;
	}

	public void SelectedItem ()
	{
		Inventory.m_instance.selectedItem = item;
	}
}
