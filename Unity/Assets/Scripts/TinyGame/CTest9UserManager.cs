using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SocketIO;
using System.Net.Sockets;

public class CTest9UserManager : CMonoSingleton<CTest9UserManager> {

	#region Properties

	[SerializeField]	private SocketIOComponent m_SocketIO;
	[SerializeField]	private EGameScene m_GameScene;
	[Header("Login")]
	[SerializeField]	private UIUserLogin m_UserLoginPrefab;
	[SerializeField]	private string m_UserLoginScene = "Test9Login";
	[Header("User Interface")]
	[SerializeField]	private UIUserInterface m_UserInterfacePrefab;
	[SerializeField]	private string m_UserInterfaceScene = "Test9UserInterface";
	[Header("User Game")]
	[SerializeField]	private UIUserGame m_UserGamePrefab;
	[SerializeField]	private string m_UserGameScene = "Test9Game";

	public enum EGameScene : byte {
		Login = 0,
		UserInterface = 1,
		Game = 2
	}

	private CTest9DataParser<string, CUserData> 		m_UserDataParser;
	private CTest9DataParser<string, CSodierSimpleData> m_UserMercenariesParser;
	private CTest9DataParser<string, CMapData> 			m_UserFormationParser;
	private CTest9DataParser<string, CShopData> 		m_ShopParser;
	private CTest9DataParser<string, CInventoryData> 	m_InventoryParser;
	private CTest9DataParser<string, CPVPData> 			m_PVPParser;
	private CTest9DataParser<string, CBattleLogData> 	m_BattleLogParser;
	private CTest9DataParser<string, CBaseData> 		m_LogoutParser;
	private CTest9DataParser<string, CBattleAward> 		m_BattleAwardParser;
	private CTest9DataParser<string, CBaseData> 		m_MapUpdateParser;
	private CTest9DataParser<string, CPurchaseData> 	m_BuyItemRequestParser;
	private CTest9DataParser<string, CConsumeItemData> 	m_UseItemRequestParser;
	private CTest9DataParser<string, CLevelUpResponseData> m_LevelUpParser;
	private CTest9DataParser<string, CLuckyWheelResultData> m_LuckyWheelParser;

	// UI
	private UIUserLogin m_UserLogin;
	private UIUserInterface m_UserInterface;
	private UIUserGame m_UserGame;

	private bool m_Init = false;

	public static CUserData currentUser;

	#endregion

	#region Monobehaviour

	protected override void Awake ()
	{
		base.Awake ();
		DontDestroyOnLoad (this.gameObject);
		m_UserDataParser 		= new CTest9DataParser<string, CUserData> ();
		m_UserMercenariesParser = new CTest9DataParser<string, CSodierSimpleData> ();
		m_UserFormationParser 	= new CTest9DataParser<string, CMapData> ();
		m_LogoutParser 			= new CTest9DataParser<string, CBaseData> ();
		m_MapUpdateParser 		= new CTest9DataParser<string, CBaseData> ();
		m_ShopParser 			= new CTest9DataParser<string, CShopData> ();
		m_BuyItemRequestParser 	= new CTest9DataParser<string, CPurchaseData> ();
		m_InventoryParser 		= new CTest9DataParser<string, CInventoryData> ();
		m_UseItemRequestParser 	= new CTest9DataParser<string, CConsumeItemData> ();
		m_PVPParser 			= new CTest9DataParser<string, CPVPData> ();
		m_BattleLogParser 		= new CTest9DataParser<string, CBattleLogData> ();
		m_BattleAwardParser 	= new CTest9DataParser<string, CBattleAward> ();
		m_LevelUpParser			= new CTest9DataParser<string, CLevelUpResponseData> ();
		m_LuckyWheelParser 		= new CTest9DataParser<string, CLuckyWheelResultData> ();

		#if UNITY_EDITOR
		InvokeRepeating ("UpdateServerStatus", 1f, 2f);
		#endif
	}

	protected override void Start ()
	{
		base.Start ();
		InitScene ();
	}

	private void UpdateServerStatus() {
		if (m_UserInterface != null) {
			m_UserInterface.SetServerStatus (m_SocketIO.IsConnected);
		}
	}

