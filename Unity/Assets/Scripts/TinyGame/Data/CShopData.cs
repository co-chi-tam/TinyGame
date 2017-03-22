using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CShopData : CBaseData {

	public string name;
	public string category;
	public CShopSlotData[] slots;
	public CItemData[] items;

	public CShopData (): base ()
	{
		this.name 		= string.Empty;
		this.category 	= string.Empty;
		this.slots 		= null;
		this.items 		= null;
	}

	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData		= instance as CShopData;
		this.id			= newData.id;
		this.name 		= newData.name;
		this.category 	= newData.category;
		this.slots 		= new CShopSlotData[newData.slots.Length];
		newData.slots.CopyTo (this.slots, 0);
		this.items		= new CItemData[newData.items.Length];
		newData.items.CopyTo (this.items, 0);
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.id			= instance["id"].ToString();
		this.name		= instance["name"].ToString();
		this.category	= instance["category"].ToString();
		var slots 		= instance["slots"] as List<object>;
		this.slots		= new CShopSlotData[slots.Count];
		for (int i = 0; i < slots.Count; i++) {
			var slotItem = slots [i] as Dictionary<string, object>;
			this.slots [i] = CShopSlotData.Parse (slotItem);
		}
		var items 		= instance["items"] as List<object>;
		this.items 		= new CItemData[items.Count]; 
		for (int i = 0; i < items.Count; i++) {
			var slotItem = items [i] as Dictionary<string, object>;
			this.items [i] = CItemData.Parse (slotItem);
			for (int j = 0; j < this.slots.Length; j++) {
				if (this.slots [j].gameType == this.items [i].id) {
					this.items [i].hotDeal = this.slots [j].hostDeal;
				}
			}
		}
	}

	public static CShopData Clone(CShopData instance) {
		var tmp 		= new CShopData ();
		tmp.id 			= instance.id;
		tmp.name 		= instance.name;
		tmp.category 	= instance.category;
		tmp.slots 		= new CShopSlotData[instance.slots.Length];
		instance.slots.CopyTo (tmp.slots, 0);
		tmp.items		= new CItemData[instance.items.Length];
		instance.items.CopyTo (tmp.items, 0);
		return tmp;
	}

	public static CShopData Parse(Dictionary<string, object> instance) {
		var tmp 		= new CShopData ();
		tmp.id 			= instance["id"].ToString();
		tmp.name		= instance["name"].ToString();
		tmp.category	= instance["category"].ToString();
		var slots 		= instance["slots"] as List<object>;
		tmp.slots		= new CShopSlotData[slots.Count];
		for (int i = 0; i < slots.Count; i++) {
			var slotItem = slots [i] as Dictionary<string, object>;
			tmp.slots [i] = CShopSlotData.Parse (slotItem);
		}
		var items 		= instance ["items"] as List<object>;
		tmp.items 		= new CItemData[items.Count]; 
		for (int i = 0; i < items.Count; i++) {
			var slotItem = items [i] as Dictionary<string, object>;
			tmp.items [i] = CItemData.Parse (slotItem);
			for (int j = 0; j < tmp.slots.Length; j++) {
				if (tmp.slots [j].gameType == tmp.items [i].id) {
					tmp.items [i].hotDeal = tmp.slots [j].hostDeal;
				}
			}
		}
		return tmp;
	}

}
