using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public class CraftingDatabase : MonoBehaviour
{
	public static CraftingDatabase m_instance = null;
	private List<CraftingItems> database = new List<CraftingItems> ();
	string folderName = "Crafting";
	//private JsonData itemData;

	void Awake ()
	{
		m_instance = this;
	}

	void Start ()
	{
		ConstructItemDatabase ();
	}

	public CraftingItems FetchitemByID (int id)
	{
		for (int i = 0; i < database.Count; i++) {
			if (database [i].ID == id) {
				return database [i];			
			}
		}
		return null;
	}

	void ConstructItemDatabase ()
	{		
		string[] lines = new string[100];
		string[] chars = new string[100];
		TextAsset itemCSV =	Resources.Load ("CSV/" + folderName) as TextAsset;
		lines = Regex.Split (itemCSV.text, "\r\n");
	
		for (int i = 1; i < lines.Length - 1; i++) {			
			chars = Regex.Split (lines [i], ",");
			database.Add (new CraftingItems (IntParse (chars [0]), IntParse (chars [1]), IntParse (chars [2]), IntParse (chars [3]), IntParse (chars [4]), IntParse (chars [5]), IntParse (chars [6]), IntParse (chars [7]), IntParse (chars [8]), IntParse (chars [9])));
		}
	}

	int IntParse (string text)
	{
		int num;
		if (int.TryParse (text, out num)) {
			return num;
		} else
			return 0;
	}

	float FloatParse (string text)
	{
		float result = 0.01f;
		float.TryParse (text, out result);
		return result;
	}
}

[System.Serializable]
public class CraftingItems
{
	public int ID{ get; set; }

	public int CraftsID{ get; set; }

	public int ItemID1{ get; set; }

	public int ItemAmount1{ get; set; }

	public int ItemID2{ get; set; }

	public int ItemAmount2{ get; set; }

	public int ItemID3{ get; set; }

	public int ItemAmount3{ get; set; }

	public int ItemID4{ get; set; }

	public int ItemAmount4{ get; set; }

	public Sprite Sprite{ get; set; }

	public CraftingItems (int id, int craftsID, int itemID1, int itemAmount1, int itemID2, int itemAmount2, int itemID3, int itemAmount3, int itemID4, int itemAmount4)
	{
		this.ID = id;
		this.CraftsID = craftsID;
		this.ItemID1 = itemID1;
		this.ItemAmount1 = itemAmount1;
		this.ItemID2 = itemID2;
		this.ItemAmount2 = itemAmount2;
		this.ItemID3 = itemID3;
		this.ItemAmount3 = itemAmount3;
		this.ItemID4 = itemID4;
		this.ItemAmount4 = itemAmount4;
		//this.Sprite = Resources.Load <Sprite> ("Textures/Inventory/" + ItemDatabase.m_instance.database [craftsID].Slug);
	}

	public CraftingItems ()
	{
		this.ID = -1;
	}
}