	private void OnLevelWasLoaded(int index) {
		m_Init = false;
		InitScene ();
	}

	private void OnApplicationQuit() {
		Debug.Log ("OnApplicationQuit");
	}

	#endregion

	#region Main method

	private void InitScene() {
		if (m_Init == false) {
			m_Init = true;
			switch (m_GameScene) {
			case EGameScene.Login:
				InitLoginScene ();
				break;
			case EGameScene.UserInterface:
				InitUserInterfaceScene ();
				break;
			case EGameScene.Game:
				InitGameScene ();
				break;
			}
		}
	}

	private void InitLoginScene() {
		Loading.OnStart ();
		// Fake login
		OnClientFakeLogin (() => {
			OnClientLoginComplete(m_UserDataParser.GetValues()[0], true);
			Loading.OnStop ();
		}, (error) => {
			OnClientFakeLoginFail(error);
			Loading.OnStop ();
		});
	}

	private void InitUserInterfaceScene() {
		m_UserInterface = Instantiate(m_UserInterfacePrefab);
		m_UserInterface.UpdateUserInterface ();
	}

	private void InitGameScene() {
		m_UserGame = Instantiate(m_UserGamePrefab);
	}

	#endregion

	#region Login

	public void OnClientFakeLogin(Action complete = null, Action<string> error = null) {
		Loading.OnStart();
		var oldUserName = PlayerPrefs.GetString (CUserData.USER_NAME, string.Empty);
		var oldToken = PlayerPrefs.GetString (CUserData.USER_TOKEN, string.Empty);
		if (string.IsNullOrEmpty (oldUserName) || string.IsNullOrEmpty (oldToken)) {
			if (error != null) {
				error ("Empty user name !!");
			}
		} else {
			var deviceId = SystemInfo.deviceUniqueIdentifier;
			var goodLoginToken = string.Format (CTest9ServerInfo.GET_GOOD_LOGIN_TOKEN, oldUserName, deviceId, oldToken);
			m_UserDataParser.ParseURL (goodLoginToken, () => {
				if (complete != null) {
					complete ();
				}
				Loading.OnStop();
			}, (tokenError) => {
				if (error != null) {
					error (tokenError.errorContent);
				}
				Loading.OnStop();
			});
		}
	}

	public void OnClientLogin(string userName, string userPassword) {
		var userInfo = new Dictionary<string, string> ();
		PlayerPrefs.SetString (CUserData.USER_NAME, userName);
		PlayerPrefs.SetString (CUserData.USER_PASSWORD, userPassword);
		PlayerPrefs.Save ();
		userInfo["uname"] = userName;
		userInfo["upass"] = userPassword;
		userInfo["macadd"] = SystemInfo.deviceUniqueIdentifier;
		m_UserDataParser.ParseURL (CTest9ServerInfo.POST_LOGIN, userInfo, () => {
			var user = m_UserDataParser.GetData(userName);
			user.userPassword = userPassword;
			if (user == null) {
				OnClientLoginFail(1, "USER DATA RESPONSE FAIL !!!");
			} else {
				OnClientLoginComplete(user, false);
			}
		}, (loginError) => {
			OnClientLoginFail(loginError.errorCode, loginError.errorContent);
			m_UserLogin.SetUserInfoActive (true);
			m_UserLogin.SetUserWarningText (loginError.errorContent);
		});
	}

	public void OnClientLogout() {
		var userInfo = new Dictionary<string, string> ();
		var userName = string.Empty;
		var userPassword = string.Empty;
		if (currentUser != null) {
			userName = currentUser.userName;
			userPassword = currentUser.userPassword;
		} else {
			var oldUserName = PlayerPrefs.GetString (CUserData.USER_NAME, string.Empty);
			var oldPassword = PlayerPrefs.GetString (CUserData.USER_PASSWORD, string.Empty);
			if (string.IsNullOrEmpty (oldUserName) || string.IsNullOrEmpty (oldPassword)) {
				return;
			} else {
				userName = oldUserName;
				userPassword = oldPassword;
			}
		}
		userInfo ["uname"] = userName;
		userInfo ["upass"] = userPassword;
//		Debug.Log (userName + " / " + userPassword);
		Loading.OnStart ();
		m_LogoutParser.ParseURL (CTest9ServerInfo.POST_LOGOUT, userInfo, () => {
			OnClientLogoutComplete();
			Loading.OnStop();
		}, (error) => {
			OnClientLogoutFail (error.errorContent);
			Loading.OnStop();
		});
	}

