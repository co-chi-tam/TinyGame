using UnityEngine;
using System;
using System.Collections;
using FSM;

public enum ESkillAnim: byte {
	Idle = 0,
	Move = 1,
	Attack = 2,
	Stay = 3,
	Destroy = 4
}

public class CSkillBehaviour : CMapObjectBehaviour, IAnimatorEvent {

	[SerializeField]	private Animator m_Animator;
	[SerializeField]	private CMapObjectBehaviour m_Owner;
	[SerializeField]	private CMapObjectBehaviour m_Enemy;
	[SerializeField]	private CSkillData m_Data;

	public string m_StateName = string.Empty;
	public EState m_State = EState.StartState;

	protected FSMManager m_FSMManager;
	protected Movable2DComponent m_Movable2DComponent;
	private ESkillAnim m_CurrentAnimation;
	private bool m_EndAnimation = false;

	public Action OnIdle;
	public Action OnAttack;
	public Action OnMove;
	public Action OnDestroy;
	public Action OnEndAnimation;

	protected override void Awake ()
	{
		base.Awake ();
		if (m_Animator == null) {
			m_Animator = this.GetComponent<Animator> ();
		}
		m_FSMManager = new FSMManager ();
		m_Movable2DComponent = new Movable2DComponent ();
	}

	public override void Init() {
		base.Init ();
		m_Movable2DComponent.SetSpeed (1.5f);
		m_Movable2DComponent.SetStartPosition (GetPosition ());
		m_FSMManager.LoadFSM (m_Data.FSMPath);
	}

	protected override void Start ()
	{
		base.Start ();

		m_FSMManager.RegisterState ("SkillIdleState", new FSMSkillIdleState (this));
		m_FSMManager.RegisterState ("SkillMoveState", new FSMSkillMoveState (this));
		m_FSMManager.RegisterState ("SkillStayOwnerState", new FSMSkillStayOwnerState (this));
		m_FSMManager.RegisterState ("SkillStayEnemyState", new FSMSkillStayEnemyState (this));
		m_FSMManager.RegisterState ("SkillAttackState", new FSMSkillAttackState (this));
		m_FSMManager.RegisterState ("SkillDestroyState", new FSMSkillDestroyState (this));

		m_FSMManager.RegisterCondition ("HaveEnemy", this.HaveEnemy);
		m_FSMManager.RegisterCondition ("DidMoveToPosition", this.DidMoveToPosition);
		m_FSMManager.RegisterCondition ("DidMoveToEnemy", this.DidMoveToEnemy);
		m_FSMManager.RegisterCondition ("DidAttackFinish", this.DidAttackFinish);
		m_FSMManager.RegisterCondition ("IsDestroy", this.IsDestroy);
	}

	protected override void UpdateBaseTime (float dt)
	{
		base.UpdateBaseTime (dt);
		m_FSMManager.UpdateState ();
		m_State = m_FSMManager.currentState;
		m_StateName = m_FSMManager.currentStateName;
		m_Movable2DComponent.UpdateComponent (dt);
	}

	#region Main methods

	public virtual void StayOwner() {
		if (m_Owner != null) {
			SetPosition (m_Owner.GetPosition ());
		}
	}

	public virtual void StayEnemy() {
		if (m_Enemy != null) {
			SetPosition (m_Enemy.GetPosition ());
		}
	}

	public virtual void ApplyDamageToEnemy() {
		
	}

	public virtual void MoveToPosition(Vector3 position) {
		m_Movable2DComponent.SetTargetPosition (position);
		var newPosition = Vector3.zero;
		if (m_Movable2DComponent.Calculate (GetPosition (), out newPosition)) {
			SetPosition (newPosition);
		}
	}

	public virtual void MoveToEnemy() {
		m_Movable2DComponent.SetDistance (0.01f);
		MoveToPosition (m_Enemy.GetPosition ());
	}

	public virtual void EndTurn() {
		
	}

	public virtual void Reset() {
		m_Enemy = null;
		SetActive (false);
		OnIdle 		= null;
		OnAttack 	= null;
		OnMove 		= null;
		OnDestroy 	= null;
	}

	public virtual void DidEndAnimation(string param) {
		if (OnEndAnimation != null) {
			OnEndAnimation ();
		}
		m_EndAnimation = true;
	}

	#endregion

	#region FSM Condition

	internal bool DidMoveToPosition() {
		return m_Movable2DComponent.Finish(GetPosition());
	}

	internal bool DidMoveToEnemy() {
		if (m_Enemy == null)
			return true;
		return m_Movable2DComponent.Finish(GetPosition());
	}

	internal bool DidAttackFinish() {
		return m_EndAnimation == true && m_CurrentAnimation == ESkillAnim.Attack;
	}

	internal bool HaveEnemy() {
		return m_Enemy != null;
	}

	internal bool IsDestroy() {
		return m_Enemy == null;
	}

	#endregion

	#region Getter && Setter

	public override void SetActive (bool value)
	{
		base.SetActive (value);
		if (value) {
			m_FSMManager.SetState ("SkillIdleState");
		}
	}

	public virtual void SetAnimation(ESkillAnim anim) {
		m_Animator.SetInteger ("AnimParam", (int)anim);
		m_CurrentAnimation = anim;
		m_EndAnimation = false;
	}

	public override void SetData(CBaseData data) {
		base.SetData (data);
		m_Data = data as CSkillData;
	}

	public virtual void SetEnemy(CSodierBehaviour enemy) {
		m_Enemy = enemy;
		if (enemy != null) {
			m_Movable2DComponent.SetTargetPosition (enemy.GetPosition ());
		}
	}

	public virtual void SetOwner(CMapObjectBehaviour owner) {
		m_Owner = owner;
		if (owner != null) {
			m_Movable2DComponent.SetStartPosition (owner.GetPosition());
		}
	}

	public virtual CMapObjectBehaviour GetEnemy() {
		return m_Enemy;
	}

	public virtual void SetTeam(ETeam team) {

	}

	public virtual ETeam GetTeam() {
		return ETeam.Neutrol;
	}

	public override void SetPosition(Vector3 value) {
		m_Transform.position = m_Movable2DComponent.GetCurrentPosition (value);
	}

	public override Vector3 GetPosition() {
		return base.GetPosition ();
	}

	public virtual string GetGameType() {
		return m_Data.gameType;
	}

	public virtual int GetSkillValue() {
		return m_Data.skillValue;
	}

	public virtual ESkillEffect GetSkillEffect() {
		return ESkillEffect.Health;
	}

	public virtual ESkillType GetSkillType() {
		return m_Data.skillType;
	}

	public virtual int GetCostHealth() {
		return m_Data.skillCostHealth;
	}

	public virtual int GetCostMana() {
		return m_Data.skillCostMana;
	}

	public virtual int GetCostRage() {
		return m_Data.skillCostRage;
	}

	#endregion

}
