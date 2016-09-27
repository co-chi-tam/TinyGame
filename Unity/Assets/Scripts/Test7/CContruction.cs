using UnityEngine;
using System.Collections;

public class CContruction : CBaseMonobehaviour {

	public void ResetTransform(Transform parent) {
		m_Transform.SetParent (parent);
		m_Transform.localPosition = Vector3.zero;
		m_Transform.localScale = Vector3.one;
		m_Transform.localRotation = Quaternion.identity;
	}

}
