using UnityEngine;
using System.Collections;
using FSM;

public class FSMSkillIdleState : Test9BaseState {

	private CSkillBehaviour m_Skill;

	public FSMSkillIdleState (IContext context) : base (context)
	{
		m_Skill = context as CSkillBehaviour;
	}

	public override void StartState ()
	{
		base.StartState ();
		if (m_Skill.OnIdle != null) {
			m_Skill.OnIdle ();
		}
		m_Skill.SetAnimation (ESkillAnim.Idle);
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
