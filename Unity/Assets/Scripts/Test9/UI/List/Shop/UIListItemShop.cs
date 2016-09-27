using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIListItemShop : UIListObjects {

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
		var child = base.OnListItemLoad (index) as UIItemShop;
		var valueItem 				= listAdapter [index] as CItemData;
		child.itemName.text 		= valueItem.name;
		child.itemImage.sprite		= Util.FindSprite (valueItem.avatar);
		child.itemGoldPrice.text 	= valueItem.goldPrice.ToString();
		child.itemDiamondPrice.text = valueItem.diamondPrice.ToString();
		child.itemHotDeal.gameObject.SetActive (valueItem.hotDeal);
		// Info
		child.itemInfoNameText.text = valueItem.name;
		child.itemInfoImage.sprite 	= Util.FindSprite (valueItem.avatar);
		child.itemInfoGoldPrice.text 	= valueItem.goldPrice.ToString();
		child.itemInfoDiamondPrice.text = valueItem.diamondPrice.ToString();
		return child;
	}

	public override void OnListItemClicked (int index, Vector2 position, UIListItem item)
	{
		base.OnListItemClicked (index, position, item);
//		var valueItem	= listAdapter [index] as CItemData;
//		var child 		= item as UIItemShop;
	}

}
