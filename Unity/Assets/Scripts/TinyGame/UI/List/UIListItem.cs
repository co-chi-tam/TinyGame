using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class UIListItem : UIPointer {

	public int index;
	public Action<int, Vector2, UIListItem> OnEventItemClicked;

}
