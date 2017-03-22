using UnityEngine;
using System.Collections;
using FSM;

public class FSMSkillStayOwnerState : Test9BaseState {

	private CSkillBehaviour m_Skill;

	public FSMSkillStayOwnerState (IContext context) : base (context)
	{
		m_Skill = context as CSkillBehaviour;
	}

	public override void StartState ()
	{
		base.StartState ();
		if (m_Skill.OnIdle != null) {
			m_Skill.OnIdle ();
		}
		m_Skill.SetAnimation (ESkillAnim.Stay);
	}

	public override void UpdateState ()
	{
		base.UpdateState ();
		m_Skill.StayOwner ();
	}

	public override void ExitState ()
	{
		base.ExitState ();
	}

}
