using UnityEngine;
using System.Collections;

public class CSimpleSodier {

	public int id;
	public string name;
	public string gameType;
	public int health;
	public int maxHealth;
	public int mana;
	public int maxMana;
	public int rage;
	public int maxRage;
	public int damage;
	public int attackSpeed;
	public int currentAttackSpeed;
	public Vector2 currentSlot;
	public CSkillSimple[] skillSlots;

	private HealthComponent m_HealthComponent;
	private HealthComponent m_ManaComponent;
	private HealthComponent m_RageComponent;

	public CSimpleSodier ()
	{
		this.id = -1;
		this.name = string.Empty;
		this.gameType = string.Empty;
		this.health = 0;
		this.maxHealth = 0;
		this.mana = 0;
		this.maxMana = 0;
		this.rage = 0;
		this.maxRage = 0;
		this.damage = 0;
		this.attackSpeed = 0;
		this.currentAttackSpeed = 0;
		this.currentSlot = Vector2.zero;
		this.skillSlots = null;

		m_HealthComponent = new HealthComponent ();
		m_ManaComponent = new HealthComponent ();
		m_RageComponent = new HealthComponent ();
	}

	public static CSimpleSodier Clone(CSodierData instance) {
		var tmp = new CSimpleSodier ();
		tmp.name = instance.name;
		tmp.gameType = instance.gameType;
		tmp.health = instance.currentHealth;
		tmp.maxHealth = instance.maxHealth;
		tmp.mana = instance.currentMana;
		tmp.maxMana = instance.maxMana;
		tmp.rage = instance.currentRage;
		tmp.maxRage = instance.maxRage;
		tmp.damage = instance.damage;
		tmp.attackSpeed = instance.attackSpeed;
		tmp.currentAttackSpeed = instance.attackSpeed;
		tmp.currentSlot = instance.slotIds;
		tmp.skillSlots = new CSkillSimple[instance.skillSlots.Length];
		return tmp;
	}

	public void ApplyDamage(int damageHealth, int damageMana, int damageRage) {
		m_HealthComponent.ApplyDamage (damageHealth);
		m_ManaComponent.ApplyDamage (damageMana);
		m_RageComponent.ApplyDamage (damageRage);
		CalculateStatus ();
	}

	public void ApplyBuff(int buffHealth, int buffMana, int buffRage) {
		m_HealthComponent.ApplyBuff (buffHealth);
		m_ManaComponent.ApplyBuff (buffMana);
		m_RageComponent.ApplyBuff (buffRage);
		CalculateStatus ();
	}

	public void ApplyCost(int costHealth, int costMana, int costRage) {
		ApplyDamage (costHealth, costMana, costRage);
	}

	private void CalculateStatus() {
		var totalHealth = 0;
		if (m_HealthComponent.Calculate (this.health, out totalHealth)) {
			this.health = totalHealth > this.maxHealth ? this.maxHealth : totalHealth;
		}

		var totalMana = 0;
		if (m_ManaComponent.Calculate (this.mana, out totalMana)) {
			this.mana = totalMana > this.maxMana ? this.maxMana : totalMana;
		}

		var totalRage = 0;
		if (m_RageComponent.Calculate (this.rage, out totalRage)) {
			this.rage = totalRage > this.maxRage ? this.maxRage : totalRage;
		}
	}

}
