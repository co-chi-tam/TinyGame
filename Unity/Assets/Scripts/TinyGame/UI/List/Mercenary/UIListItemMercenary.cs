using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIListItemMercenary : UIListItem {

	public Image mercenaryImage;
	public Text mercenaryText;
	public Text mercenaryLevel;
	[Header("Info")]
	public GameObject infoPanel;
	public Text nameText;
	public Text healthText;
	public Text manaText;
	public Text attackDamageText;
	public Text attackSpeedText;
	public UIDrag dragObject;

//	protected override void OnItemPointDown(Vector2 position) {
//		base.OnItemFocusEnter (position);
//		infoPanel.SetActive (true);
//	}
//
//	protected override void OnItemFocusStay (Vector2 position)
//	{
//		base.OnItemFocusStay (position);
//		infoPanel.transform.position = position;
//	}
//
//	protected override void OnItemPointUp (Vector2 position)
//	{
//		base.OnItemFocusExit (position);
//		infoPanel.SetActive (false);
//	}

}
