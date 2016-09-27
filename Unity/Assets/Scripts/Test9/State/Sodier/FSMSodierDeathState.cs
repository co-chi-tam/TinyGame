using UnityEngine;
using System.Collections;
using FSM;

public class FSMSodierDeathState : Test9BaseState {

	private CSodierBehaviour m_Sodier;

	public FSMSodierDeathState (IContext context) : base (context)
	{
		m_Sodier = context as CSodierBehaviour;
	}

	public override void StartState ()
	{
		base.StartState ();
		m_Sodier.SetAnimationBaseTeam (ESodierAnim.Death);
		if (m_Sodier.OnDeath != null) {
			m_Sodier.OnDeath ();
		}
		m_Sodier.Reset ();
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
