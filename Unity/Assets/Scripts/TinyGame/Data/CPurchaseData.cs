using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPurchaseData : CBaseData {

	public int currentGold;
	public int currrentDiamond;
	public string productName;
	public int amount;
	public int totalGold;
	public int totalDiamond;
	public string token;

	public CPurchaseData () : base()
	{
		this.currentGold 				= 0;
		this.currrentDiamond 			= 0;
		this.productName 		= string.Empty;
		this.amount 			= 0;
		this.totalGold 		= 0;
		this.totalDiamond 	= 0;
		this.token 				= string.Empty;
	}


	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData				= instance as CPurchaseData;
		this.id					= newData.id;
		this.currentGold 				= newData.currentGold;
		this.currrentDiamond 			= newData.currrentDiamond;
		this.productName 		= newData.productName;
		this.amount 			= newData.amount;
		this.totalGold 			= newData.totalGold;
		this.totalDiamond 		= newData.totalDiamond;
		this.token 				= newData.token;
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.id					= instance["id"].ToString();
		this.currentGold				= int.Parse (instance["currentGold"].ToString());
		this.currrentDiamond 			= int.Parse (instance["currentDiamond"].ToString());
		this.productName 		= instance["productName"].ToString();
		this.amount				= int.Parse (instance["amount"].ToString());
		this.totalGold			= int.Parse (instance["totalGold"].ToString());
		this.totalDiamond 		= int.Parse (instance["totalDiamond"].ToString());
		this.token				= instance["token"].ToString();
	}

	public static CPurchaseData Clone(CPurchaseData instance) {
		var tmp 				= new CPurchaseData ();
		tmp.id 					= instance.id;
		tmp.currentGold 				= instance.currentGold;
		tmp.currrentDiamond 			= instance.currrentDiamond;
		tmp.productName 		= instance.productName;
		tmp.amount 				= instance.amount;
		tmp.totalGold 			= instance.totalGold;
		tmp.totalDiamond 		= instance.totalDiamond;
		tmp.token 				= instance.token;
		return tmp;
	}

	public static CPurchaseData Parse(Dictionary<string, object> instance) {
		var tmp 				= new CPurchaseData ();
		tmp.id 					= instance["id"].ToString();
		tmp.currentGold				= int.Parse (instance["gold"].ToString());
		tmp.currrentDiamond 			= int.Parse (instance["diamond"].ToString());
		tmp.productName 		= instance["productName"].ToString();
		tmp.amount				= int.Parse (instance["amount"].ToString());
		tmp.totalGold			= int.Parse (instance["totalGold"].ToString());
		tmp.totalDiamond 		= int.Parse (instance["totalDiamond"].ToString());
		tmp.token				= instance["token"].ToString();
		return tmp;
	}

}
