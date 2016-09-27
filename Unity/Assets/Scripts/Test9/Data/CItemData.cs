using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CItemData : CBaseData {

	public string name;
	public string avatar;
	public string modelPath;
	public int goldPrice;
	public int diamondPrice;
	public int amount;
	public int amountPerConsume;
	public bool hotDeal;

	public CItemData () : base()
	{
		this.name = string.Empty;
		this.avatar = string.Empty;
		this.modelPath = string.Empty;
		this.goldPrice = 0;
		this.diamondPrice = 0;
		this.amount = 0;
		this.amountPerConsume = 0;
		this.hotDeal = false;
	}

	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData		= instance as CItemData;
		this.id			= newData.id;
		this.name 		= newData.name;
		this.avatar 	= newData.avatar;
		this.modelPath 	= newData.modelPath;
		this.goldPrice 	= newData.goldPrice;
		this.diamondPrice 	= newData.diamondPrice;
		this.amount 	= newData.amount;
		this.amountPerConsume = newData.amountPerConsume;
		this.hotDeal = newData.hotDeal;
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.id			= instance["id"].ToString();
		this.name		= instance["name"].ToString();
		this.avatar 	= instance["avatar"].ToString();
		this.modelPath 	= instance["modelPath"].ToString();
		this.goldPrice		= int.Parse (instance["goldPrice"].ToString());
		this.diamondPrice	= int.Parse (instance["diamondPrice"].ToString());
		this.amountPerConsume = int.Parse (instance ["amountPerConsume"].ToString ());
	}

	public static CItemData Clone(CItemData instance) {
		var tmp 		= new CItemData ();
		tmp.id			= instance.id;
		tmp.name 		= instance.name;
		tmp.avatar 		= instance.avatar;
		tmp.modelPath	= instance.modelPath;
		tmp.goldPrice	= instance.goldPrice;
		tmp.diamondPrice = instance.diamondPrice;
		tmp.amount 		= instance.amount;
		tmp.amountPerConsume = instance.amountPerConsume;
		tmp.hotDeal = instance.hotDeal;
		return tmp;
	}

	public static CItemData Parse(Dictionary<string, object> instance) {
		var tmp 		= new CItemData ();
		tmp.id 			= instance ["id"].ToString ();
		tmp.name 		= instance ["name"].ToString ();
		tmp.avatar 		= instance ["avatar"].ToString ();
		tmp.modelPath 	= instance ["modelPath"].ToString ();
		tmp.goldPrice		= int.Parse (instance["goldPrice"].ToString());
		tmp.diamondPrice	= int.Parse (instance["diamondPrice"].ToString());
		tmp.amountPerConsume = int.Parse (instance ["amountPerConsume"].ToString ());
		return tmp;
	}
}
