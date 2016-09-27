using System;
using System.Collections;
using System.Collections.Generic;

public class CLevelUpResponseData : CBaseData {

	public bool result;
	public string token;

	public CLevelUpResponseData () : base ()
	{
		this.result = false;
		this.token = string.Empty;
	}

	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData		= instance as CLevelUpResponseData;
		this.id			= newData.id;
		this.result 	= newData.result;
		this.token 		= newData.token;
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.id			= instance["id"].ToString();
		this.result		= int.Parse (instance["result"].ToString()) == 1;
		this.token		= instance["token"].ToString();
	}

	public static CLevelUpResponseData Clone(CLevelUpResponseData instance) {
		var tmp 		= new CLevelUpResponseData ();
		tmp.id 			= instance.id;
		tmp.result		= instance.result;
		tmp.token 		= instance.token;
		return tmp;
	}

	public static CLevelUpResponseData Parse(Dictionary<string, object> instance) {
		var tmp 		= new CLevelUpResponseData ();
		tmp.id 			= instance["id"].ToString();
		tmp.result		= int.Parse (instance["result"].ToString()) == 1;
		tmp.token		= instance["token"].ToString();
		return tmp;
	}
}
