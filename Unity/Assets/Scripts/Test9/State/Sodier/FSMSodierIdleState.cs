using UnityEngine;
using System.Collections;
using FSM;

public class FSMSodierIdleState : Test9BaseState {

	private CSodierBehaviour m_Sodier;

	public FSMSodierIdleState (IContext context) : base (context)
	{
		m_Sodier = context as CSodierBehaviour;
	}

	public override void StartState ()
	{
		base.StartState ();
		m_Sodier.SetAnimationBaseTeam (ESodierAnim.Idle);
		m_Sodier.EndTurn ();
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
