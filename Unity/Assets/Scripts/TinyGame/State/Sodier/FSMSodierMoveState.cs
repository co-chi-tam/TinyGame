using UnityEngine;
using System.Collections;
using FSM;

public class FSMSodierMoveState : Test9BaseState {

	private CSodierBehaviour m_Sodier;

	public FSMSodierMoveState (IContext context) : base (context)
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
		m_Sodier.MoveToEnemy ();
	}

	public override void ExitState ()
	{
		base.ExitState ();
	}

}
