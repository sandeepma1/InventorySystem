using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
	public static Crafting m_instance = null;
	public GameObject mainPanel;
	public GameObject craftPanel;
	public GameObject craftSlot;
	public GameObject craftItem;
	public int slotAmount = 10;
	public List<int> craftingItems = new List<int> ();
	public Image slotSelectedImage;
	public List<CraftingItems> craftableItems = new List<CraftingItems> ();
	public List<GameObject> craftingSlots = new List<GameObject> ();

	void Awake ()
	{
		m_instance = this;
	}

	void Start ()
	{
		for (int i = 0; i < slotAmount; i++) {
			craftableItems.Add (new CraftingItems ());
			craftingSlots.Add (Instantiate (craftSlot, craftPanel.transform));
			craftingSlots [i].GetComponent <CraftingSlot> ().id = i;
			craftingSlots [i].GetComponent <RectTransform> ().localScale = Vector3.one;
		}
		AddItem (7);
		AddItem (8);
		AddItem (9);
	}

	public void AddItem (int id)
	{
		GameObject itemsGO = Instantiate (craftItem, craftingSlots [id].transform);
		itemsGO.GetComponent <RectTransform> ().localScale = Vector3.one;	
		itemsGO.GetComponent <RectTransform> ().anchoredPosition = Vector3.one;

		/*CraftingItems itemsToAdd = CraftingDatabase.m_instance.FetchitemByID (id);				
		for (int i = 0; i < craftableItems.Count; i++) {
			GameObject itemsGO = Instantiate (craftItem, craftingSlots [i].transform);
			itemsGO.GetComponent <RectTransform> ().localScale = Vector3.one;
			//itemsGO.GetComponent <Image> ().sprite = itemsToAdd.Sprite;
			itemsGO.GetComponent <RectTransform> ().anchoredPosition = Vector3.one;
		}*/
	}
}