	public void OnClientLoginComplete(CUserData user, bool baseToken) {
		currentUser = user;
		currentUser.userPassword = PlayerPrefs.GetString (CUserData.USER_PASSWORD, string.Empty);
		currentUser.needUpdateAPI = "{SHOP}{PARTY}{LEVELUP}{USERMAP}";
		PlayerPrefs.SetString (CUserData.USER_NAME, currentUser.userName);
		PlayerPrefs.SetString (CUserData.USER_TOKEN, currentUser.token);
		PlayerPrefs.SetInt (CUserData.USER_LOGIN_STATUS, 1);
		PlayerPrefs.Save ();
		if (baseToken) {
			Debug.Log ("GOOD TOKEN: " + user.token + " USER: " + user.displayName);
		} else {
			Debug.Log ("LOGIN COMPLETE " + user.token);
		}
		if (user.active) {
			OnClientStartGame ();
			m_GameScene = EGameScene.UserInterface;
			SceneManager.LoadSceneAsync (m_UserInterfaceScene);
		} else {
			m_UserLogin = Instantiate(m_UserLoginPrefab);
			m_UserLogin.SetDisplayNameUserActive (true);
			m_UserLogin.SetUserInfoActive (false);
		}
		// Connect Web Socket.
		m_SocketIO.Connect ();
		m_SocketIO.On ("all_client", (obj) => {
			Debug.Log(obj.ToString());
			if (obj.data.GetField("result")) {
				var notice = obj.data.GetField("result").ToString();
				m_UserInterface.SetNoticeText(notice);
			}
		});
		m_SocketIO.On ("message", (obj) => {
			Debug.Log(obj.ToString());
		});
	}	

	public void OnClientFakeLoginFail(string error) {
		m_UserLogin = Instantiate(m_UserLoginPrefab);
		m_UserLogin.SetUserInfoActive (true);
		Debug.Log("FAKE LOGIN FAIL !!! -- ERROR: " + error);
	}

	public void OnClientLoginFail(int errorCode, string error) {
		Debug.Log ("LOGIN ERROR: " + error);
		if (errorCode == 101) {
			// TODO
		}
	}

	public void OnClientLogoutComplete () {
		Debug.Log ("LOGOUT COMPLETE");
		PlayerPrefs.SetString (CUserData.USER_NAME, string.Empty);
		PlayerPrefs.SetString (CUserData.USER_PASSWORD, string.Empty);
		PlayerPrefs.SetString (CUserData.USER_TOKEN, string.Empty);
		PlayerPrefs.SetInt (CUserData.USER_LOGIN_STATUS, 0);
		PlayerPrefs.Save ();
		OnClientEndGame();
	}

	public void OnClientLogoutFail(string error) {
		Debug.Log (error);
	}

	#endregion

	#region Active User

	public void OnClientActiveUser(string displayName) {
		var userInfo = new Dictionary<string, string> ();
		userInfo["uname"] = currentUser.userName;
		userInfo["dname"] = displayName;
		userInfo["secret_key"] = currentUser.secretKey;
		userInfo["token"] = currentUser.token;
//		Debug.Log (currentUser.userName + "/" + displayName + "/" + currentUser.secretKey + "/" + currentUser.token);
		Loading.OnStart();
		var activeUserParser = new CTest9DataParser<string, CBaseData> ();
		activeUserParser.ParseURL (CTest9ServerInfo.POST_ACTIVE_USER, userInfo, () => {
			currentUser.displayName = displayName;
			m_GameScene = EGameScene.UserInterface;
			SceneManager.LoadSceneAsync (m_UserInterfaceScene);
			OnClientStartGame ();
			Loading.OnStop();
		}, (error) => {
			m_UserLogin.SetDisplayNameUserActive (true);
			m_UserLogin.SetUserWarningText (error.errorContent);
			MessageBox.Show("ERROR", "ACTIVE USER FAIL !!! " + error.errorContent, null);
			Loading.OnStop();
		});
	}

