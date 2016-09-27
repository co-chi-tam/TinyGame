using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMapSlotData : CSlotData {

	public Vector2 slotIds;

	public CMapSlotData () : base()
	{
		this.slotIds 	= Vector2.zero;
	}

	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData		= instance as CMapSlotData;
		this.id			= newData.id;
		this.gameType 	= newData.gameType;
		this.slotIds	= newData.slotIds;
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.id			= instance["id"].ToString();
		this.gameType	= instance["gameType"].ToString();
		var slotTmp 	= instance ["slotIds"] as List<object>;
		this.slotIds	= new Vector2 (float.Parse (slotTmp[0].ToString()), float.Parse (slotTmp[1].ToString()));
	}

	public static CMapSlotData Clone(CMapSlotData instance) {
		var tmp 		= new CMapSlotData ();
		tmp.id 			= instance.id;
		tmp.gameType 	= instance.gameType;
		tmp.slotIds 	= instance.slotIds;
		return tmp;
	}

	public static CMapSlotData Parse(Dictionary<string, object> instance) {
		var tmp 		= new CMapSlotData ();
		tmp.id 			= instance["id"].ToString();
		tmp.gameType	= instance["gameType"].ToString();
		var slotTmp 	= instance ["slotIds"] as List<object>;
		tmp.slotIds	= new Vector2 (float.Parse (slotTmp[0].ToString()), float.Parse (slotTmp[1].ToString()));
		return tmp;
	}
}
