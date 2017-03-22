using UnityEngine;
using System.Collections;
using FSM;

public class CMapObjectBehaviour : CBaseMonobehaviour, IContext {

	private bool m_Active = false;

	public virtual void Init() {

	}

	protected override void Awake ()
	{
		base.Awake ();
	}

	protected override void Start ()
	{
		base.Start ();
	}

	public virtual bool GetActive() {
		return m_Active && this.gameObject.activeInHierarchy;	
	}

	public virtual void SetActive(bool value) {
		m_Active = value;
		this.gameObject.SetActive (value);
	}

	public virtual Vector3 GetPosition(){
		return m_Transform.position;
	}

	public virtual void SetPosition(Vector3 value) {
		m_Transform.position = value;
	}

	public virtual void SetData(CBaseData data) {
		
	}

}