	#endregion

	#region Party

	public void OnClientLoadParty() {
		Loading.OnStart ();
		if (currentUser != null) {
			if (currentUser.needUpdateAPI.IndexOf("{PARTY}") != -1) {
				var userMercenariesURL = string.Format (CTest9ServerInfo.GET_ALL_MERCENARIES, currentUser.userName, currentUser.token);
				var userFormationURL = string.Format (CTest9ServerInfo.GET_USER_MAP_FORMATION, currentUser.userName, currentUser.token);
				m_UserMercenariesParser.ParseURL (userMercenariesURL, () => {
					m_UserFormationParser.ParseURL (userFormationURL, () => {
						currentUser.needUpdateAPI = currentUser.needUpdateAPI.Replace("{PARTY}", string.Empty);
						OnClientLoadPartyComplete();
						Loading.OnStop ();
					}, (error) => {
						OnClientLoadPartyFail (error.errorContent);	
						if (currentUser.needUpdateAPI.IndexOf("{PARTY}") == -1) {
							currentUser.needUpdateAPI += "{PARTY}";
						}
						Loading.OnStop ();
						MessageBox.Show("ERROR", "LOAD PARTY FAIL !!! ", null);
					}); 
				}, (error) => {
					OnClientLoadPartyFail (error.errorContent);	
					if (currentUser.needUpdateAPI.IndexOf("{PARTY}") == -1) {
						currentUser.needUpdateAPI += "{PARTY}";
					}
					MessageBox.Show("ERROR", "LOAD PARTY FAIL !!! " + error.errorContent, null);
					Loading.OnStop ();
				});
			} else {
				OnClientLoadPartyComplete();
				Loading.OnStop ();
			}
		} else {
			OnClientLoadPartyFail("ERROR Open party. Error: User not correct !!");
			Loading.OnStop ();
		}
	}

	public void OnClientLoadPartyComplete() {
		Debug.Log("OnClientLoadPartyComplete");
		var mercenaryData = m_UserMercenariesParser.GetValues ();
		var formationData = m_UserFormationParser.GetValues () [0].mapSlots;
		m_UserInterface.SetupMercenaryData (mercenaryData, formationData);
	}

	public void OnClientLoadPartyFail(string error) {
		Debug.Log (error);
	}

	public void OnClientSaveFormation(string[] formation) {
		var mapSlot = new Dictionary<string, string> ();
		mapSlot ["uname"] = currentUser.userName;
		mapSlot ["token"] = currentUser.token;
		for (int i = 0; i < formation.Length; i++) {
			mapSlot.Add ("slot" + (i + 1), formation [i]);
		}
		var mapFormationUrl = string.Format (CTest9ServerInfo.POST_UPDATE_MAP_FORMATION);
		Loading.OnStart ();
		m_MapUpdateParser.ParseURL (mapFormationUrl, mapSlot, () => {
			MessageBox.Show("SAVE COMPLETE", "Save formation complete !!", null);
			Loading.OnStop ();
		}, (error) => {
			MessageBox.Show("ERROR FORMATION", "ERROR FORMATION " + error.errorContent, null);
			Loading.OnStop ();
		});
		if (currentUser.needUpdateAPI.IndexOf("{PARTY}") == -1) {
			currentUser.needUpdateAPI += "{PARTY}";
		}
		if (currentUser.needUpdateAPI.IndexOf("{USERMAP}") == -1) {
			currentUser.needUpdateAPI += "{USERMAP}";
		}
	}

	#endregion

	#region Shop

	public void OnClientEnterShop() {
		var getShopURL = string.Format (CTest9ServerInfo.GET_SHOP, currentUser.token);
		Loading.OnStart ();
		if (currentUser != null) {
			if (currentUser.needUpdateAPI.IndexOf("{SHOP}") != -1) {
				m_ShopParser.ParseURL (getShopURL, () => {
					currentUser.needUpdateAPI = currentUser.needUpdateAPI.Replace("{SHOP}", string.Empty);
					OnClientEnterShopComplete();
					Loading.OnStop ();
				}, (error) => {
					OnClientEnterShopFail ("ERROR Open shop. Error: " + error.errorContent);	
					if (currentUser.needUpdateAPI.IndexOf("{SHOP}") == -1) {
						currentUser.needUpdateAPI += "{SHOP}";
					}
					Loading.OnStop ();
				});
			} else {
				OnClientEnterShopComplete ();
				Loading.OnStop ();
			}
		} else {
			OnClientEnterShopFail("ERROR Open shop. Error: User not correct !!");
			Loading.OnStop ();
		}
	}

