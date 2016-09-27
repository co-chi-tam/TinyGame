using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CUserConnectData : CBaseData {

	public string user;
	public string avatar;
	public string mapId;

	public CUserConnectData () : base()
	{
		this.user = string.Empty;
		this.avatar = string.Empty;
		this.mapId = string.Empty;
	}

	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData			= instance as CUserConnectData;
		this.id				= newData.id;
		this.user 			= newData.user;
		this.avatar 		= newData.avatar;
		this.mapId 			= newData.mapId;
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.id				= instance["id"].ToString();
		this.user			= instance["user"].ToString();
		this.avatar 		= instance ["avatar"].ToString ();
	}

	public static CUserConnectData Clone(CUserConnectData instance) {
		var tmp 			= new CUserConnectData ();
		tmp.id 				= instance.id;
		tmp.user			= instance.user;
		tmp.avatar 			= instance.avatar;
		tmp.mapId			= instance.mapId;
		return tmp;
	}

	public static CUserConnectData Parse(Dictionary<string, object> instance) {
		var tmp 			= new CUserConnectData ();
		tmp.id 				= instance["id"].ToString();
		tmp.user			= instance["user"].ToString();
		tmp.avatar 			= instance ["avatar"].ToString ();
		return tmp;
	}

}
