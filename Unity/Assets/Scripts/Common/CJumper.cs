using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

[RequireComponent(typeof(Parabola))]
public class CJumper : NetworkBehaviour {

	protected Parabola m_Parabola;
	protected Queue<Vector3> m_StepQueue;
	protected Transform m_Transform;


	protected bool m_OnClientJumping = false;
	protected bool m_Jumping = false;
	protected int m_JumpIndex = -1;

	protected virtual void Awake ()
	{
		m_Transform = this.transform;
		m_Parabola = this.GetComponent<Parabola> ();
		m_Jumping = false;
		m_StepQueue = new Queue<Vector3> ();
	}

	protected virtual void Start ()
	{
		
	}

	protected virtual void Update ()
	{

	}

	[ClientRpc]
	public virtual void RpcJumpTo(Vector3 position) {
		if (m_OnClientJumping == false) {
			m_OnClientJumping = true;	
			JumpTo (position, () => { 
				m_OnClientJumping = false;
				CmdOnClientCompleteJumpTo(); 
			});
		}
	}

	[Command]
	public virtual void CmdOnClientCompleteJumpTo() {
		
	}

	public virtual void JumpTo(Vector3 position, Action complete = null) {
		m_Jumping = true;
		CHandleEvent.Instance.AddEvent (m_Parabola.HandleJumpTo (position, (jump) => {
			// TODO
		}, () => {
			m_Jumping = false;
			if (complete != null) {
				complete();
			}
		}));
		if (isServer) {
			RpcJumpTo (position);
		}
	}

	[ClientRpc]
	public virtual void RpcJumpToPositions(string positions) {
		if (m_OnClientJumping == false) {
			m_OnClientJumping = true;	
			var v3Array = positions.Split (';'); // (x,y,z);(x,y,z);....
			var v3Positions = new Vector3[v3Array.Length];
			for (int i = 0; i < v3Positions.Length; i++) {
				v3Positions [i] = CUtil.V3Parser (v3Array [i]);
			}
			JumpToPositions (CmdOnClientJumpComplete, () => {
				m_OnClientJumping = false;	
				CmdOnClientAllStepComplete();
			}, v3Positions);
		}
	}

	[Command]
	public virtual void CmdOnClientJumpComplete(Vector3 position) {

	}

	[Command]
	public virtual void CmdOnClientAllStepComplete() {

	}

	public virtual void JumpToPositions(Action<Vector3> jumpComplete = null, Action allComplete = null, params Vector3[] positions) {
		if (m_StepQueue.Count > 0)
			return;
		for (int i = 0; i < positions.Length; i++) {
			m_StepQueue.Enqueue (positions [i]);
		}
		CHandleEvent.Instance.AddEvent (HandleJumpToPositions (jumpComplete), allComplete);
		if (isServer) {
			var strPositions = new StringBuilder ();
			for (int i = 0; i < positions.Length; i++) {
				strPositions.Append (CUtil.V3StrParser (positions [i]) + ";");
			}
			strPositions.Remove (strPositions.Length - 1, 1);
			RpcJumpToPositions (strPositions.ToString ());
		}
	}

	protected virtual IEnumerator HandleJumpToPositions(Action<Vector3> complete = null) {
		while (m_StepQueue.Count > 0) {
			if (GetJumping () == false) {
				var position = m_StepQueue.Peek ();
				AddStep ();
				JumpTo (position, () => {
					if (complete != null) {
						complete(position);
					}
					m_StepQueue.Dequeue();
				});
			}
			yield return null;
		}
		yield return null;
	}

	public virtual void AddStep() {
		m_JumpIndex++;
	}

	public virtual void SetPosition(Vector3 position) {
		m_Transform.position = position;
		RpcSetPosition (position);
	}

	[ClientRpc]
	public virtual void RpcSetPosition(Vector3 position) {
		m_Transform.position = position;
	}

	public virtual Vector3 GetPosition() {
		return m_Transform.position;
	}

	public virtual bool GetJumping() {
		return m_Jumping;
	}

}
