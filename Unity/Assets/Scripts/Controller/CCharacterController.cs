using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CCharacterController : CBaseController {

	private CEnum.EAnimation m_CurrentAnimation;

	protected override void Awake ()
	{
		base.Awake ();
	}

	protected override void Start ()
	{
		base.Start ();
	}

	public override void SetAnimation(CEnum.EAnimation anim) {
		base.SetAnimation (anim);
		m_CurrentAnimation = anim;
	}

	public virtual CEnum.EAnimation GetAnimation() {
		return m_CurrentAnimation;
	}

}
