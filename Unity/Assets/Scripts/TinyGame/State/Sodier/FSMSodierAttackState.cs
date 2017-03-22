using UnityEngine;
using System.Collections;
using FSM;

public class FSMSodierAttackState : Test9BaseState {

	private CSodierBehaviour m_Sodier;

	public FSMSodierAttackState (IContext context) : base (context)
	{
		m_Sodier = context as CSodierBehaviour;
	}

	public override void StartState ()
	{
		base.StartState ();
		m_Sodier.SetAnimationBaseTeam (ESodierAnim.Attack);
		m_Sodier.ApplyDamageToEnemy ();
		if (m_Sodier.OnAttack != null) {
			m_Sodier.OnAttack ();
		}
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