	public void OnClientEnterShopComplete() {
		Debug.Log ("SHOP ENTER COMPLETE !!!");	
		var shopdata = m_ShopParser.GetData ("shopItem");
		m_UserInterface.SetupShopData (shopdata.items);
	}

	public void OnClientEnterShopFail(string error) {
		MessageBox.Show("ERROR !!", error, null);
		Loading.OnStop ();
	}

	public void OnClientBuyItem(string itemId, string amount) {
		var buyItemURL = CTest9ServerInfo.POST_BUY_ITEM_GOLD;
		var buyItemInfo = new Dictionary<string, string> ();
		buyItemInfo ["iid"] = itemId;
		buyItemInfo ["uname"] = currentUser.userName;
		buyItemInfo ["token"] = currentUser.token;
		buyItemInfo ["amount"] = amount;
		Loading.OnStart ();
		m_BuyItemRequestParser.ParseURL(buyItemURL, buyItemInfo, () => {
			OnClientBuyComplete(m_BuyItemRequestParser.GetValues()[0]);
			Loading.OnStop ();
		}, (error) => {
			OnClientBuyFail(error.errorContent);
			Loading.OnStop ();
		});
	}

	public void OnClientBuyComplete(CPurchaseData purchase) {
		Debug.Log ("CLIENT BUY COMPLETE ");
		currentUser.gold = purchase.currentGold;
		currentUser.diamond = purchase.currrentDiamond;
		m_UserInterface.UpdateUserInterface ();
		MessageBox.Show ("COMPLETE !!", "User buy item complete, please check your bag !!", null);
	}

	public void OnClientBuyFail(string error) {
		Debug.Log ("CLIENT BUY FAIL " + error);
		MessageBox.Show ("FAIL !!", "Your can not buy item !! \n" + error , null);
	}

	#endregion

	#region Inventory

	public void OnClientOpenInventory() {
		var getInventoryURL = string.Format (CTest9ServerInfo.GET_INVENTORY, currentUser.userName, currentUser.token);
		Loading.OnStart ();
		m_InventoryParser.ParseURL (getInventoryURL, () => {
			var inventorydata = m_InventoryParser.GetData("inventory-" + currentUser.userName);
			m_UserInterface.SetupInventoryData(inventorydata.items);
			Loading.OnStop ();
		}, (error) => {
			Debug.Log (error.errorContent);	
			MessageBox.Show("ERROR", "LOAD INVENTORY FAIL !!! " + error.errorContent, null);
			Loading.OnStop ();
		});
	}

	public void OnClientUseItem(string itemId, string amount) {
		var useItemURL = CTest9ServerInfo.POST_USE_ITEM_INVENTORY;
		var useItemInfo = new Dictionary<string, string> ();
		useItemInfo ["iid"] = itemId;
		useItemInfo ["uname"] = currentUser.userName;
		useItemInfo ["token"] = currentUser.token;
		useItemInfo ["amount"] = amount;
		Loading.OnStart ();
		m_UseItemRequestParser.ParseURL(useItemURL, useItemInfo, () => {
			OnClientUseItemComplete(m_UseItemRequestParser.GetValues()[0]);
			Loading.OnStop ();
		}, (error) => {
			OnClientUseItemFail(error.errorContent);
			Loading.OnStop ();
		});
		if (currentUser.needUpdateAPI.IndexOf("{PARTY}") == -1) {
			currentUser.needUpdateAPI += "{PARTY}";
		}
		if (currentUser.needUpdateAPI.IndexOf("{LEVELUP}") == -1) {
			currentUser.needUpdateAPI += "{LEVELUP}";
		}
	}

