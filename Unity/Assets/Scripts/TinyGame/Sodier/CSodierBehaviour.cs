using UnityEngine;
using System;
using System.Collections;
using FSM;

public enum ESodierAnim: byte {
	Idle = 0,
	Move = 1,
	Attack = 2,
	Hit = 3,
	Death = 4
}

public class CSodierBehaviour : CMapObjectBehaviour {

	#region Properties

	[SerializeField]	private Animator m_Animator;
	[SerializeField]	private BoxCollider2D m_Collider2D;
	[SerializeField]	private ETeam m_Team = ETeam.Neutrol;
	[SerializeField]	private CSodierBehaviour m_Enemy;
	[SerializeField]	private CSodierData m_Data;

	// TEST
	public int currentAttackSpeed;

	public string m_StateName = string.Empty;

	public Action OnAttack;
	public Action OnEndTurn;
	public Action OnDeath;

	protected FSMManager m_FSMManager;
	protected CTest9Manager m_GameManager;
	protected HealthComponent m_HealthComponent;
	protected Movable2DComponent m_Movable2DComponent;

	protected CSkillBehaviour m_NormalSkill;

	private float m_FakeAttackTime = 1f;

	#endregion

	#region Implementation Monobehaviour

	protected override void Awake ()
	{
		base.Awake ();
		if (m_Animator == null) {
			m_Animator = this.GetComponent<Animator> ();
		}
		m_FSMManager = new FSMManager ();
		m_FSMManager.LoadFSM ("Data/Test9/FSM/SodierFSM");
		m_GameManager = CTest9Manager.GetInstance ();
		m_HealthComponent = new HealthComponent ();
		m_Movable2DComponent = new Movable2DComponent ();
	}

	public override void Init() {
		base.Init ();
		m_Movable2DComponent.SetSpeed (1.5f);
		m_Movable2DComponent.SetStartPosition (GetPosition ());
		m_FakeAttackTime = m_Data.attackSpeed;
	}

	protected override void Start ()
	{
		base.Start ();

		m_FSMManager.RegisterState ("IdleState", new FSMSodierIdleState (this));
		m_FSMManager.RegisterState ("MoveState", new FSMSodierMoveState (this));
		m_FSMManager.RegisterState ("AttackState", new FSMSodierAttackState (this));
		m_FSMManager.RegisterState ("BackState", new FSMSodierBackState (this));
		m_FSMManager.RegisterState ("DeathState", new FSMSodierDeathState (this));

		m_FSMManager.RegisterCondition ("HaveEnemy", this.HaveEnemy);
		m_FSMManager.RegisterCondition ("DidMoveToPosition", this.DidMoveToPosition);
		m_FSMManager.RegisterCondition ("DidAttackFinish", this.DidAttackFinish);
		m_FSMManager.RegisterCondition ("IsDeath", this.IsDeath);
	}

	protected override void UpdateBaseTime (float dt)
	{
		base.UpdateBaseTime (dt);
		m_FSMManager.UpdateState ();
		m_StateName = m_FSMManager.currentStateName;

		m_HealthComponent.UpdateComponent (Time.deltaTime);
		m_Movable2DComponent.UpdateComponent (Time.deltaTime);

		var health = 0;
		if (m_HealthComponent.Calculate (GetHealth (), out health)) {
			SetHealth (health);
		}
	}

	#endregion

	#region Main methods

	public virtual void ApplyDamage(int value, CSodierBehaviour enemy) {
		if (m_Enemy != null) {
			SetEnemy (enemy);
		}
		m_HealthComponent.ApplyDamage (value);
	}

	public virtual void ApplyBuff(int value, CSodierBehaviour buffer) {
		m_HealthComponent.ApplyBuff (value);
	}

