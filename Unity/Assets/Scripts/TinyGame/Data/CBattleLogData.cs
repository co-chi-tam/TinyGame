using System;
using System.Collections;
using System.Collections.Generic;

public class CBattleLogData : CBaseData {

	public string awardCode;
	public bool result;

	public CBattleLogData () : base()
	{
		this.awardCode = string.Empty;
		this.result = false;
	}

	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData		= instance as CBattleLogData;
		this.id			= newData.id;
		this.awardCode = newData.awardCode;
		this.result 	= newData.result;
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.id			= instance["id"].ToString();
		this.awardCode	= instance["awardCode"].ToString();
		this.result		= int.Parse (instance["result"].ToString()) == 1;
	}

	public static CBattleLogData Clone(CBattleLogData instance) {
		var tmp 		= new CBattleLogData ();
		tmp.id 			= instance.id;
		tmp.awardCode 	= instance.awardCode;
		tmp.result		= instance.result;
		return tmp;
	}

	public static CBattleLogData Parse(Dictionary<string, object> instance) {
		var tmp 		= new CBattleLogData ();
		tmp.id 			= instance["id"].ToString();
		tmp.awardCode	= instance["awardCode"].ToString();
		tmp.result		= int.Parse (instance["result"].ToString()) == 1;
		return tmp;
	}


}
