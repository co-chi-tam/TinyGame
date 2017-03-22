using UnityEngine;
using System.Collections;
using FSM;

public class FSMSkillMoveState : Test9BaseState {

	private CSkillBehaviour m_Skill;

	public FSMSkillMoveState (IContext context) : base (context)
	{
		m_Skill = context as CSkillBehaviour;
	}

	public override void StartState ()
	{
		base.StartState ();
		if (m_Skill.OnMove != null) {
			m_Skill.OnMove ();
		}
		m_Skill.SetAnimation (ESkillAnim.Move);
	}

	public override void UpdateState ()
	{
		base.UpdateState ();
		m_Skill.MoveToEnemy ();
	}

	public override void ExitState ()
	{
		base.ExitState ();
	}

}
