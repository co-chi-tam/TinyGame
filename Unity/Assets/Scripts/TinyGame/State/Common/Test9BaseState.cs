using UnityEngine;
using System.Collections;
using FSM;

public class Test9BaseState : FSMBaseState {

	protected CMapObjectBehaviour m_Controller;

	public Test9BaseState (IContext context) : base (context)
	{
		this.m_Controller = context as CMapObjectBehaviour;
	}

}
