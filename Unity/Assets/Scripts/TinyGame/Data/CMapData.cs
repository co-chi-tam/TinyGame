using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMapData : CBaseData {

	public string name;
	public string displayImage;
	public string backgroundImage;
	public CMapSlotData[] mapSlots;
	public Dictionary<string, CSodierData> sodierDatas;
	public Dictionary<string, CSkillData> skillDatas;
	public CSkillData[] skillValueDatas;

	public CMapData () : base()
	{
		this.name = string.Empty;
		this.displayImage = string.Empty;
		this.backgroundImage = string.Empty;
		this.mapSlots = null;
		this.sodierDatas = null;
		this.skillDatas = null;
		this.skillValueDatas = null;
	}

	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData		= instance as CMapData;
		this.id			= newData.id;
		this.name 		= newData.name;
		this.displayImage	= newData.displayImage;
		this.backgroundImage = newData.backgroundImage;
		this.mapSlots 		= new CMapSlotData[newData.mapSlots.Length];
		newData.mapSlots.CopyTo (this.mapSlots, 0);
		this.sodierDatas 	= new Dictionary<string, CSodierData> (newData.sodierDatas);
		this.skillDatas 	= new Dictionary<string, CSkillData> (newData.skillDatas);
		this.skillValueDatas = new CSkillData[newData.skillValueDatas.Length];
		newData.skillValueDatas.CopyTo (this.skillValueDatas, 0);
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.id			= instance["id"].ToString();
		this.name		= instance["name"].ToString();
		this.displayImage 	= instance["displayImage"].ToString();
		this.backgroundImage = instance ["bg"].ToString ();
		var slots 			= instance ["slots"] as List<object>;
		this.mapSlots 		= new CMapSlotData[slots.Count];
		for (int i = 0; i < slots.Count; i++) {
			var slotItem = slots [i] as Dictionary<string, object>;
			this.mapSlots [i] = CMapSlotData.Parse (slotItem);
		}
		var sodiers = instance ["sodiers"] as List<object>;
		this.sodierDatas	= new Dictionary<string, CSodierData> ();
		for (int i = 0; i < sodiers.Count; i++) {
			var sodierItem = sodiers [i] as Dictionary<string, object>;
			var sodierData = CSodierData.Parse (sodierItem); 
			if (this.sodierDatas.ContainsKey (sodierData.id) == false) {
				this.sodierDatas.Add (sodierData.id, sodierData);
			}
		}
		var skills = instance ["skills"] as List<object>;
		this.skillDatas	= new Dictionary<string, CSkillData> ();
		this.skillValueDatas = new CSkillData[skills.Count];
		for (int i = 0; i < skills.Count; i++) {
			var skillItems = skills [i] as Dictionary<string, object>;
			var skillData = CSkillData.Parse (skillItems); 
			if (this.skillDatas.ContainsKey (skillData.id) == false) {
				this.skillDatas.Add (skillData.id, skillData);
			}
			this.skillValueDatas [i] = skillData;
		}
	}

	public static CMapData Clone(CMapData instance) {
		var tmp 		= new CMapData ();
		tmp.id			= instance.id;
		tmp.name 		= instance.name;
		tmp.displayImage	= instance.displayImage;
		tmp.backgroundImage = instance.backgroundImage;
		tmp.mapSlots 		= new CMapSlotData[instance.mapSlots.Length];
		instance.mapSlots.CopyTo (tmp.mapSlots, 0);
		tmp.sodierDatas = new Dictionary<string, CSodierData> (instance.sodierDatas);
		tmp.skillDatas 	= new Dictionary<string, CSkillData> (instance.skillDatas);
		tmp.skillValueDatas = new CSkillData[instance.skillValueDatas.Length];
		instance.skillValueDatas.CopyTo (tmp.skillValueDatas, 0);
		return tmp;
	}

	public static CMapData Parse(Dictionary<string, object> instance) {
		var tmp 		= new CMapData ();
		tmp.id			= instance["id"].ToString();
		tmp.name		= instance["name"].ToString();
		tmp.displayImage 	= instance["displayImage"].ToString();
		tmp.backgroundImage = instance ["bg"].ToString ();
		var slots 			= instance ["slots"] as List<object>;
		tmp.mapSlots 		= new CMapSlotData[slots.Count];
		for (int i = 0; i < slots.Count; i++) {
			var slotItem = slots [i] as Dictionary<string, object>;
			tmp.mapSlots [i] = CMapSlotData.Parse (slotItem);
		}
		var sodiers = instance ["sodiers"] as List<object>;
		tmp.sodierDatas	= new Dictionary<string, CSodierData> ();
		for (int i = 0; i < sodiers.Count; i++) {
			var sodierItem = sodiers [i] as Dictionary<string, object>;
			var sodierData = CSodierData.Parse (sodierItem); 
			if (tmp.sodierDatas.ContainsKey (sodierData.id) == false) {
				tmp.sodierDatas.Add (sodierData.id, sodierData);
			}
		}
		var skills = instance ["skills"] as List<object>;
		tmp.skillDatas	= new Dictionary<string, CSkillData> ();
		tmp.skillValueDatas = new CSkillData[skills.Count];
		for (int i = 0; i < skills.Count; i++) {
			var skillItems = skills [i] as Dictionary<string, object>;
			var skillData = CSkillData.Parse (skillItems); 
			if (tmp.skillDatas.ContainsKey (skillData.id) == false) {
				tmp.skillDatas.Add (skillData.id, skillData);
			}
			tmp.skillValueDatas [i] = skillData;
		}
		return tmp;
	}

}
