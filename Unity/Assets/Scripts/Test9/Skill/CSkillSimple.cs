using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSkillSimple {

	public int id;
	public string name;
	public string gameType;
	public int skillValue;
	public ESkillType skillType;
	public ESkillEffect skillEffect;
	public int costMana;
	public int costHealth;
	public int costRage;
	public List<Vector2> skillRange;

	public CSkillSimple ()
	{
		this.id = -1;
		this.name = string.Empty;
		this.skillValue = 0;
		this.skillType = ESkillType.None;
		this.skillEffect = ESkillEffect.None;
		this.costMana = 0;
		this.costHealth = 0;
		this.costRage = 0;
		this.skillRange = null;
	}

	public static CSkillSimple Clone(CSkillData instance) {
		var tmp = new CSkillSimple ();
		tmp.name = instance.name;
		tmp.gameType = instance.gameType;
		tmp.skillValue = instance.skillValue;
		tmp.skillType = instance.skillType;
		tmp.skillEffect = instance.skillEffect;
		tmp.costMana = instance.skillCostMana;
		tmp.costHealth = instance.skillCostHealth;
		tmp.costRage = instance.skillCostRage;
		tmp.skillRange = new List<Vector2> (instance.skillRange);
		return tmp;
	}

}
