using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIItemInventory : UIListItemClickable {

	public Image itemImage;
	public Text itemNameText;
	public Text itemAmountText;
	[Header("Info")]
	public GameObject itemInfoPanel;
	public Image itemInfoImage;
	public Text itemInfoNameText;
	public Text itemInfoAmountText;

	public string id;

}
