using System;
using System.Collections;
using System.Collections.Generic;

public class CTurnData : CBaseData {

	public ETeam winner;
	public CTurnInfoData[] turns;

	public CTurnData () : base()
	{
		this.winner = ETeam.None;
		this.turns 	= null;
	}

	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData		= instance as CTurnData;
		this.id			= newData.id;
		this.winner		= newData.winner;
		this.turns 		= new CTurnInfoData[newData.turns.Length];
		newData.turns.CopyTo (this.turns, 0);
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.id			= instance["id"].ToString();
		this.winner		= (ETeam) int.Parse (instance["winner"].ToString());
		var listTurn 	= instance ["turns"] as List<object>;
		this.turns		= new CTurnInfoData[listTurn.Count];
		for (int i = 0; i < listTurn.Count; i++) {
			this.turns [i] = CTurnInfoData.Parse (listTurn[i] as Dictionary<string, object>);
		}
	}

	public static CTurnData Clone(CTurnData instance) {
		var tmp 		= new CTurnData ();
		tmp.id 			= instance.id;
		tmp.winner 		= instance.winner;
		tmp.turns 		= new CTurnInfoData[instance.turns.Length];
		instance.turns.CopyTo (tmp.turns, 0);
		return tmp;
	}

	public static CTurnData Parse(Dictionary<string, object> instance) {
		var tmp 		= new CTurnData ();
		tmp.id 			= instance["id"].ToString();
		tmp.winner		= (ETeam) int.Parse (instance["winner"].ToString());
		var listTurn 	= instance ["turns"] as List<object>;
		tmp.turns		= new CTurnInfoData[listTurn.Count];
		for (int i = 0; i < listTurn.Count; i++) {
			tmp.turns [i] = CTurnInfoData.Parse (listTurn[i] as Dictionary<string, object>);
		}
		return tmp;
	}

}
