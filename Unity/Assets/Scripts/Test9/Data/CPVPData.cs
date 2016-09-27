using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CPVPData : CBaseData {

	public CUserConnectData[] connects;

	public CPVPData () : base()
	{
		this.connects = null;
	}

	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData			= instance as CPVPData;
		this.id				= newData.id;
		this.connects		= new CUserConnectData[newData.connects.Length];
		newData.connects.CopyTo (this.connects, 0);
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.id				= instance["id"].ToString();
		var users			= instance["connects"] as List<object>;
		this.connects		= new CUserConnectData[users.Count];
		for (int i = 0; i < users.Count; i++) {
			var slot = users [i] as Dictionary<string, object>;
			this.connects [i] = CUserConnectData.Parse (slot);
		}
		var mapIds = instance["mapIds"] as List<object>;
		for (int i = 0; i < mapIds.Count; i++) {
			var slot = mapIds [i] as Dictionary<string, object>;
			var mapName = slot["mapName"].ToString();
			var mapId 	= slot["id"].ToString();	
			for (int j = 0; j < this.connects.Length; j++) {
				if (("mName-" + this.connects [j].user) == mapName) {
					this.connects [j].mapId = mapId;
				}
			}
		}
	}

	public static CPVPData Clone(CPVPData instance) {
		var tmp 			= new CPVPData ();
		tmp.id 				= instance.id;
		tmp.connects		= new CUserConnectData[instance.connects.Length];
		instance.connects.CopyTo (tmp.connects, 0);
		return tmp;
	}

	public static CPVPData Parse(Dictionary<string, object> instance) {
		var tmp 			= new CPVPData ();
		tmp.id 				= instance["id"].ToString();
		var users			= instance["connects"] as List<object>;
		tmp.connects		= new CUserConnectData[users.Count];
		for (int i = 0; i < users.Count; i++) {
			var slot = users [i] as Dictionary<string, object>;
			tmp.connects [i] = CUserConnectData.Parse (slot);
		}
		var mapIds 			= instance["mapIds"] as List<object>;
		for (int i = 0; i < mapIds.Count; i++) {
			var slot = mapIds [i] as Dictionary<string, object>;
			var mapName = slot["mapName"].ToString();
			var mapId 	= slot["id"].ToString();	
			for (int j = 0; j < tmp.connects.Length; j++) {
				if (("mName-" + tmp.connects [j].user) == mapName) {
					tmp.connects [j].mapId = mapId;
				}
			}
		}
		return tmp;
	}

}
