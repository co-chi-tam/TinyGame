using UnityEngine;
using System.Collections;
using FSM;

public class FSMSodierBackState : Test9BaseState {
	
	private CSodierBehaviour m_Sodier;

	public FSMSodierBackState (IContext context) : base (context)
	{
		m_Sodier = context as CSodierBehaviour;
	}

	public override void StartState ()
	{
		base.StartState ();
		m_Sodier.SetAnimationBaseTeam (ESodierAnim.Move);
	}

	public override void UpdateState ()
	{
		base.UpdateState ();
		m_Sodier.MoveToStartPoint ();
	}

	public override void ExitState ()
	{
		base.ExitState ();
	}
}
