using UnityEngine;
using System.Collections;

public class CBaseController : MonoBehaviour {

	protected virtual void Awake() {
		
	}

	protected virtual void Start() {
		
	}

	protected virtual void ApplyDamage(int damage, CEnum.EAttackType damageType) {
		var totalDamage = damage;
		switch (damageType) {
		case CEnum.EAttackType.None:
			totalDamage = damage;
			break;
		case CEnum.EAttackType.Physic:
			totalDamage = damage - GetPhysicDefend();
			break;
		case CEnum.EAttackType.Magic:
			totalDamage = damage - GetMagicDefend();
			break;
		}
		var totalHealth = GetHealth ();
		totalHealth = totalHealth - totalDamage;
		SetHealth (totalHealth);
	}

	public virtual void SetAnimation(CEnum.EAnimation anim) {
		
	}

	public virtual CEnum.EAnimation GetAnimation() {
		return CEnum.EAnimation.Idle;
	}

	public virtual int GetPhysicDefend() {
		return 0;
	}

	public virtual int GetMagicDefend() {
		return 0;
	}

	public virtual int GetHealth() {
		return 0;
	}

	public virtual void SetHealth(int value) {
	
	}

}
