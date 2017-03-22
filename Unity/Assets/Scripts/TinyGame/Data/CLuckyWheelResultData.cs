using System;
using System.Collections;
using System.Collections.Generic;

public class CLuckyWheelResultData : CBaseData {

	public string result;
	public int amount;
	public int nextTimes;

	public CLuckyWheelResultData () : base()
	{
		this.result 	= string.Empty;
		this.amount 	= 0;
		this.nextTimes 	= 0;
	}

	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData		= instance as CLuckyWheelResultData;
		this.id			= newData.id;
		this.result 	= newData.result;
		this.amount 	= newData.amount;
		this.nextTimes 	= newData.nextTimes;
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.id			= instance["id"].ToString();
		this.result		= instance["result"].ToString();
		this.amount		= int.Parse (instance["amount"].ToString());
		this.nextTimes 	= int.Parse (instance["nextTimes"].ToString());
	}

	public static CLuckyWheelResultData Clone(CLuckyWheelResultData instance) {
		var tmp 		= new CLuckyWheelResultData ();
		tmp.id 			= instance.id;
		tmp.result 		= instance.result;
		tmp.amount 		= instance.amount;
		tmp.nextTimes 	= instance.nextTimes;
		return tmp;
	}

	public static CLuckyWheelResultData Parse(Dictionary<string, object> instance) {
		var tmp 		= new CLuckyWheelResultData ();
		tmp.id 			= instance["id"].ToString();
		tmp.result		= instance["result"].ToString();
		tmp.amount		= int.Parse (instance["amount"].ToString());
		tmp.nextTimes 	= int.Parse (instance["nextTimes"].ToString());
		return tmp;
	}

}
