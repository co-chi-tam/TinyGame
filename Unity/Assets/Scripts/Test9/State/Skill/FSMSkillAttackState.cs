using UnityEngine;
using System.Collections;
using FSM;

public class FSMSkillAttackState : Test9BaseState {

	private CSkillBehaviour m_Skill;

	public FSMSkillAttackState (IContext context) : base (context)
	{
		m_Skill = context as CSkillBehaviour;
	}

	public override void StartState ()
	{
		base.StartState ();
		if (m_Skill.OnAttack != null) {
			m_Skill.OnAttack ();
		}
		m_Skill.ApplyDamageToEnemy ();
		m_Skill.SetAnimation (ESkillAnim.Attack);
	}

	public override void UpdateState ()
	{
		base.UpdateState ();
	}

	public override void ExitState ()
	{
		base.ExitState ();
	}

}