	public void OnClientUseItemComplete(CConsumeItemData value) {
		Debug.Log ("CLIENT USE ITEM COMPLETE " + value.result);
//		m_UserInterface.OpenInventoryPanel ();
		MessageBox.Show ("COMPLETE !!", "You use item. \n" + value.result, null);
	}

	public void OnClientUseItemFail(string error) {
		Debug.Log ("CLIENT USE ITEM FAIL " + error);
		MessageBox.Show ("FAIL !!", "You can not use item. \n" + error,  null);
	}

	#endregion

	#region PVP

	public void OnClientEnterPVP(int page, int amount) {
		var pvpURL = string.Format (CTest9ServerInfo.GET_USER_CONNECT_PVP, page, amount, currentUser.userName, currentUser.token);
		Loading.OnStart ();
		m_PVPParser.ParseURL (pvpURL, () => {
			OnClientEnterPVPComplete(m_PVPParser.GetValues()[0]);
			Loading.OnStop ();
		}, (error) => {
			OnClientEnterPVPFail(error.errorContent);
			MessageBox.Show("ERROR", "LOAD PVP FAIL !!! " + error.errorContent, null);
			Loading.OnStop ();
		});
	}

	public void OnClientEnterPVPComplete(CPVPData value) {
		Debug.Log ("CLIENT ENTER PVP COMPLETE ");
		m_UserInterface.SetupPVPData (value.connects);
	}

	public void OnClientEnterPVPFail(string error) {
		Debug.Log ("CLIENT ENTER PVP FAIL " + error);
	}

	#endregion

	#region LevelUP Mercenary

	public void OnClientLoadAllMercenary() {
		Loading.OnStart ();
		if (currentUser != null) {
			if (currentUser.needUpdateAPI.IndexOf("{LEVELUP}") != -1) {
				var userMercenariesURL = string.Format (CTest9ServerInfo.GET_ALL_MERCENARIES, currentUser.userName, currentUser.token);
				m_UserMercenariesParser.ParseURL (userMercenariesURL, () => {
					currentUser.needUpdateAPI = currentUser.needUpdateAPI.Replace("{LEVELUP}", string.Empty);
					OnClientLoadAllMercenaryComplete();
					Loading.OnStop ();
				}, (error) => {
					OnClientLoadAllMercenaryFail (error.errorContent);	
					if (currentUser.needUpdateAPI.IndexOf("{LEVELUP}") == -1) {
						currentUser.needUpdateAPI += "{LEVELUP}";
					}
					MessageBox.Show("ERROR", "LOAD PARTY FAIL !!! " + error.errorContent, null);
					Loading.OnStop ();
				});
			} else {
				OnClientLoadAllMercenaryComplete();
				Loading.OnStop ();
			}
		} else {
			OnClientLoadAllMercenaryFail("ERROR Open Levelup. Error: User not correct !!");
			Loading.OnStop ();
		}
	}

	public void OnClientLoadAllMercenaryComplete() {
		var mercenaryData = m_UserMercenariesParser.GetValues ();
		m_UserInterface.SetupMaterialMercenaryData (mercenaryData);
	}

	public void OnClientLoadAllMercenaryFail(string error) {
		Debug.Log ("LOAD ALL MERCENARY FAIL " + error);
	}

	public void OnClientLevelUpMercenary(string mercenaryId, string material1, string material2, string material3) {
		Loading.OnStart ();
		var levelUpURL = CTest9ServerInfo.POST_LEVEL_UP_MERCENARY;
		var levelUpInfo = new Dictionary<string, string> ();
		levelUpInfo ["merid"] = mercenaryId;
		levelUpInfo ["mat1"] = material1;
		levelUpInfo ["mat2"] = material2;
		levelUpInfo ["mat3"] = material3;
		levelUpInfo ["uname"] = currentUser.userName;
		levelUpInfo ["token"] = currentUser.token;
		m_LevelUpParser.ParseURL (levelUpURL, levelUpInfo, () => {
			OnClientLevelUpComplete(m_LevelUpParser.GetValues()[0]);
			Loading.OnStop ();
		}, (error) => {
			OnClientLevelUpFail(error.errorContent);
			Debug.Log("LEVEL UP MERCENARY FAIL !! " +  error.errorContent);
			Loading.OnStop ();
		});
		if (currentUser.needUpdateAPI.IndexOf("{PARTY}") == -1) {
			currentUser.needUpdateAPI += "{PARTY}";
		}
		if (currentUser.needUpdateAPI.IndexOf("{LEVELUP}") == -1) {
			currentUser.needUpdateAPI += "{LEVELUP}";
		}
		if (currentUser.needUpdateAPI.IndexOf("{USERMAP}") == -1) {
			currentUser.needUpdateAPI += "{USERMAP}";
		}
	}

