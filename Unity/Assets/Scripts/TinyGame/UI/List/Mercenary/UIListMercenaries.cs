using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIListMercenaries : UIListObjects {

	public ListAdapter<CSodierSimpleData> listAdapter;
	public CMapSlotData[] listFormation;

	[SerializeField] 	private UIGroup m_FormationMecenary;

	protected override void Awake ()
	{
		base.Awake ();
	}

	public override void OnListAlready ()
	{
		if (listAdapter != null) {
			length = listAdapter.length;
			listAdapter.OnClear -= OnListClear;
			listAdapter.OnClear += OnListClear;
			base.OnListAlready ();
		}
	}

	public override UIListItem OnListItemLoad (int index)
	{
		var child = base.OnListItemLoad (index) as UIListItemMercenary;
		var valueMercenary 			= listAdapter [index] as CSodierSimpleData;
		child.mercenaryText.text 	= valueMercenary.name;
		child.nameText.text 		= "Name: " + valueMercenary.name;
		child.mercenaryLevel.text 	= "Lv" + valueMercenary.level;
//		child.healthText.text 		= "HP: " + valueMercenary.maxHealth.ToString();
//		child.manaText.text 		= "MP: " + valueMercenary.maxMana.ToString();
//		child.attackDamageText.text = "Attack: " + valueMercenary.damage.ToString();
//		child.attackSpeedText.text 	= "Speed: " + valueMercenary.attackSpeed.ToString();
		child.mercenaryImage.sprite = Util.FindSprite (valueMercenary.avatar);
		child.dragObject.GetResultObject ().SetString (valueMercenary.id);
		for (int i = 0; i < listFormation.Length; i++) {
			if (listFormation [i].gameType.CompareTo (valueMercenary.id) == 0) {
				var indexSlot = (int) (listFormation[i].slotIds.x + (listFormation[i].slotIds.y * 3));
				var member = m_FormationMecenary.members [indexSlot];
				var dropableObj = member.GetComponent<UIDrop> ();
				if (dropableObj != null) {
					dropableObj.SetDropObject (child.dragObject.gameObject, Vector2.zero);
					child.dragObject.OnEventEndDrag (Vector2.zero, child.dragObject.GetResultObject());
					child.dragObject.SetState (UIDrag.EDragState.EndDrag);
				}
				member.GetResultObject ().SetString (valueMercenary.id);
				break;
			}
		}
		return child;
	}

}
