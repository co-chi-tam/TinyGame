using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CConsumeItemData : CBaseData {

	public string result;
	public int amount;

	public CConsumeItemData () : base()
	{
		this.result = string.Empty;
		this.amount = 0;
	}

	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData			= instance as CConsumeItemData;
		this.id				= newData.id;
		this.result 		= newData.result;
		this.amount 		= newData.amount;
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.id				= instance["id"].ToString();
		this.result			= instance["result"].ToString();
		this.amount			= int.Parse (instance["amount"].ToString());
	}

	public static CConsumeItemData Clone(CConsumeItemData instance) {
		var tmp 			= new CConsumeItemData ();
		tmp.id 				= instance.id;
		tmp.result 			= instance.result;
		tmp.amount 			= instance.amount;
		return tmp;
	}

	public static CConsumeItemData Parse(Dictionary<string, object> instance) {
		var tmp 			= new CConsumeItemData ();
		tmp.id 				= instance["id"].ToString();
		tmp.result			= instance["result"].ToString();
		tmp.amount			= int.Parse (instance["amount"].ToString());
		return tmp;
	}
}
