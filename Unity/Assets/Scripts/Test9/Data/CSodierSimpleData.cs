using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSodierSimpleData : CBaseData {

	public string name;
	public string modelPath;
	public string avatar;
	public string gameType;
	public int level;
	public float ability;

	public CSodierSimpleData () : base()
	{
		this.name 			= string.Empty;
		this.modelPath 		= string.Empty;
		this.avatar 		= string.Empty;
		this.gameType 		= string.Empty;
		this.level 			= 0;
		this.ability 		= 0f;
	}

	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData 		= instance as CSodierSimpleData;
		this.name 			= newData.name;
		this.modelPath 		= newData.modelPath;
		this.avatar 		= newData.avatar;
		this.gameType 		= newData.gameType;
		this.level 			= newData.level;
		this.ability 		= newData.ability;
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.id 			= instance["id"].ToString();
		this.name 			= instance["name"].ToString();
		this.modelPath 		= instance["modelPath"].ToString ();
		this.avatar			= instance["avatar"].ToString();
		this.gameType 		= instance["gameType"].ToString();
		this.level 			= int.Parse (instance ["level"].ToString ());
	}

	public static CSodierSimpleData Clone(CSodierSimpleData instance) {
		var tmp 			= new CSodierSimpleData ();
		tmp.id 				= instance.id;
		tmp.name 			= instance.name;
		tmp.modelPath 		= instance.modelPath;
		tmp.avatar 			= instance.avatar;
		tmp.gameType 		= instance.gameType;
		tmp.level 			= instance.level;
		tmp.ability 		= instance.ability;
		return tmp;
	}

	public static CSodierSimpleData Parse(Dictionary<string, object> instance) {
		var tmp 			= new CSodierSimpleData ();
		tmp.id 				= instance["id"].ToString();
		tmp.name 			= instance["name"].ToString();
		tmp.modelPath 		= instance ["modelPath"].ToString ();
		tmp.avatar 			= instance ["avatar"].ToString ();
		tmp.gameType		= instance ["gameType"].ToString ();
		tmp.level       	= int.Parse (instance ["level"].ToString ());
		return tmp;
	}

}
