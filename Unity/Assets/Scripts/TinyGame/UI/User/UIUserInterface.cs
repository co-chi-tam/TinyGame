using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UIUserInterface : CBaseMonobehaviour {

	#region Properties
	[Header("User Info")]
	[SerializeField]	private GameObject m_UserInterfacePanel;
	[SerializeField]	private GameObject m_UserWorldMapPanel;
	[SerializeField]	private GameObject m_UserPartyPanel;
	[SerializeField]	private Text m_UserDisplayNameText;
	[SerializeField]	private Image m_UserAvatarImage;
	[SerializeField]	private Text m_UserDisplayGoldText;
	[SerializeField]	private Text m_UserDisplayDiamondText;
	[SerializeField]	private UIListMercenaries m_UserMercenaryList;
	[SerializeField]	private MercenaryFormationGroup m_MercenaryFormation;
	[SerializeField]	private UITextMovable m_NoticeText;
	[SerializeField]	private Text m_ServerStatus;
	[Header("Shop Panel")]
	[SerializeField]	private GameObject m_ShopPanel;
	[SerializeField]	private UIListItemShop m_ListItemShop;
	[Header("Inventory Panel")]
	[SerializeField]	private GameObject m_InventoryPanel;
	[SerializeField]	private UIListItemInventory m_ListItemInventory;
	[Header("User Connect Panel")]
	[SerializeField]	private GameObject m_PVPPanel;
	[SerializeField]	private UIListUserConnected m_ListUserConnect;
	[Header("Level Up Panel")]
	[SerializeField]	private GameObject m_LevelUpPanel;
	[SerializeField]	private UIListMaterialMecenaries m_UserMaterialMercenaryList;
	[SerializeField]	private MaterialMercenaryGroup m_MaterialMercenaryGroup;
	[Header("Lucky Wheel Panel")]
	[SerializeField]	private GameObject m_LuckyWheelPanel;
	[SerializeField]	private CLuckyWheel m_LuckyWheel;

	[Header("World Map")]
	[SerializeField]	private Button[] m_MapPoints;

	private CTest9UserManager m_UserManager;
	private int m_CurrentPVPPage = 0;

	#endregion

	#region Monobehaviour
	protected override void Start ()
	{
		base.Start ();
		m_UserManager = CTest9UserManager.GetInstance ();
		OpenUserInterface ();

//		var messageItem = new CMessageItem ();
//		messageItem.prefabPath = "MessageBoxPanel";
//		messageItem.title = "TITLE";
//		messageItem.content = "OK";
//		messageItem.AddNegativeListener (() => {
//			Debug.Log("OK");
//		});
//		MessageBox.Show (messageItem);
	}

	#endregion

	#region Main methods

	public void SetServerStatus(bool value) {
		m_ServerStatus.text = "Server " + (value ? "Connect" : "Disconnect");
	}

	public void SetupMapPoint(string mapID) {
		m_UserManager.OnClientEnterMatch (mapID, "{world_map_tiny_game}");
	}

	public void UpdateUserInterface() {
		m_UserDisplayNameText.text = CTest9UserManager.currentUser.displayName;
		m_UserDisplayGoldText.text = CTest9UserManager.currentUser.gold.ToString("#,##");
		m_UserDisplayDiamondText.text = CTest9UserManager.currentUser.diamond.ToString("#,##");
		m_UserAvatarImage.sprite = Util.FindSprite (CTest9UserManager.currentUser.avartar);
	}

	#endregion

	#region Main Interface

	public void Logout() {
		MessageBox.Show ("LOGOUT !!", "YOU want logout !!", () => {
			m_UserManager.OnClientLogout ();
		}, null);
	}

	public void OpenUserInterface() {
		UpdateUserInterface ();
		m_UserInterfacePanel.SetActive (true);
		m_UserWorldMapPanel.SetActive (false);
		m_UserPartyPanel.SetActive (false);
		m_ShopPanel.SetActive (false);
		m_InventoryPanel.SetActive (false);
		m_PVPPanel.SetActive (false);
		m_LevelUpPanel.SetActive (false);
		m_LuckyWheelPanel.SetActive (false);

		if (m_ListUserConnect.listAdapter != null) {
			m_ListUserConnect.listAdapter.Clear ();
		}
		if (m_UserMercenaryList.listAdapter != null) {
			m_UserMercenaryList.listAdapter.Clear ();
		}
		if (m_MercenaryFormation != null) {
			m_MercenaryFormation.Clear ();
		}
		if (m_ListItemInventory.listAdapter != null) {
			m_ListItemInventory.listAdapter.Clear ();
		}
		if (m_ListItemShop.listAdapter != null) {
			m_ListItemShop.listAdapter.Clear ();
		}
		if (m_UserMaterialMercenaryList.listAdapter != null) {
			m_UserMaterialMercenaryList.listAdapter.Clear ();
		}
	}

	public void SetNoticeText(string value) {
		m_NoticeText.text = value;
	}

	#endregion

	#region WorldMap

	public void OpenWorldMap() {
		m_UserWorldMapPanel.SetActive (true);
		m_UserInterfacePanel.SetActive (false);
		m_UserPartyPanel.SetActive (false);
		m_ShopPanel.SetActive (false);
		m_InventoryPanel.SetActive (false);
		m_PVPPanel.SetActive (false);
		m_LevelUpPanel.SetActive (false);
		m_LuckyWheelPanel.SetActive (false);
	}

	#endregion

	#region Shop

	public void OpenShopInterface() {
		m_UserInterfacePanel.SetActive (false);
		m_UserWorldMapPanel.SetActive (false);
		m_UserPartyPanel.SetActive (false);
		m_InventoryPanel.SetActive (false);
		m_PVPPanel.SetActive (false);
		m_ShopPanel.SetActive (true);
		m_LevelUpPanel.SetActive (false);
		m_LuckyWheelPanel.SetActive (false);
		if (m_ListItemShop.listAdapter != null) {
			m_ListItemShop.listAdapter.Clear ();
		}
		m_UserManager.OnClientEnterShop ();
	}

	public void SetupShopData(CItemData[] items) {
		m_ListItemShop.length = items.Length;
		m_ListItemShop.listAdapter = new ListAdapter<CItemData> (items.Length, items.ToList ());
		m_ListItemShop.OnListAlready ();
		m_ListItemShop.OnEventItemClicked -= OnOpenItemInShop;
		m_ListItemShop.OnEventItemClicked += OnOpenItemInShop;
	}

	private void OnOpenItemInShop(int index, Vector2 position, UIListItem item) {
		if (m_ListItemShop.listAdapter == null)
			return;
		if (m_ListItemShop.listAdapter.length <= index)
			return;
		var valueItem	= m_ListItemShop.listAdapter [index] as CItemData;
//		var child 		= item as UIItemShop;
		MessageBox.Show ("Buy - " + valueItem.name, valueItem.name + "\n GOLD: " + valueItem.goldPrice, () => {
			OnBuyItemInShop (valueItem);
		}, () => {
			OnCancelBuyItemInShop(valueItem);
		});
	}

	private void OnBuyItemInShop(CItemData value) {
		Debug.Log("BUY ITEM " + value.name + " / " + value.id);
		if (CTest9UserManager.currentUser.gold >= value.goldPrice) {
			m_UserManager.OnClientBuyItem (value.id, "1");
		} else {
			MessageBox.Show ("FAIL !!", "Your can not buy item !!" , null);
		}
	}

	private void OnCancelBuyItemInShop(CItemData value) {
		Debug.Log("CANCEL BUY ITEM " + value.name);
	}

	#endregion

	#region Inventory

	public void OpenInventoryPanel() {
		m_UserPartyPanel.SetActive (false);
		m_UserInterfacePanel.SetActive (false);
		m_UserWorldMapPanel.SetActive (false);
		m_ShopPanel.SetActive (false);
		m_InventoryPanel.SetActive (true);
		m_PVPPanel.SetActive (false);
		m_LevelUpPanel.SetActive (false);
		m_LuckyWheelPanel.SetActive (false);
		if (m_ListItemInventory.listAdapter != null) {
			m_ListItemInventory.listAdapter.Clear ();
		}
		m_UserManager.OnClientOpenInventory ();
	}

	public void SetupInventoryData(CItemData[] items) {
		m_ListItemInventory.length = items.Length;
		m_ListItemInventory.listAdapter = new ListAdapter<CItemData> (items.Length, items.ToList ());
		m_ListItemInventory.OnListAlready ();
		m_ListItemInventory.OnEventItemClicked -= OnOpenItemInInventory;
		m_ListItemInventory.OnEventItemClicked += OnOpenItemInInventory;
	}

	private void OnOpenItemInInventory(int index, Vector2 position, UIListItem item) {
		if (m_ListItemInventory.listAdapter == null)
			return;
		if (m_ListItemInventory.listAdapter.length <= index)
			return;
		var valueItem	= m_ListItemInventory.listAdapter [index] as CItemData;
//		var child 		= item as UIItemInventory;
		MessageBox.Show ("Use - " + valueItem.name, valueItem.name + "\n Amount: " + valueItem.amount, () => {
			if (item.gameObject.activeInHierarchy == true) {
				OnUseItemInInventory (valueItem);
				m_ListItemInventory.OnListItemUpdate(index, item);
			}
		}, () => {
			OnCancelUseItemInInventory(valueItem);
		});
	}

	private void OnUseItemInInventory(CItemData value) {
		Debug.Log("USE ITEM " + value.name + " / " + value.id);
		if (value.amount >= value.amountPerConsume) {
			value.amount -= value.amountPerConsume;
			m_UserManager.OnClientUseItem (value.id, value.amountPerConsume.ToString ());
		} else {
			MessageBox.Show ("Use - " + value.name, value.name + "\n Not enough amount: " + value.amount, null);
		}
	}

	private void OnCancelUseItemInInventory(CItemData value) {
		Debug.Log("CANCEL USE ITEM " + value.name);
	}

	#endregion

	#region Party

	public void SaveFormation() {
		m_MercenaryFormation.CalculateResult ();
		var results = m_MercenaryFormation.GetStringResults ();
		if (results != null) {
			m_UserManager.OnClientSaveFormation (results);
		}
	}

	public void OpenPartyPanel() {
		m_UserPartyPanel.SetActive (true);
		m_UserInterfacePanel.SetActive (false);
		m_UserWorldMapPanel.SetActive (false);
		m_ShopPanel.SetActive (false);
		m_InventoryPanel.SetActive (false);
		m_PVPPanel.SetActive (false);
		m_LevelUpPanel.SetActive (false);
		m_LuckyWheelPanel.SetActive (false);
		m_UserManager.OnClientLoadParty ();
	}

	public void SetupMercenaryData(CSodierSimpleData[] mercenaries, CMapSlotData[] formation) {
		m_UserMercenaryList.length = mercenaries.Length;
		m_UserMercenaryList.listAdapter = new ListAdapter<CSodierSimpleData> (mercenaries.Length, mercenaries.ToList ());
		m_UserMercenaryList.listFormation = new CMapSlotData[formation.Length];
		formation.CopyTo (m_UserMercenaryList.listFormation, 0);
		m_UserMercenaryList.OnListAlready ();
	}

	#endregion

	#region PVP

	public void OpenPVPInterface() {
		m_UserInterfacePanel.SetActive (false);
		m_UserWorldMapPanel.SetActive (false);
		m_UserPartyPanel.SetActive (false);
		m_InventoryPanel.SetActive (false);
		m_ShopPanel.SetActive (false);
		m_PVPPanel.SetActive (true);
		m_LevelUpPanel.SetActive (false);
		m_LuckyWheelPanel.SetActive (false);
		m_CurrentPVPPage = 1;
		m_UserManager.OnClientEnterPVP (m_CurrentPVPPage, 3);
	}

	public void NextPVPInterface() {
		if (m_PVPPanel.activeInHierarchy == false)
			return;
		if (m_ListUserConnect.listAdapter != null) {
			m_ListUserConnect.listAdapter.Clear ();
		}
		m_CurrentPVPPage = m_CurrentPVPPage < 25 ? m_CurrentPVPPage + 1 : 25;
		m_UserManager.OnClientEnterPVP (m_CurrentPVPPage, 3);
	}

	public void BackPVPInterface() {
		if (m_PVPPanel.activeInHierarchy == false)
			return;
		if (m_ListUserConnect.listAdapter != null) {
			m_ListUserConnect.listAdapter.Clear ();
		}
		m_CurrentPVPPage = m_CurrentPVPPage >= 2 ? m_CurrentPVPPage - 1 : 1;
		m_UserManager.OnClientEnterPVP (m_CurrentPVPPage, 3);
	}

	public void SetupPVPData(CUserConnectData[] items) {
		m_ListUserConnect.length = items.Length;
		m_ListUserConnect.listAdapter = new ListAdapter<CUserConnectData> (items.Length, items.ToList ());
		m_ListUserConnect.OnListAlready ();
		m_ListUserConnect.OnEventItemClicked -= OnOpenUserPVP;
		m_ListUserConnect.OnEventItemClicked += OnOpenUserPVP;
	}

	private void OnOpenUserPVP(int index, Vector2 position, UIListItem item) {
		if (m_ListUserConnect.listAdapter == null)
			return;
		if (m_ListUserConnect.listAdapter.length <= index)
			return;
		var valueItem	= m_ListUserConnect.listAdapter [index] as CUserConnectData;
//		var child 		= item as UIItemUserConnected;
		MessageBox.Show ("PVP - " + valueItem.user, valueItem.user + "\n ATTACK", () => {
			OnOpenPVP(valueItem);
		}, () => {
			OnCacelOpenPVP(valueItem);
		});
	}

	private void OnOpenPVP(CUserConnectData value) {
		Debug.Log("PVP with " + value.user);
		m_UserManager.OnClientEnterMatch (value.mapId, value.user, true);
	}

	private void OnCacelOpenPVP(CUserConnectData value) {
		Debug.Log("CANCEL PVP with " + value.user);
	}

	#endregion

	#region Level up

	public void OpenLevelUpPanel() {
		m_UserPartyPanel.SetActive (false);
		m_UserInterfacePanel.SetActive (false);
		m_UserWorldMapPanel.SetActive (false);
		m_ShopPanel.SetActive (false);
		m_InventoryPanel.SetActive (false);
		m_PVPPanel.SetActive (false);
		m_LevelUpPanel.SetActive (true);
		m_LuckyWheelPanel.SetActive (false);
		if (m_UserMaterialMercenaryList.listAdapter != null) {
			m_UserMaterialMercenaryList.listAdapter.Clear ();
		}
		m_MaterialMercenaryGroup.Clear ();
		m_UserManager.OnClientLoadAllMercenary ();
	}

	public void SetupMaterialMercenaryData(CSodierSimpleData[] items) {
		m_UserMaterialMercenaryList.length = items.Length;
		m_UserMaterialMercenaryList.listAdapter = new ListAdapter<CSodierSimpleData> (items.Length, items.ToList ());
		m_UserMaterialMercenaryList.OnListAlready ();
	}

	public void LevelUpMecenary() {
		m_MaterialMercenaryGroup.CalculateResult ();
		var results = m_MaterialMercenaryGroup.GetStringResults ();
		if (results != null) {
			m_UserManager.OnClientLevelUpMercenary (results[0], results[1], results[2], results[3]);
//			Debug.Log (results [0] + "\n" + results [1] + "\n" + results [2] + "\n" + results [3]);
		} else {
			MessageBox.Show ("FAIL", "LEVEL UP FAIL !!!", null);
		}
	}

	#endregion

	#region Lucky Wheel

	public void OpenLuckyWheelPanel() {
		m_UserPartyPanel.SetActive (false);
		m_UserInterfacePanel.SetActive (false);
		m_UserWorldMapPanel.SetActive (false);
		m_ShopPanel.SetActive (false);
		m_InventoryPanel.SetActive (false);
		m_PVPPanel.SetActive (false);
		m_LevelUpPanel.SetActive (false);
		m_LuckyWheelPanel.SetActive (true);
		m_UserManager.OnClientEnterLuckyWheel ();
	}

	public void ScrollLuckyWheel() {
		m_LuckyWheel.OnEndRotation -= OnScrollLuckyWheelComplete;
		m_LuckyWheel.OnEndRotation += OnScrollLuckyWheelComplete;
	}

	public void OnScrollLuckyWheelComplete() {
		m_UserManager.OnClientStartLuckyWheel ();
	}

	#endregion

}