	public void OnClientLevelUpComplete(CLevelUpResponseData value) {
		Debug.Log("LEVEL UP MERCENARY COMPLETE !!");
		var result = value.result ? "LEVEL UP COMPLETE !!!" : "LEVEL UP FAIL !!!";
		MessageBox.Show("COMPLETE", result, () => {
			m_UserInterface.OpenLevelUpPanel ();
		});
	}

	public void OnClientLevelUpFail(string error) {
		Debug.Log ("LEVEL UP MERCENARY FAIL " + error);
		MessageBox.Show("FAIL", "LEVEL UP MERCENARY FAIL !! " +  error, null);
	}

	#endregion

	#region BattleLog

	public void OnClientCreateBattleLog(string mapid, bool success) {
		var battleLogURL = CTest9ServerInfo.POST_BATTLE_LOG;
		var battleInfo 			= new Dictionary<string, string> ();
		battleInfo ["mid"] 		= mapid;
		battleInfo ["battle"] 	= success ? "1" : "0";
		battleInfo ["pvp"] 		= currentUser.currentTarget != "{world_map_tiny_game}" ? "1" : "0";
		battleInfo ["uname"] 	= currentUser.userName;
		battleInfo ["token"] 	= currentUser.token;
		m_BattleLogParser.ParseURL (battleLogURL, battleInfo, () => {
			OnClienCreateBattleLogComplete(m_BattleLogParser.GetValues()[0]);
		}, (error) => {
			OnClientCreateBattleLogFail(error.errorContent);
			MessageBox.Show("ERROR", "CREATE BATTLE LOG FAIL !!! " + error.errorContent, null);
		});
	}

	public void OnClienCreateBattleLogComplete(CBattleLogData value) {
		Debug.Log ("CREATE BATTLE LOG COMPLETE " + value.awardCode);
		currentUser.currentAwardCode = value.awardCode;
		OnClientStartMatch ();
	}

	public void OnClientCreateBattleLogFail(string error) {
		Debug.Log ("CREATE BATTLE LOG FAIL " + error);
		OnClientEndMatch ();
	}

	public void OnClientClaimAward(string bid) {
		var battleLogURL 		= CTest9ServerInfo.POST_CLAIM_AWARD_BATTLE;
		var battleInfo			= new Dictionary<string, string> ();
		battleInfo ["bid"] 		= bid;
		battleInfo ["uname"] 	= currentUser.userName;
		battleInfo ["pvp"] 		= currentUser.currentTarget != "{world_map_tiny_game}" ? "1" : "0";
		battleInfo ["token"] 	= currentUser.token;
		m_BattleAwardParser.ParseURL (battleLogURL, battleInfo, () => {
			OnClientClaimAwardComplete(m_BattleAwardParser.GetValues()[0]);
		}, (error) => {
			OnClientClaimAwardFail(error.errorContent);
		});
	}

	public void OnClientClaimAwardComplete(CBattleAward value) {
		Debug.Log ("CLAIM AWARD COMPLETE ");
		m_UserGame.ShowFinishPanel (true);
		currentUser.gold += value.goldAward;
		MessageBox.Show ("YOU WIN !!", "Congratulation, you win !!! \n Gold award: " + value.goldAward, () => {
			m_UserGame.ShowFinishPanel (true);
		});
	}

	public void OnClientClaimAwardFail(string error) {
		Debug.Log ("CLAIM AWARD FAIL " + error);
		MessageBox.Show ("YOU WIN !!", "Congratulation, you win !!! \n BUT Error: " + error, () => {
			m_UserGame.ShowFinishPanel (true);
		});
	}

	#endregion

	#region LuckyWheel

