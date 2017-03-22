using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSlotData : CBaseData {

	public string gameType;

	public CSlotData () : base()
	{
		this.gameType 	= string.Empty;
	}

	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData		= instance as CSlotData;
		this.id			= newData.id;
		this.gameType 	= newData.gameType;
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.id			= instance["id"].ToString();
		this.gameType	= instance["gameType"].ToString();
	}

	public static CSlotData Clone(CSlotData instance) {
		var tmp 		= new CSlotData ();
		tmp.id 			= instance.id;
		tmp.gameType 	= instance.gameType;
		return tmp;
	}

	public static CSlotData Parse(Dictionary<string, object> instance) {
		var tmp 		= new CSlotData ();
		tmp.id 			= instance["id"].ToString();
		tmp.gameType	= instance["gameType"].ToString();
		return tmp;
	}

}
