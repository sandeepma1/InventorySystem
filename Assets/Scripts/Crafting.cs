using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
	public static Crafting m_instance = null;
	//public GameObject mainPanel;
	public GameObject craftPanel;
	public GameObject craftSlot;
	public Image slotSelectedImage;
	public int[] craftingItems;
	public List<GameObject> craftingSlots = new List<GameObject> ();

	public int selectedItemID = -1;

	void Awake ()
	{
		m_instance = this;
	}

	void Start ()
	{
		for (int i = 0; i < craftingItems.Length; i++) {		
			craftingSlots.Add (Instantiate (craftSlot, craftPanel.transform));
			craftingSlots [i].GetComponent <CraftingSlot> ().id = i;
			craftingSlots [i].GetComponent <CraftingSlot> ().itemID = craftingItems [i];
			craftingSlots [i].GetComponent <RectTransform> ().localScale = Vector3.one;
			craftingSlots [i].transform.GetChild (0).GetComponent<Image> ().sprite = ItemDatabase.m_instance.database [craftingItems [i]].Sprite;
		}
	}

	public void CraftSelectedItem ()
	{
		if (selectedItemID >= 0) {
			
			if (CheckForRequiredItemsInInventory ()) { //if inventory has all the craftable items
				RemoveItemsFromInventory ();
				Inventory.m_instance.AddItem (selectedItemID);
				print ("crafted item" + selectedItemID);
			} else {
				print ("missing Items");
			}
		}
	}

	void RemoveItemsFromInventory ()
	{
		MyItem itemToCraft = ItemDatabase.m_instance.FetchItemByID (selectedItemID);
		if (itemToCraft.ItemID1 >= 0) {
			for (int i = 0; i < itemToCraft.ItemAmount1; i++) {
				Inventory.m_instance.RemoveItem (itemToCraft.ItemID1);
			}
		}
		if (itemToCraft.ItemID2 >= 0) {
			for (int i = 0; i < itemToCraft.ItemAmount2; i++) {
				Inventory.m_instance.RemoveItem (itemToCraft.ItemID2);			
			}
		}
		if (itemToCraft.ItemID3 >= 0) {
			for (int i = 0; i < itemToCraft.ItemAmount3; i++) {
				Inventory.m_instance.RemoveItem (itemToCraft.ItemID3);
			}
		}
		if (itemToCraft.ItemID4 >= 0) {
			for (int i = 0; i < itemToCraft.ItemAmount4; i++) {
				Inventory.m_instance.RemoveItem (itemToCraft.ItemID4);
			}
		}
	}

	bool CheckForRequiredItemsInInventory ()
	{
		MyItem itemToCraft = ItemDatabase.m_instance.FetchItemByID (selectedItemID);
		bool item1 = false;
		bool item2 = false;
		bool item3 = false;
		bool item4 = false;

		if (itemToCraft.ItemID1 >= 0) {
			if (Inventory.m_instance.CheckItemAmountInInventory (itemToCraft.ItemID1) >= itemToCraft.ItemAmount1) {
				item1 = true;
			}
		} else {
			item1 = true;
		}

		if (itemToCraft.ItemID2 >= 0) {
			if (Inventory.m_instance.CheckItemAmountInInventory (itemToCraft.ItemID2) >= itemToCraft.ItemAmount2) {
				item2 = true;
			}
		} else {
			item2 = true;
		}
		if (itemToCraft.ItemID3 >= 0) {
			if (Inventory.m_instance.CheckItemAmountInInventory (itemToCraft.ItemID3) >= itemToCraft.ItemAmount3) {
				item3 = true;
			}
		} else {
			item3 = true;
		}
		if (itemToCraft.ItemID4 >= 0) {
			if (Inventory.m_instance.CheckItemAmountInInventory (itemToCraft.ItemID4) >= itemToCraft.ItemAmount4) {
				item4 = true;
			}
		} else {
			item4 = true;
		}

		if (item1 && item2 && item3 && item4) {
			return true;
		} else {
			return false;
		}
	}
}
