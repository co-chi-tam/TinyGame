using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CShopSlotData : CSlotData {

	public int goldPercentPrice;
	public int diamondPercentPrice;
	public bool hostDeal;

	public CShopSlotData () : base()
	{
		this.goldPercentPrice = 0;
		this.diamondPercentPrice = 0;
		this.hostDeal = false;
	}

	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData			= instance as CShopSlotData;
		this.id				= newData.id;
		this.gameType 		= newData.gameType;
		this.goldPercentPrice 		= newData.goldPercentPrice;
		this.diamondPercentPrice 	= newData.diamondPercentPrice;
		this.hostDeal 		= newData.hostDeal;
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.id				= instance["id"].ToString();
		this.gameType		= instance["gameType"].ToString();
		this.goldPercentPrice		= int.Parse (instance["goldPercentPrice"].ToString());
		this.diamondPercentPrice	= int.Parse (instance["diamondPercentPrice"].ToString());
		this.hostDeal		= int.Parse (instance["hotDeal"].ToString()) == 1;
	}

	public static CShopSlotData Clone(CShopSlotData instance) {
		var tmp 			= new CShopSlotData ();
		tmp.id 				= instance.id;
		tmp.gameType 		= instance.gameType;
		tmp.goldPercentPrice 		= instance.goldPercentPrice;
		tmp.diamondPercentPrice 	= instance.diamondPercentPrice;
		tmp.hostDeal 		= instance.hostDeal;
		return tmp;
	}

	public static CShopSlotData Parse(Dictionary<string, object> instance) {
		var tmp 			= new CShopSlotData ();
		tmp.id 				= instance["id"].ToString();
		tmp.gameType		= instance["gameType"].ToString();
		tmp.goldPercentPrice		= int.Parse (instance["goldPercentPrice"].ToString());
		tmp.diamondPercentPrice	= int.Parse (instance["diamondPercentPrice"].ToString());
		tmp.hostDeal		= int.Parse (instance["hotDeal"].ToString()) == 1;
		return tmp;
	}

}
