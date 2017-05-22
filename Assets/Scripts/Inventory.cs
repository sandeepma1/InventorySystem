using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
			for (int i = 0; i < items.Count; i++) {								
				if (items [i].ID == id) {
					print (i);
					occurance.Add (i);					
				}
			}
			print ("occurance.Count " + occurance.Count);
			if (occurance.Count > 0) {
				for (int i = 0; i < occurance.Count; i++) {					
					ItemData data = slots [occurance [i]].transform.GetChild (0).GetComponent <ItemData> ();
					if (data.amount > 9) {
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
		//PrintItems ();
	}

	public void RemoveItem (int id)
	{
		id = inputFeildID;
		MyItem itemsToRemove = ItemDatabase.m_instance.FetchItemByID (id);

		if (itemsToRemove.Stackable) {
			for (int i = 0; i < items.Count; i++) {								
				if (items [i].ID == id) {
					ItemData data = slots [i].transform.GetChild (0).GetComponent <ItemData> ();
					if (data.amount > 1) {
						data.amount--;	
						data.transform.GetChild (0).GetComponent <Text> ().text = data.amount.ToString ();
					} else {
						DestroyItem (i);
					}
					print ("asd");
					break;			
						
				}
			}
		} else {
			for (int i = 0; i < items.Count; i++) {								
				if (items [i].ID == id) {
					DestroyItem (i);
					break;
				}
			}
		}
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			for (int i = 0; i < items.Count; i++) {			
				print (i + " " + items [i].ID + " " + items [i].Name);// items
				if (slots [i].transform.childCount > 0 && slots [i].transform.GetChild (0).CompareTag ("Item")) {
					print (i + " " + slots [i].transform.GetChild (0).GetComponent <ItemData> ().item.ID + " " + slots [i].transform.GetChild (0).GetComponent <ItemData> ().item.Name);
				}

			}
		}
	}

	void AddNewItemInUI (MyItem itemsToAdd)
	{
		for (int i = 0; i < items.Count; i++) {
			if (items [i].ID == -1) {
				items [i] = itemsToAdd;
				GameObject itemsGO = Instantiate (inventoryItem, slots [i].transform);
				itemsGO.transform.SetAsFirstSibling ();
				itemsGO.GetComponent <RectTransform> ().localScale = Vector3.one;
				itemsGO.GetComponent <ItemData> ().slot = i;
				itemsGO.GetComponent <ItemData> ().type = itemsToAdd.Type;			
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
		AddItem (inputFeildID);
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