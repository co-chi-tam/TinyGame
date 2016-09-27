using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIListUserConnected : UIListObjects {

	public ListAdapter<CUserConnectData> listAdapter;

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
		var child = base.OnListItemLoad (index) as UIItemUserConnected;
		var valueItem 					= listAdapter [index] as CUserConnectData;
		child.itemUserNameText.text		= valueItem.user;
		child.itemUserAvatarImage.sprite = Util.FindSprite (valueItem.avatar);
		child.id 						= valueItem.mapId;
		child.gameObject.SetActive (CTest9UserManager.currentUser.displayName != valueItem.user);
		return child;
	}

	public override void OnListItemClicked (int index, Vector2 position, UIListItem item)
	{
		base.OnListItemClicked (index, position, item);
//		var valueItem	= listAdapter [index] as CUserConnectData;
//		var child 		= item as UIItemUserConnected;

	}

}
