using System;
using System.Collections;
using System.Collections.Generic;

public class CBaseData {

	public string id;

	public CBaseData ()
	{
		this.id = string.Empty;
	}

	public virtual void OnSerialize() {
		
	}

	public virtual void OnDeserialize() {
	
	}

	public virtual void ParseData (Dictionary<string, object> instance) {
		this.id		= instance["id"].ToString();
	}

	public virtual void CloneData(CBaseData instance) {
		this.id 	= instance.id;
	}

	public static CBaseData Clone(CBaseData instance) {
		var tmp		= new CBaseData ();
		tmp.id 		= instance.id;
		return tmp;
	}

	public static CBaseData Parse(Dictionary<string, object> instance) {
		var tmp		= new CBaseData ();
		tmp.id		= instance["id"].ToString();
		return tmp;
	}

	public static T[] ConvertListObjectToArray<T>(List<object> values) {
		var tmp = new T[values.Count];
		for (int i = 0; i < values.Count; i++) {
			tmp[i] = (T)Convert.ChangeType(values[i], typeof(T));
		}
		return tmp;
	}

	public static EGameType[] ConvertListObjectToEGameTypeArray(List<object> values) {
		var tmp = new EGameType[values.Count];
		for (int i = 0; i < values.Count; i++) {
			tmp[i] = (EGameType) int.Parse(values[i].ToString());
		}
		return tmp;
	}

}
