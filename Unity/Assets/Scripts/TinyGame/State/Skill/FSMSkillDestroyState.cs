using UnityEngine;
using System.Collections;
using FSM;

public class FSMSkillDestroyState : Test9BaseState {

	private CSkillBehaviour m_Skill;

	public FSMSkillDestroyState (IContext context) : base (context)
	{
		m_Skill = context as CSkillBehaviour;
	}

	public override void StartState ()
	{
		base.StartState ();
		if (m_Skill.OnDestroy != null) {
			m_Skill.OnDestroy ();
		}
		m_Skill.SetAnimation (ESkillAnim.Destroy);
		m_Skill.EndTurn ();
		m_Skill.Reset ();
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
