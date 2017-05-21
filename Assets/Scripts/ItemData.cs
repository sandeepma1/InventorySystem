using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public MyItem item;
	public int amount = 1;
	public ItemType type;
	public int slot;
	Vector2 offset = new Vector2 (10, 0);

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
		this.transform.SetParent (Inventory.m_instance.slots [slot].transform);
		this.transform.position = Inventory.m_instance.slots [slot].transform.position;
		GetComponent <CanvasGroup> ().blocksRaycasts = true;
	}

	public void SelectedItem ()
	{
		Inventory.m_instance.selectedItem = item;
		print (item.Name);
	}
}
