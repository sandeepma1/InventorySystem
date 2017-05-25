using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	public static Inventory m_instance = null;
	public GameObject inventorypanel;
	public GameObject inventorySlotPanel, armourSlotPanel;
	public GameObject inventorySlot, armorSlot;
	public GameObject inventoryItem;
	public int inventorySlotAmount = 10, armourSlotAmount = 4;
	public Image slotSelectedImage;
	public List<MyItem> l_items = new List<MyItem> ();
	public List<GameObject> slotsGO = new List<GameObject> ();
	public MyItem selectedItem = null;
	public int selectedSlotID = -1;
	public int maxStackAmount = 10;
	public Text debug;

	int inputFeildID = -1, inputFeildAmount = -1;
	InventoryItems[] myInventory;

	void Awake ()
	{
		m_instance = this;
	}

	void Start ()
	{
		for (int i = 0; i < inventorySlotAmount; i++) {
			l_items.Add (new MyItem ());
			slotsGO.Add (Instantiate (inventorySlot, inventorySlotPanel.transform));
			slotsGO [i].GetComponent <InventorySlot> ().id = i;
			slotsGO [i].GetComponent <RectTransform> ().localScale = Vector3.one;
		}
		for (int i = inventorySlotAmount; i < inventorySlotAmount + armourSlotAmount; i++) {
			l_items.Add (new MyItem ());
			slotsGO.Add (Instantiate (armorSlot, armourSlotPanel.transform));
			slotsGO [i].GetComponent <ArmourSlot> ().id = i;
			slotsGO [i].GetComponent <RectTransform> ().localScale = Vector3.one;
		}

		myInventory = new InventoryItems[slotsGO.Count];

		AddItem (3);
		AddItem (2);
		AddItem (1);
		AddItem (1);
		AddItem (1);
		AddItem (1);
		AddItem (7);
	}

	public void AddItem (int id)
	{
		List<int> occurance = new List<int> ();
		MyItem itemsToAdd = ItemDatabase.m_instance.FetchItemByID (id);	
		if (itemsToAdd.Stackable) {
			for (int i = 0; i < l_items.Count; i++) {								
				if (l_items [i].ID == id) {
					occurance.Add (i);					
				}
			}
			if (occurance.Count > 0) {
				for (int i = 0; i < occurance.Count; i++) {					
					InventoryItemData data = slotsGO [occurance [i]].transform.GetChild (0).GetComponent <InventoryItemData> ();
					if (data.amount >= maxStackAmount) {
						if (i == occurance.Count - 1) {
							AddNewItemInUI (itemsToAdd);
						} else {
							continue;		
						}								
					} else {
						data.amount++;
						data.transform.GetChild (0).GetComponent <Text> ().text = data.amount.ToString ();
						break;
					}
				}
			} else {
				AddNewItemInUI (itemsToAdd);
			}			
		} else {
			AddNewItemInUI (itemsToAdd);
		}
		SaveInventoryItems ();
	}

	public void RemoveItem (int id)
	{		
		MyItem itemsToRemove = ItemDatabase.m_instance.FetchItemByID (id);
		//print ("removed " + itemsToRemove.Name);
		if (itemsToRemove.Stackable) {
			for (int i = 0; i < l_items.Count; i++) {								
				if (l_items [i].ID == id) {
					InventoryItemData data = slotsGO [i].transform.GetChild (0).GetComponent <InventoryItemData> ();
					if (data.amount > 1) {
						data.amount--;	
						data.transform.GetChild (0).GetComponent <Text> ().text = data.amount.ToString ();
					} else {
						DestroyItem (i);
					}
					break;						
				}
			}
		} else {
			for (int i = 0; i < l_items.Count; i++) {								
				if (l_items [i].ID == id) {
					DestroyItem (i);
					break;
				}
			}
		}
		SaveInventoryItems ();
	}

	public void DeleteSelectedItem ()
	{
		if (selectedSlotID >= 0 && slotsGO [selectedSlotID].transform.childCount > 0 && slotsGO [selectedSlotID].transform.GetChild (0).CompareTag ("Item")) {
			l_items [selectedSlotID] = new MyItem ();
			Destroy (slotsGO [selectedSlotID].transform.GetChild (0).gameObject);
		}
	}

	void Update ()
	{
		/*foreach (var item in l_items) {
			if (item != null) {
				debug.text = item.ID + " " + item.Name;
			}

		}*/
		if (Input.GetKeyDown (KeyCode.Space)) {
			for (int i = 0; i < slotsGO.Count; i++) {			
				print ("inv " + slotsGO [i].GetComponent <InventorySlot> ().id + " " + l_items [i].ID);
			}
			//print (CheckInventoryHasAtleastOneSpace ());
		}
	}

	void AddNewItemInUI (MyItem itemsToAdd)
	{
		if (CheckInventoryHasAtleastOneSpace ()) {
			for (int i = 0; i < l_items.Count - armourSlotAmount; i++) {
				if (l_items [i].ID == -1) {
					l_items [i] = itemsToAdd;
					GameObject itemsGO = Instantiate (inventoryItem, slotsGO [i].transform);
					itemsGO.transform.SetAsFirstSibling ();
					itemsGO.GetComponent <RectTransform> ().localScale = Vector3.one;
					itemsGO.GetComponent <InventoryItemData> ().slotID = i;
					itemsGO.GetComponent <InventoryItemData> ().type = itemsToAdd.Type;			
					if (itemsToAdd.Durability > 0) {
						itemsGO.GetComponent <InventoryItemData> ().durability = itemsToAdd.Durability;
						itemsGO.transform.GetChild (1).GetComponent <RectTransform> ().sizeDelta = new Vector2 (90, 10);
					} else {
						itemsGO.GetComponent <InventoryItemData> ().durability = -1;
						itemsGO.transform.GetChild (1).GetComponent <RectTransform> ().sizeDelta = new Vector2 (0, 10);
					}
					itemsGO.GetComponent <InventoryItemData> ().item = itemsToAdd;
					itemsGO.GetComponent <Image> ().sprite = itemsToAdd.Sprite;
					itemsGO.GetComponent <RectTransform> ().anchoredPosition = Vector3.one;
					break;
				}
			}
		}
	}

	bool CheckItemInInventory (MyItem item)
	{
		for (int i = 0; i < l_items.Count - armourSlotAmount; i++) {
			if (l_items [i].ID == item.ID) {
				return true;
			}
		}
		return false;
	}

	public int CheckItemAmountInInventory (int id) //do this by slots or items saved
	{
		int amount = 0;
		for (int i = 0; i < slotsGO.Count; i++) {
			if (slotsGO [i].transform.childCount > 0 && slotsGO [i].transform.GetChild (0).CompareTag ("Item") && slotsGO [i].transform.GetChild (0).GetComponent <InventoryItemData> ().item.ID == id) {
				amount += slotsGO [i].transform.GetChild (0).GetComponent <InventoryItemData> ().amount;				
			}
		}
		return amount;
	}

	public bool CheckInventoryHasAtleastOneSpace ()
	{
		for (int i = 0; i < slotsGO.Count - armourSlotAmount; i++) {
			if (slotsGO [i].transform.childCount <= 0) {
				return true;
			} else if (!slotsGO [i].transform.GetChild (0).CompareTag ("Item")) {
				return true;
			} 
		}
		return false;
	}

	void SaveInventoryItems ()
	{
		foreach (var item in l_items) {
			if (item != null) {
				//print (item.ID + " " + item.Name);
			}
		}
		Crafting.m_instance.CheckHighlightCraftableItems ();
		for (int i = 0; i < slotsGO.Count; i++) {
			if (slotsGO [i].transform.childCount > 0 && slotsGO [i].transform.GetChild (0).CompareTag ("Item")) {
				myInventory [i] = new InventoryItems (slotsGO [i].transform.GetChild (0).GetComponent <InventoryItemData> ().item.ID,
					slotsGO [i].transform.GetChild (0).GetComponent <InventoryItemData> ().amount,
					slotsGO [i].transform.GetChild (0).GetComponent <InventoryItemData> ().slotID,
					slotsGO [i].transform.GetChild (0).GetComponent <InventoryItemData> ().item.Durability);
			} else {
				myInventory [i] = new InventoryItems ();
			}
		}
		//print ("done");
	}

	void LoadInventoryItems ()
	{
		InventoryItems[] myInventory = new InventoryItems[slotsGO.Count];// = new InventoryItems ();
		for (int i = 0; i < slotsGO.Count; i++) {
			if (slotsGO [i].transform.childCount > 0 && slotsGO [i].transform.GetChild (0).CompareTag ("Item")) {
				myInventory [i].ID = slotsGO [i].transform.GetChild (0).GetComponent <InventoryItemData> ().item.ID;
				myInventory [i].Amount = slotsGO [i].transform.GetChild (0).GetComponent <InventoryItemData> ().amount;
				myInventory [i].SlotID = slotsGO [i].transform.GetChild (0).GetComponent <InventoryItemData> ().slotID;
				myInventory [i].Health = slotsGO [i].transform.GetChild (0).GetComponent <InventoryItemData> ().item.Durability;
			} else {
				myInventory [i] = new InventoryItems ();
			}
		}
	}

	public void DecreseWeaponDurability (int amount)
	{
		if (selectedSlotID >= 0 && slotsGO [selectedSlotID].transform.childCount > 0 &&
		    slotsGO [selectedSlotID].transform.GetChild (0).CompareTag ("Item") &&
		    slotsGO [selectedSlotID].transform.GetChild (0).GetComponent <InventoryItemData> ().durability >= 0) {
			slotsGO [selectedSlotID].transform.GetChild (0).GetComponent <InventoryItemData> ().DecreaseItemDurability (amount);
		}
	}

	public void AddItemButton ()
	{
		AddItem (inputFeildID);
	}

	public void RemoveItemButton ()
	{
		RemoveItem (inputFeildID);
	}

	public void DestroyItem (int id)
	{
		for (int i = 0; i < slotsGO [id].transform.childCount; i++) {
			if (slotsGO [id].transform.GetChild (i).CompareTag ("Item")) {
				Destroy (slotsGO [id].transform.GetChild (i).gameObject);	
			}
		}

		l_items [id] = new MyItem ();
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

	public InventoryItems ()
	{
		this.ID = -1;
		this.Amount = -1;
		this.SlotID = -1;
		this.Health = -1;
	}
}