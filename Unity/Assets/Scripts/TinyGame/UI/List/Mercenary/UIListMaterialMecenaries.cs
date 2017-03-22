using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIListMaterialMecenaries : UIListObjects {

	public ListAdapter<CSodierSimpleData> listAdapter;
	[SerializeField] 	private UIGroup m_MaterialMecenaryGroup;

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
		return child;
	}

}