	public virtual void ApplyDamageToEnemy() {
		if (m_Enemy != null) {
			m_NormalSkill = m_GameManager.GetSkillPool (m_Data.skillSlots [0]);
			if (m_NormalSkill != null) {
				m_NormalSkill.SetActive (true);
				m_NormalSkill.SetEnemy (m_Enemy);
				m_NormalSkill.SetOwner (this);
				m_NormalSkill.SetPosition (GetPosition ());
			}
			var totalDamage = 1;
			switch (m_NormalSkill.GetSkillEffect()) {
			default:
			case ESkillEffect.Health:
				totalDamage = m_Data.damage + m_NormalSkill.GetSkillValue ();
				break;
			case ESkillEffect.Mana:
			case ESkillEffect.Rage:
				totalDamage = m_NormalSkill.GetSkillValue ();
				break;
			}
			m_Enemy.ApplyDamage (totalDamage, this);
//			m_HealthComponent.ApplyDamage (m_NormalSkill.GetCostHealth());
		}
	}

	public virtual void MoveToPosition(Vector3 position) {
		m_Movable2DComponent.SetTargetPosition (position);
		var newPosition = Vector3.zero;
		if (m_Movable2DComponent.Calculate (GetPosition (), out newPosition)) {
			SetPosition (newPosition);
		}
	}

	public virtual void MoveToEnemy() {
		m_Movable2DComponent.SetDistance (m_Data.attackRange);
		MoveToPosition (m_Enemy.GetPosition ());
	}

	public virtual void MoveToStartPoint() {
		m_Movable2DComponent.SetDistance (0.0005f);
		MoveToPosition (m_Movable2DComponent.GetStartPosition());
	}

	public virtual void EndTurn() {
		m_FakeAttackTime = m_Data.attackSpeed;
		SetEnemy (null);
		if (GetActive() && OnEndTurn != null) {
			OnEndTurn ();
		}
	}

	public virtual void Reset() {
		
	}

	#endregion

	#region FSM Condition

	internal bool DidMoveToPosition() {
		return m_Movable2DComponent.Finish(GetPosition());
	}

	internal bool DidAttackFinish() {
		if (m_NormalSkill != null) {
			return !m_NormalSkill.GetActive();
		} else {
			m_FakeAttackTime -= Time.deltaTime;
			return m_FakeAttackTime <= 0f;
		}
		return true;
	}

	internal bool HaveEnemy() {
		return m_Enemy != null && m_Enemy.GetHealth() > 0;
	}

	internal bool IsDeath() {
		return m_Data.currentHealth <= 0f;
	}

	#endregion

	#region Getter && Setter

	public virtual void SetData(CBaseData data) {
		m_Data = data as CSodierData;
		currentAttackSpeed = m_Data.attackSpeed;
	}

	public virtual void SetEnemy(CSodierBehaviour enemy) {
		m_Enemy = enemy;
		if (enemy != null) {
			m_Movable2DComponent.SetTargetPosition (enemy.GetPosition ());
		}
	}

	public virtual CSodierBehaviour GetEnemy() {
		return m_Enemy;
	}

	public virtual void SetAnimationBaseTeam(ESodierAnim anim) {
		SetAnimation (anim, m_Team == ETeam.Ally);
	}

	public virtual void SetAnimation(ESodierAnim anim, bool side) {
		m_Animator.SetBool ("UpDown", side);
		m_Animator.SetInteger ("AnimParam", (int) anim);
	}

	public virtual void SetTeam(ETeam team) {
		m_Team = team;
		SetAnimation (ESodierAnim.Idle, m_Team == ETeam.Ally);
	}

	public virtual ETeam GetTeam() {
		return m_Team;
	}

	public override void SetPosition(Vector3 value) {
		// base.SetPosition(position);
		m_Transform.position = m_Movable2DComponent.GetCurrentPosition (value);
	}

	public override Vector3 GetPosition() {
		return base.GetPosition ();
	}

	public virtual int GetHealth() {
		return m_Data.currentHealth;
	}

	public virtual void SetHealth(int value) {
		m_Data.currentHealth = value;
	}

	public virtual int GetDamage() {
		return m_Data.damage;
	}

	public virtual int GetAttackSpeed() {
		return m_Data.attackSpeed;
	}

	public virtual string GetGameType() {
		return m_Data.gameType;
	}

	public AnimationClip GetAnimationClipByName(string name) {
		var animationClips = m_Animator.runtimeAnimatorController.animationClips;
		for (int i = 0; i < animationClips.Length; i++) {
			if (animationClips [i].name == name) {
				return animationClips [i];
			}
		}
		return null;
	}

	#endregion

}
