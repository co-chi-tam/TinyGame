using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIListItemInventory : UIListObjects {

	public ListAdapter<CItemData> listAdapter;

	protected override void Awake ()
	{
		base.Awake ();
	}

	public override void OnListAlready ()
	{
		if (listAdapter != null) {
			length = listAdapter.length;
			listAdapter.OnClear -= OnListClear;
			listAdapter.OnClear += OnListClear;
			base.OnListAlready ();
		}
	}

	public override UIListItem OnListItemLoad (int index)
	{
		return base.OnListItemLoad (index);
	}

	public override void OnListItemUpdate (int index, UIListItem item)
	{
		base.OnListItemUpdate (index, item);
		var child 						= item as UIItemInventory;
		var valueItem 					= listAdapter [index] as CItemData;
		child.itemNameText.text 		= valueItem.name;
		child.itemImage.sprite			= Util.FindSprite (valueItem.avatar);
		child.itemAmountText.text 		= valueItem.amount.ToString();
		// Info
		child.itemInfoNameText.text 	= valueItem.name;
		child.itemInfoImage.sprite 		= Util.FindSprite (valueItem.avatar);
		child.itemInfoAmountText.text 	= valueItem.amount.ToString();
		child.id 						= valueItem.id;
		child.gameObject.SetActive (valueItem.amount > 0);
	}

	public override void OnListItemClicked (int index, Vector2 position, UIListItem item)
	{
		var valueItem	= listAdapter [index] as CItemData;
//		var child 		= item as UIItemShop;
		if (valueItem.amount >= valueItem.amountPerConsume) {
			base.OnListItemClicked (index, position, item);
		}
	}

}
