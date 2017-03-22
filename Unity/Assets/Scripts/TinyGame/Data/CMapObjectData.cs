using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMapObjectData : CBaseData {

	public string name;
	public string modelPath;

	public CMapObjectData () : base()
	{
		this.name 		= string.Empty;
		this.modelPath 	= string.Empty;
	}

	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData		= instance as CMapObjectData;
		this.id			= newData.id;
		this.name		= newData.name;
		this.modelPath	= newData.modelPath;
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.id			= instance["id"].ToString();
		this.name		= instance["name"].ToString();
		this.modelPath	= instance["modelPath"].ToString ();
	}

	public static CMapObjectData Clone(CMapObjectData instance) {
		var tmp 		= new CMapObjectData ();
		tmp.id 			= instance.id;
		tmp.name 		= instance.name;
		tmp.modelPath 	= instance.modelPath;
		return tmp;
	}

	public static CMapObjectData Parse(Dictionary<string, object> instance) {
		var tmp 		= new CMapObjectData ();
		tmp.id 			= instance["id"].ToString();
		tmp.name 		= instance["name"].ToString();
		tmp.modelPath 	= instance ["modelPath"].ToString ();
		return tmp;
	}

}
