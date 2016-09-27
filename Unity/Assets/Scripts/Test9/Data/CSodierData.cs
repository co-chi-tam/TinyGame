using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CSodierData : CLevelableData {

	public string name;
	public string modelPath;
	public string avatar;
	public string gameType;
	public int level;
	public float ability;
	public Vector2 slotIds;
	public string[] skillSlots;
	public CLevelableData levelData;

	public CSodierData () : base()
	{
		this.name 			= string.Empty;
		this.modelPath 		= string.Empty;
		this.avatar 		= string.Empty;
		this.gameType 		= string.Empty;
		this.level 			= 0;
		this.ability 		= 0f;
		this.slotIds 		= Vector2.zero;
		this.skillSlots  	= null;
		this.levelData 		= null;
	}

	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData 		= instance as CSodierData;
		this.name 			= newData.name;
		this.modelPath 		= newData.modelPath;
		this.avatar 		= newData.avatar;
		this.gameType 		= newData.gameType;
		this.level 			= newData.level;
		this.ability 		= newData.ability;
		this.slotIds 		= newData.slotIds;
		this.skillSlots 	= new string[newData.skillSlots.Length];
		newData.skillSlots.CopyTo (this.skillSlots, 0);
		this.levelData 		= CLevelableData.Clone (newData.levelData);
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.id 			= instance["id"].ToString();
		this.name 			= instance["name"].ToString();
		this.modelPath 		= instance["modelPath"].ToString ();
		this.avatar			= instance["avatar"].ToString();
		this.gameType 		= instance["gameType"].ToString();
		this.level 			= int.Parse (instance ["level"].ToString ());
		this.skillSlots 	= ConvertListObjectToArray<string> (instance["skillSlots"] as List<object>);
		this.levelData 		= CLevelableData.Parse (instance["levelData"] as Dictionary<string, object>);
	}

	public static CSodierData Clone(CSodierData instance) {
		var tmp 			= new CSodierData ();
		tmp.id 				= instance.id;
		tmp.name 			= instance.name;
		tmp.modelPath 		= instance.modelPath;
		tmp.avatar 			= instance.avatar;
		tmp.gameType 		= instance.gameType;
		tmp.currentHealth 	= instance.currentHealth;
		tmp.maxHealth 		= instance.maxHealth;
		tmp.currentMana 	= instance.currentMana;
		tmp.maxMana			= instance.maxMana;
		tmp.currentRage		= instance.currentRage;
		tmp.maxRage			= instance.maxRage;
		tmp.damage 			= instance.damage;
		tmp.defend 			= instance.defend;
		tmp.moveSpeed 		= instance.moveSpeed;
		tmp.attackSpeed 	= instance.attackSpeed;
		tmp.attackRange 	= instance.attackRange;
		tmp.level 			= instance.level;
		tmp.ability 		= instance.ability;
		tmp.slotIds 		= instance.slotIds;
		tmp.skillSlots  	= new string[instance.skillSlots.Length];
		instance.skillSlots.CopyTo (tmp.skillSlots, 0);
		tmp.levelData 		= CLevelableData.Clone (instance.levelData);
		return tmp;
	}

	public static CSodierData Parse(Dictionary<string, object> instance) {
		var tmp 			= new CSodierData ();
		tmp.id 				= instance["id"].ToString();
		tmp.name 			= instance["name"].ToString();
		tmp.modelPath 		= instance ["modelPath"].ToString ();
		tmp.avatar 			= instance ["avatar"].ToString ();
		tmp.gameType		= instance ["gameType"].ToString ();
		tmp.currentHealth 	= int.Parse (instance["currentHealth"].ToString());
		tmp.maxHealth 		= int.Parse (instance["maxHealth"].ToString());
		tmp.currentMana		= int.Parse (instance["currentMana"].ToString());
		tmp.maxMana			= int.Parse (instance["maxMana"].ToString());
		tmp.currentRage		= int.Parse (instance["currentRage"].ToString());
		tmp.maxRage			= int.Parse (instance["maxRage"].ToString());
		tmp.damage 			= int.Parse (instance["damage"].ToString());
		tmp.defend 			= int.Parse (instance["defend"].ToString());
		tmp.moveSpeed 		= float.Parse (instance["moveSpeed"].ToString());
		tmp.attackSpeed 	= int.Parse (instance["attackSpeed"].ToString());
		tmp.attackRange 	= float.Parse (instance["attackRange"].ToString());
		tmp.level       	= int.Parse (instance ["level"].ToString ());
		tmp.skillSlots 		= ConvertListObjectToArray<string>(instance["skillSlots"] as List<object>);
		tmp.levelData 		= CLevelableData.Parse (instance["levelData"] as Dictionary<string, object>);
		return tmp;
	}

}
