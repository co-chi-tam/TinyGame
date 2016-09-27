using UnityEngine;
using System.Collections;

public class MaterialMercenaryGroup : UIGroup {

	private string[] m_StringResults;

	public override void CalculateResult ()
	{
		base.CalculateResult ();
		m_StringResults = new string[members.Length];
		var active = true;
		for (int i = 0; i < members.Length; i++) {
			var dropItem = members [i].GetComponent<UIDrop> ();
			var result = members [i].GetResult ();
			if (result != null) {
				var dragObject = dropItem.dragableObject;
				m_StringResults[i] = dragObject.GetResult().GetString();
			} else {
				m_StringResults[i] = "000000000000000000";
				active = false;
			}
		}
		if (active == false) {
			m_StringResults = null;
		}
	}

	public override string[] GetStringResults ()
	{
		if (m_StringResults == null)
			return null;
		for (int i = 0; i < m_StringResults.Length; i++) {
			if (m_StringResults [i] == "000000000000000000") {
				return null;
			}
		}
		return m_StringResults;
	}

	public override void Clear ()
	{
		base.Clear ();
		for (int i = 0; i < members.Length; i++) {
			var dropItem = members [i].GetComponent<UIDrop> ();
			if (dropItem.cloneDragableObject != null) {
				dropItem.ClearDropObject ();
			}
		}
	}
}
