using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class CTurnInfoData : CBaseData {

	public int attacker;
	public int target;
	public EGameType[] skills;

	public CTurnInfoData () : base()
	{
		this.attacker = -1;
		this.target = -1;
		this.skills = null;
	}

	public override string ToString ()
	{
		var result = new StringBuilder ("{");
		var currentSkills = new StringBuilder ("[");
		for (int i = 0; i < skills.Length; i++) {
			currentSkills.Append ((int)skills[i]);
			if (i < skills.Length - 1) {
				currentSkills.Append (",");
			}
		}
		currentSkills.Append ("]");
		result.AppendFormat ("\"id\": {0},\"attacker\": {1},\"target\": {2},\"skills\":{3}", id, attacker, target, currentSkills.ToString());
		result.Append ("}");
		return result.ToString();
	}

	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData		= instance as CTurnInfoData;
		this.attacker 	= newData.attacker;
		this.target 	= newData.target;
		this.skills		= new EGameType[newData.skills.Length];
		newData.skills.CopyTo (this.skills, 0);
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.attacker 	= int.Parse (instance["attacker"].ToString());
		this.target 	= int.Parse (instance["target"].ToString());
		this.skills	 	= ConvertListObjectToEGameTypeArray(instance["skills"] as List<object>);
	}

	public static CTurnInfoData Clone(CTurnInfoData instance) {
		var tmp 		= new CTurnInfoData ();
		tmp.attacker 	= instance.attacker;
		tmp.target 		= instance.target;
		tmp.skills		= new EGameType[instance.skills.Length];
		instance.skills.CopyTo (tmp.skills, 0);
		return tmp;
	}

	public static CTurnInfoData Parse(Dictionary<string, object> instance) {
		var tmp 		= new CTurnInfoData ();
		tmp.attacker 	= int.Parse (instance["attacker"].ToString());
		tmp.target 		= int.Parse (instance["target"].ToString());
		tmp.skills	 	= ConvertListObjectToEGameTypeArray(instance["skills"] as List<object>);
		return tmp;
	}
}
