using System;
using System.Collections;
using System.Collections.Generic;

public class CBattleAward : CBaseData {

	public bool result;
	public int goldAward;

	public CBattleAward () : base ()
	{
		this.result = false;
		this.goldAward = 0;
	}

	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData		= instance as CBattleAward;
		this.id			= newData.id;
		this.result 	= newData.result;
		this.goldAward = newData.goldAward;
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.id			= instance["id"].ToString();
		this.result		= int.Parse (instance["result"].ToString()) == 1;
		this.goldAward	= int.Parse (instance["goldAward"].ToString());
	}

	public static CBattleAward Clone(CBattleAward instance) {
		var tmp 		= new CBattleAward ();
		tmp.id 			= instance.id;
		tmp.result		= instance.result;
		tmp.goldAward 	= instance.goldAward;
		return tmp;
	}

	public static CBattleAward Parse(Dictionary<string, object> instance) {
		var tmp 		= new CBattleAward ();
		tmp.id 			= instance["id"].ToString();
		tmp.goldAward	= int.Parse (instance["goldAward"].ToString());
		tmp.result		= int.Parse (instance["result"].ToString()) == 1;
		return tmp;
	}

}
