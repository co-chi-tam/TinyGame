using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CSkillData : CBaseData {

	public string name;
	public string modelPath;
	public string FSMPath;
	public string gameType;
	public int skillValue;
	public ESkillType skillType;
	public ESkillEffect skillEffect;
	public int skillCostMana;
	public int skillCostHealth;
	public int skillCostRage;
	public List<Vector2> skillRange;

	public CSkillData () : base()
	{
		this.name 		= string.Empty;
		this.modelPath 	= string.Empty;
		this.gameType 	= string.Empty;
		this.FSMPath 	= string.Empty;
		this.skillValue = 0;
		this.skillType = ESkillType.None;
		this.skillEffect = ESkillEffect.None;
		this.skillCostMana = 0;
		this.skillCostHealth = 0;
		this.skillCostRage = 0;
		this.skillRange = null;
	}

	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData		= instance as CSkillData;
		this.id			= newData.id;
		this.name		= newData.name;
		this.modelPath	= newData.modelPath;
		this.gameType 	= newData.gameType;
		this.FSMPath 	= newData.FSMPath;
		this.skillValue = newData.skillValue;
		this.skillType = newData.skillType;
		this.skillEffect = newData.skillEffect;
		this.skillCostMana = newData.skillCostMana;
		this.skillCostHealth = newData.skillCostHealth;
		this.skillCostRage = newData.skillCostRage;
		this.skillRange = new List<Vector2> (newData.skillRange);
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.id			= instance["id"].ToString();
		this.name		= instance["name"].ToString();
		this.modelPath	= instance["modelPath"].ToString ();
		this.FSMPath = instance ["FSMPath"].ToString ();
		this.gameType	= instance["id"].ToString();
		this.skillValue	= int.Parse (instance["skillValue"].ToString());
		this.skillType = (ESkillType)int.Parse (instance["skillType"].ToString());
		this.skillEffect = (ESkillEffect)int.Parse (instance["skillEffect"].ToString());
		this.skillCostMana = int.Parse (instance["skillCostMana"].ToString());
		this.skillCostHealth = int.Parse (instance["skillCostHealth"].ToString());
		this.skillCostRage = int.Parse (instance["skillCostRage"].ToString());
		this.skillRange	= new List<Vector2> ();
		var skiRanges = instance ["skillRange"] as List<object>;
		for (int i = 0; i < skiRanges.Count; i++) {
			var listTmp = skiRanges [i] as List<object>;
			var vector2 = new Vector2 (float.Parse (listTmp[0].ToString()), float.Parse (listTmp[1].ToString()));
			this.skillRange.Add (vector2);
		}
	}

	public static CSkillData Clone(CSkillData instance) {
		var tmp 		= new CSkillData ();
		tmp.id 			= instance.id;
		tmp.name 		= instance.name;
		tmp.modelPath 	= instance.modelPath;
		tmp.gameType 	= instance.gameType;
		tmp.FSMPath 	= instance.FSMPath;
		tmp.skillValue	= instance.skillValue;
		tmp.skillType 	= instance.skillType;
		tmp.skillEffect = instance.skillEffect;
		tmp.skillCostMana = instance.skillCostMana;
		tmp.skillCostHealth = instance.skillCostHealth;
		tmp.skillCostRage = instance.skillCostRage;
		tmp.skillRange 	= new List<Vector2> (instance.skillRange);
		return tmp;
	}

	public static CSkillData Parse(Dictionary<string, object> instance) {
		var tmp 		= new CSkillData ();
		tmp.id 			= instance["id"].ToString();
		tmp.name 		= instance["name"].ToString();
		tmp.modelPath 	= instance ["modelPath"].ToString ();
		tmp.gameType	= instance["id"].ToString();
		tmp.FSMPath 	= instance["FSMPath"].ToString();
		tmp.skillValue	= int.Parse (instance["skillValue"].ToString());
		tmp.skillType 	= (ESkillType)int.Parse (instance["skillType"].ToString());
		tmp.skillEffect = (ESkillEffect)int.Parse (instance["skillEffect"].ToString());
		tmp.skillCostMana = int.Parse (instance["skillCostMana"].ToString());
		tmp.skillCostHealth = int.Parse (instance["skillCostHealth"].ToString());
		tmp.skillCostRage = int.Parse (instance["skillCostRage"].ToString());
		tmp.skillRange	= new List<Vector2> ();
		var skiRanges   = instance ["skillRange"] as List<object>;
		for (int i = 0; i < skiRanges.Count; i++) {
			var listTmp = skiRanges [i] as List<object>;
			var vector2 = new Vector2 (float.Parse (listTmp[0].ToString()), float.Parse (listTmp[1].ToString()));
			tmp.skillRange.Add (vector2);
		}
		return tmp;
	}

}
