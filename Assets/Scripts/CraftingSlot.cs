﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingSlot : MonoBehaviour,IPointerClickHandler
{
	public int id;

	public void OnPointerClick (PointerEventData eventData)
	{		
		Crafting.m_instance.slotSelectedImage.transform.parent = this.transform;
		Crafting.m_instance.slotSelectedImage.GetComponent <RectTransform> ().SetAsFirstSibling ();
		Crafting.m_instance.slotSelectedImage.GetComponent <RectTransform> ().anchoredPosition = Vector3.zero;
	}
}