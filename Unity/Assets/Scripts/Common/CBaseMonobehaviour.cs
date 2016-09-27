using UnityEngine;
using UnityEngine.Networking;

public class CBaseMonobehaviour : MonoBehaviour {

	protected Transform m_Transform;

	protected virtual void Awake() {
		m_Transform = this.transform;
	}

	protected virtual void Start () {
	
	}
	
	private void Update () {
		this.UpdateBaseTime (Time.deltaTime);
	}

	protected virtual void UpdateBaseTime(float dt) {
		
	}

	public virtual void SetActive(bool value, Transform parent = null) {
		this.gameObject.SetActive (value);
		this.transform.SetParent (parent);
	}

}
