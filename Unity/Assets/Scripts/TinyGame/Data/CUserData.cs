using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUserData : CBaseData {

	public string userName;
	public string userPassword;
	public string displayName;
	public string avartar;
	public bool active;
	public bool completeTutorial;
	public string secretKey;
	public int gold;
	public int diamond;
	public string power;
	public string token;
	public string currentMap;
	public string currentTarget;
	public string currentAwardCode;
	public string needUpdateAPI;

	public static string USER_TOKEN = "USER_TOKEN";
	public static string USER_NAME = "USER_NAME";
	public static string USER_PASSWORD = "USER_PASSWORD";
	public static string USER_LOGIN_STATUS = "USER_LOGIN_STATUS";


	public CUserData () : base()
	{
		this.userName 			= string.Empty;
		this.avartar 			= string.Empty;
		this.active 			= false;
		this.completeTutorial 	= false;
		this.displayName 		= string.Empty;
		this.secretKey 			= string.Empty;
		this.gold 				= 0;
		this.diamond 			= 0;
		this.power 				= string.Empty;
		this.token 				= string.Empty;
		this.currentMap 		= string.Empty;
		this.currentTarget 		= string.Empty;
		this.currentAwardCode	= string.Empty;
		this.needUpdateAPI		= string.Empty;
	}

	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData				= instance as CUserData;
		this.id					= newData.id;
		this.userName 			= newData.userName;
		this.userPassword		= newData.userPassword;
		this.displayName 		= newData.displayName;
		this.avartar 			= newData.avartar;
		this.active 			= newData.active;
		this.completeTutorial 	= newData.completeTutorial;
		this.secretKey 			= newData.secretKey;
		this.gold 				= newData.gold;
		this.diamond			= newData.diamond;
		this.power 				= newData.power;
		this.token				= newData.token;
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.id					= instance["id"].ToString();
		this.userName 			= instance ["userName"].ToString ();
		this.displayName 		= instance["displayName"].ToString();
		this.avartar 			= instance ["avatar"].ToString ();
		this.active 			= bool.Parse (instance["active"].ToString());
		this.completeTutorial 	= bool.Parse (instance["completeTutorial"].ToString());
		this.secretKey			= instance["secretKey"].ToString();
		this.gold 				= int.Parse (instance["gold"].ToString());
		this.diamond 			= int.Parse (instance["diamond"].ToString());
		this.power 				= instance ["power"].ToString ();
		this.token				= instance["token"].ToString ();
	}

	public static CUserData Clone(CUserData instance) {
		var tmp 				= new CUserData ();
		tmp.id					= instance.id;
		tmp.userName 			= instance.userName;
		tmp.userPassword		= instance.userPassword;
		tmp.displayName 		= instance.displayName;
		tmp.avartar 			= instance.avartar;
		tmp.active 				= instance.active;
		tmp.completeTutorial 	= instance.completeTutorial;
		tmp.secretKey 			= instance.secretKey;
		tmp.gold 				= instance.gold;
		tmp.diamond 			= instance.diamond;
		tmp.power 				= instance.power;
		tmp.token				= instance.token;
		return tmp;
	}

	public static CUserData Parse(Dictionary<string, object> instance) {
		var tmp 				= new CUserData ();
		tmp.id					= instance["id"].ToString();
		tmp.userName 			= instance ["userName"].ToString ();
		tmp.displayName 		= instance["displayName"].ToString();
		tmp.avartar 			= instance ["avatar"].ToString ();
		tmp.active 				= bool.Parse (instance["active"].ToString());
		tmp.completeTutorial 	= bool.Parse (instance["completeTutorial"].ToString());
		tmp.secretKey			= instance["secretKey"].ToString();
		tmp.gold 				= int.Parse (instance["gold"].ToString());
		tmp.diamond 			= int.Parse (instance["diamond"].ToString());
		tmp.power 				= instance ["power"].ToString ();
		tmp.token				= instance ["token"].ToString ();
		return tmp;
	}

}
