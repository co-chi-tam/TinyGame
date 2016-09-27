using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CInventoryData : CBaseData {

	public string owner;
	public int gold;
	public int diamond;
	public CItemData[] items;

	public CInventoryData (): base ()
	{
		this.owner 		= string.Empty;
		this.gold 		= 0;
		this.diamond 	= 0;
		this.items 		= null;
	}
	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData		= instance as CInventoryData;
		this.id			= newData.id;
		this.owner 		= newData.owner;
		this.gold 		= newData.gold;
		this.diamond 	= newData.diamond;
		this.items 		= new CItemData[newData.items.Length];
		newData.items.CopyTo (this.items, 0);
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.id			= instance["id"].ToString();
		this.owner		= instance["owner"].ToString();
		this.gold		= int.Parse (instance["gold"].ToString());
		var diamond 	= int.Parse (instance["diamond"].ToString());
		var items		= instance ["items"] as List<object>;
		var inventory	= instance ["inventory"] as List<object>;
		this.items		= new CItemData[items.Count];
		for (int i = 0; i < items.Count; i++) {
			var slotItem = items [i] as Dictionary<string, object>;
			this.items [i] = CItemData.Parse (slotItem);
		}
		for (int i = 0; i < inventory.Count; i++) {
			var slotInventory = inventory[i] as Dictionary<string, object>;
			var gameType = slotInventory ["gameType"].ToString ();
			for (int j = 0; j < this.items.Length; j++) {
				var id = slotInventory ["id"].ToString ();
				var amount = int.Parse (slotInventory ["amount"].ToString ());
				if (this.items [j].id == gameType) {
					this.items [j].id = id;
					this.items [j].amount = amount;
				}
			}
		}
	}

	public static CInventoryData Clone(CInventoryData instance) {
		var tmp 		= new CInventoryData ();
		tmp.id			= instance.id;
		tmp.owner 		= instance.owner;
		tmp.gold 		= instance.gold;
		tmp.diamond 	= instance.diamond;
		tmp.items 		= new CItemData[instance.items.Length];
		instance.items.CopyTo (tmp.items, 0);
		return tmp;
	}

	public static CInventoryData Parse(Dictionary<string, object> instance) {
		var tmp 		= new CInventoryData ();
		tmp.id 			= instance["id"].ToString();
		tmp.id			= instance["id"].ToString();
		tmp.owner		= instance["owner"].ToString();
		tmp.gold		= int.Parse (instance["gold"].ToString());
		var diamond 	= int.Parse (instance["diamond"].ToString());
		var items		= instance ["items"] as List<object>;
		var inventory	= instance ["inventory"] as List<object>;
		tmp.items		= new CItemData[items.Count];
		for (int i = 0; i < items.Count; i++) {
			var slotItem = items [i] as Dictionary<string, object>;
			tmp.items [i] = CItemData.Parse (slotItem);
		}
		for (int i = 0; i < inventory.Count; i++) {
			var slotInventory = inventory[i] as Dictionary<string, object>;
			var gameType = slotInventory ["gameType"].ToString ();
			for (int j = 0; j < tmp.items.Length; j++) {
				var id = slotInventory ["id"].ToString ();
				var amount = int.Parse (slotInventory ["amount"].ToString ());
				if (tmp.items [j].id == gameType) {
					tmp.items [j].id = id;
					tmp.items [j].amount = amount;
				}
			}
		}
		return tmp;
	}

}
