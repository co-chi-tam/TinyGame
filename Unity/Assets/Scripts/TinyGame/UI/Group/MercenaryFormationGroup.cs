using UnityEngine;
using System.Collections;

public class MercenaryFormationGroup : UIGroup {

	private string[] m_StringResults;

	public override void CalculateResult ()
	{
		base.CalculateResult ();
		m_StringResults = new string[members.Length];
		var active = false;
		for (int i = 0; i < members.Length; i++) {
			var dropItem = members [i].GetComponent<UIDrop> ();
			var result = members [i].GetResult ();
			if (result != null) {
				var stringPattern = "{0},{1},{2},{3}";
				var dragObject = dropItem.dragableObject;
				var availableSlot = dragObject.GetResult ().GetBool () ? 1 : 0;
				active |= availableSlot == 1;
				m_StringResults[i] = string.Format(stringPattern, availableSlot, 
																	dragObject.GetResult().GetString(), 
																	result.GetVector3 ().x, result.GetVector3().y);	
			} else {
				m_StringResults[i] = "0,0000000000000000,-1,-1";
			}
		}
		if (active == false) {
			m_StringResults = null;
		}
	}

	public override string[] GetStringResults ()
	{
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