	public void OnClientEnterLuckyWheel() {
		var luckyWheelURL = string.Format (CTest9ServerInfo.GET_LUCKY_WHEEL_TIME, currentUser.userName, currentUser.token);
		Loading.OnStart ();
		m_LuckyWheelParser.ParseURL (luckyWheelURL, () => {
			OnClientEnterLuckyWheelComplete(m_LuckyWheelParser.GetValues()[0]);	
			Loading.OnStop ();
		}, (error) => {
			OnClientEnterLuckyWheelFail(error.errorContent);
			Loading.OnStop ();
		});
	}

	public void OnClientEnterLuckyWheelComplete(CLuckyWheelResultData value) {
		Debug.Log ("ENTER LUCKY WHEEL COMPLETE " + value.result);
		var second 	= (float)value.nextTimes / 1000f / 60f;
		var nextTime = "00:00";
		if (second > 0) {
			var hours = ((int)(second / 60f)).ToString ("d2");
			var minute = ((int)(second % 60f)).ToString ("d2");
			nextTime = string.Format ("{0}:{1}", hours, minute);
			MessageBox.Show ("COMPLETE !!", "Enter lucky wheel \n" + value.result + "\n" + nextTime, () => {
				m_UserInterface.OpenUserInterface();
			});
		}
	}

	public void OnClientEnterLuckyWheelFail(string error) {
		Debug.Log ("ENTER LUCKY WHEEL FAIL " + error);
		MessageBox.Show ("ERROR !!", "ERROR, Enter lucky wheel fail, error: " + error, () => {
			m_UserInterface.OpenUserInterface();
		});
	}

	public void OnClientStartLuckyWheel() {
		var luckyWheelURL = CTest9ServerInfo.POST_SCROLL_LUCKY_WHEEL;
		Loading.OnStart ();
		var luckyWheelInfo		= new Dictionary<string, string> ();
		luckyWheelInfo ["uname"] 	= currentUser.userName;
		luckyWheelInfo ["token"] 	= currentUser.token;
		m_LuckyWheelParser.ParseURL (luckyWheelURL, luckyWheelInfo, () => {
			OnClientStartLuckyWheelComplete(m_LuckyWheelParser.GetValues()[0]);	
			Loading.OnStop ();
		}, (error) => {
			OnClientStartLuckyWheelFail(error.errorContent);
			Loading.OnStop ();
		});
	}

	public void OnClientStartLuckyWheelComplete(CLuckyWheelResultData value) {
		Debug.Log ("START LUCKY WHEEL COMPLETE " + value.result);
		MessageBox.Show ("COMPLETE !!", "START LUCKY WHEEL COMPLETE " + value.result, () => {
			m_UserInterface.OpenUserInterface();
		});
	}

	public void OnClientStartLuckyWheelFail(string error) {
		Debug.Log ("START LUCKY WHEEL FAIL " + error);
		MessageBox.Show ("ERROR !!", "ERROR, START lucky wheel fail, error: " + error, null);
	}

	#endregion

	#region GAME

	public void OnClientStartGame() {
		Debug.Log("START GAME !!!");
	}

	public void OnClientEnterMatch(string mapId, string target, bool pvp = false) {
		currentUser.currentMap = mapId;
		currentUser.currentTarget = target;
		m_GameScene = EGameScene.Game;
		CTest9Manager.OnPVP = pvp;
		SceneManager.LoadSceneAsync (m_UserGameScene);
	}

	public void OnClientStartMatch() {
		m_UserGame.ShowMainPanel ();
	}

	public void OnClientSetupMatch(string bg) {
		var spriteBG = Util.FindSprite (bg);
		m_UserGame.SetBG (spriteBG);
	}

	public void OnClientWinMatch() {
		OnClientClaimAward (currentUser.currentAwardCode);
	}

	public void OnClientCloseMatch() {
		m_UserGame.ShowFinishPanel (false);
	}

	public void OnClientEndMatch() {
		currentUser.currentMap = string.Empty;
		m_GameScene = EGameScene.UserInterface;
		SceneManager.LoadSceneAsync (m_UserInterfaceScene);
	}

	public void OnClientEndGame() {
		DestroyImmediate (this.gameObject);
		SceneManager.LoadSceneAsync (m_UserLoginScene);
		Loading.OnStop();
	}

	#endregion

}
