using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public class ItemDatabase : MonoBehaviour
{
	public static ItemDatabase m_instance = null;
	public List<MyItem> database = new List<MyItem> ();
	string folderName = "Items";
	//private JsonData itemData;

	void Awake ()
	{
		m_instance = this;
	
	}

	void Start ()
	{
		ConstructItemDatabase ();
	}

	public MyItem FetchItemByID (int id)
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

			database.Add (new MyItem (IntParse (chars [0]), (ItemType)System.Enum.Parse (typeof(ItemType), chars [1]), chars [2], IntParse (chars [3]), 
				IntParse (chars [4]), IntParse (chars [5]), IntParse (chars [6]), chars [7],
				bool.Parse (chars [8]), IntParse (chars [9]), (chars [10])));
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
public class MyItem
{
	public int ID{ get; set; }

	public ItemType Type{ get; set; }

	public string Name{ get; set; }

	public int Health{ get; set; }

	public int Power{ get; set; }

	public int Defence{ get; set; }

	public int Vitality{ get; set; }

	public string Description{ get; set; }

	public bool Stackable{ get; set; }

	public int Rarity{ get; set; }

	public string Slug{ get; set; }

	public Sprite Sprite{ get; set; }



	public MyItem (int id, ItemType type, string name, int health, int power, int defence, int vitality, string description, bool stackable, int rarity, string slug)
	{
		this.ID = id;

		this.Type = type;
		this.Name = name;
		this.Health = health;
		this.Power = power;
		this.Defence = defence;
		this.Vitality = vitality;
		this.Description = description;
		this.Stackable = stackable;
		this.Rarity = rarity;
		this.Slug = slug;
		this.Sprite = Resources.Load <Sprite> ("Textures/Inventory/" + slug);
	}

	public MyItem ()
	{
		this.ID = -1;
	}
}

public enum ItemType
{
	item,
	weapon,
	armor,
	building

}
