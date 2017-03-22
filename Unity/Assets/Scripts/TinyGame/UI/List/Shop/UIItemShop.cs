using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIItemShop : UIListItemClickable {

	public Image itemImage;
	public Text itemName;
	public Text itemGoldPrice;
	public Text itemDiamondPrice;
	public Image itemHotDeal;
	[Header("Info")]
	public GameObject itemInfoPanel;
	public Image itemInfoImage;
	public Text itemInfoNameText;
	public Text itemInfoGoldPrice;
	public Text itemInfoDiamondPrice;

//	protected override void OnItemPointDown(Vector2 position) {
//		base.OnItemFocusEnter (position);
//		itemInfoPanel.SetActive (true);
//		itemInfoPanel.transform.position = position;
//	}
//
//	protected override void OnItemPointUp (Vector2 position)
//	{
//		base.OnItemFocusExit (position);
//		itemInfoPanel.SetActive (false);
//	}
//
}
