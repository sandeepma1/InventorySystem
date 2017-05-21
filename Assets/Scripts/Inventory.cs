using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	public static Inventory m_instance = null;
	public GameObject inventorypanel;
	public GameObject slotPanel;
	public GameObject inventorySlot;
	public GameObject inventoryItem;
	public int slotAmount = 10;
	public Image slotSelectedImage;
	public List<MyItem> items = new List<MyItem> ();
	public List<GameObject> slots = new List<GameObject> ();
	public MyItem selectedItem = null;

	int inputFeildID = -1, inputFeildAmount = -1;

	void Awake ()
	{
		m_instance = this;
	}

	void Start ()
	{
		for (int i = 0; i < slotAmount; i++) {
			items.Add (new MyItem ());
			slots.Add (Instantiate (inventorySlot, slotPanel.transform));
			slots [i].GetComponent <Slot> ().id = i;
			slots [i].GetComponent <RectTransform> ().localScale = Vector3.one;
		}
		AddItem (0, 1);
		AddItem (1, 2);
		AddItem (3, 3);
	}

	public void AddItem (int id, int amount)
	{		
		MyItem itemsToAdd = ItemDatabase.m_instance.FetchItemByID (id);	

		if (itemsToAdd.Stackable && CheckItemInInventory (itemsToAdd)) {			
			for (int i = 0; i < items.Count; i++) {
				if (items [i].ID == id) {
					ItemData data = slots [i].transform.GetChild (0).GetComponent <ItemData> ();
					print (data.amount);
					if (data.amount < 99) {
						data.amount += amount;
						data.transform.GetChild (0).GetComponent <Text> ().text = data.amount.ToString ();
						break;
					} else {
						AddNewitem (itemsToAdd, amount);
						break;
					}						
				}
			}
		} else {
			AddNewitem (itemsToAdd, amount);
		}
	}

	void AddNewitem (MyItem itemsToAdd, int amount)
	{
		for (int i = 0; i < items.Count; i++) {
			if (items [i].ID == -1) {
				items [i] = itemsToAdd;
				GameObject itemsGO = Instantiate (inventoryItem, slots [i].transform);
				itemsGO.GetComponent <RectTransform> ().localScale = Vector3.one;
				itemsGO.GetComponent <ItemData> ().slot = i;
				itemsGO.GetComponent <ItemData> ().type = itemsToAdd.Type;
				itemsGO.GetComponent <ItemData> ().amount += amount;
				itemsGO.GetComponent <ItemData> ().transform.GetChild (0).GetComponent <Text> ().text += amount.ToString ();
				itemsGO.GetComponent <ItemData> ().item = itemsToAdd;
				itemsGO.GetComponent <Image> ().sprite = itemsToAdd.Sprite;
				itemsGO.GetComponent <RectTransform> ().anchoredPosition = Vector3.one;
				break;
			}
		}
	}

	bool CheckItemInInventory (MyItem item)
	{
		for (int i = 0; i < items.Count; i++) {
			if (items [i].ID == item.ID) {
				return true;
			}
		}
		return false;
	}

	public void AddItemButton ()
	{
		AddItem (inputFeildID, inputFeildAmount);
	}

	public void RemoveItem (int id)
	{
		id = inputFeildID;
		MyItem itemsToRemove = ItemDatabase.m_instance.FetchItemByID (id);
		if (CheckItemInInventory (itemsToRemove)) {
			if (itemsToRemove.Stackable) {
				for (int i = 0; i < items.Count; i++) {
					if (items [i].ID == id) {					
						ItemData data = slots [i].transform.GetChild (0).GetComponent <ItemData> ();
						if (data.amount > 1) {
							data.amount--;	
						} else {
							DestroyItem (i);
						}
						data.transform.GetChild (0).GetComponent <Text> ().text = data.amount.ToString ();
						break;				
					}
				}			
			} else {
				for (int i = 0; i < items.Count; i++) {
					print (items [i].Name);
					if (items [i].ID == inputFeildID) {					
						DestroyItem (i);
						break;
					} else {
						print ("nothing to remove");
					}
				}
			}
		}
	}

	void DestroyItem (int id)
	{
		Destroy (slots [id].transform.GetChild (0).gameObject);	
		items [id] = new MyItem ();
	}

	public void InputFeildID (string text)
	{
		int id;
		int.TryParse (text, out id);
		inputFeildID = id;	
	}

	public void InputFeildAmount (string text)
	{
		int amount;
		int.TryParse (text, out amount);
		inputFeildAmount = amount;	
	}
}


[System.Serializable]
public class InventoryItems
{
	public int ID{ get; set; }

	public int Amount{ get; set; }

	public int SlotID{ get; set; }

	public int Health{ get; set; }

	public InventoryItems (int id, int amount, int slotID, int health)
	{
		this.ID = id;
		this.Amount = amount;
		this.SlotID = slotID;
		this.Health = health;
	}
}