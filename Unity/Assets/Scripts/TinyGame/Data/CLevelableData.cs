using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CLevelableData : CBaseData {

	public int currentHealth;
	public int maxHealth;
	public int currentMana;
	public int maxMana;
	public int currentRage;
	public int maxRage;
	public int damage;
	public int defend;
	public float moveSpeed;
	public int attackSpeed;
	public float attackRange;

	public CLevelableData () : base()
	{
		this.currentHealth = 0;
		this.maxHealth 	= 0;
		this.currentMana = 0;
		this.maxMana 	= 0;
		this.currentRage = 0;
		this.maxRage 	= 0;
		this.damage 	= 0;
		this.defend 	= 0;
		this.moveSpeed 	= 0f;
		this.attackSpeed = 0;
		this.attackRange = 0f;
	}

	public override void CloneData (CBaseData instance)
	{
		base.CloneData (instance);
		var newData 		= instance as CLevelableData;
		this.id 			= newData.id;
		this.currentHealth 	= newData.currentHealth;
		this.maxHealth 		= newData.maxHealth;
		this.currentMana 	= newData.currentMana;
		this.maxMana		= newData.maxMana;
		this.currentRage	= newData.currentRage;
		this.maxRage		= newData.maxRage;
		this.damage 		= newData.damage;
		this.defend 		= newData.defend;
		this.moveSpeed 		= newData.moveSpeed;
		this.attackSpeed 	= newData.attackSpeed;
		this.attackRange 	= newData.attackRange;
	}

	public override void ParseData (Dictionary<string, object> instance) {
		base.ParseData (instance);
		this.id 			= instance["id"].ToString();
		this.currentHealth 	= int.Parse (instance["currentHealth"].ToString());
		this.maxHealth 		= int.Parse (instance["maxHealth"].ToString());
		this.currentMana	= int.Parse (instance["currentMana"].ToString());
		this.maxMana		= int.Parse (instance["maxMana"].ToString());
		this.currentRage	= int.Parse (instance["currentRage"].ToString());
		this.maxRage		= int.Parse (instance["maxRage"].ToString());
		this.damage 		= int.Parse (instance["damage"].ToString());
		this.defend 		= int.Parse (instance["defend"].ToString());
		this.moveSpeed 		= float.Parse (instance["moveSpeed"].ToString());
		this.attackSpeed 	= int.Parse (instance["attackSpeed"].ToString());
		this.attackRange 	= float.Parse (instance["attackRange"].ToString());
	}

	public virtual void IncreaseData(CLevelableData instance) {
		this.currentHealth 	+= instance.currentHealth;
		this.maxHealth 		+= instance.maxHealth;
		this.currentMana 	+= instance.currentMana;
		this.maxMana		+= instance.maxMana;
		this.currentRage	+= instance.currentRage;
		this.maxRage		+= instance.maxRage;
		this.damage 		+= instance.damage;
		this.defend 		+= instance.defend;
		this.moveSpeed 		+= instance.moveSpeed;
		this.attackSpeed 	+= instance.attackSpeed;
		this.attackRange 	+= instance.attackRange;
	}

	public static CLevelableData operator *(CLevelableData instance, int multi) {
		var value = CLevelableData.Clone (instance);
		value.currentHealth 	*= multi;
		value.maxHealth 		*= multi;
		value.currentMana 		*= multi;
		value.maxMana			*= multi;
		value.currentRage		*= multi;
		value.maxRage			*= multi;
		value.damage 			*= multi;
		value.defend 			*= multi;
		value.moveSpeed 		*= multi;
		value.attackSpeed 		*= multi;
		value.attackRange 		*= multi;
		return value;
	}

	public static CLevelableData Clone(CLevelableData instance) {
		var tmp 			= new CLevelableData ();
		tmp.id 				= instance.id;
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
		return tmp;
	}

	public static CLevelableData Parse(Dictionary<string, object> instance) {
		var tmp 			= new CLevelableData ();
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
		return tmp;
	}
}
