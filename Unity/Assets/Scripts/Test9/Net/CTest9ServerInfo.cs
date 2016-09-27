using System;
using System.Collections;
using System.Collections.Generic;

public class CTest9ServerInfo {

	public static string HOST 						= "https://tamco-tinygame.rhcloud.com/";
	public static string POST_LOGIN 				= HOST + "login";
	public static string POST_LOGOUT 				= HOST + "logout";
	public static string POST_ACTIVE_USER 			= HOST + "active_user";
	public static string POST_COMPLETE_TUTORIAL		= HOST + "complete_tutorial_user";
	public static string GET_GOOD_LOGIN_TOKEN 		= HOST + "api/good_login_token?uname={0}&macadd={1}&token={2}";
	public static string GET_ALL_SODIER 			= HOST + "api/all_sodier?token={0}";
	public static string GET_SODIER 				= HOST + "api/sodier?sid={0}&token={1}";
	public static string GET_MAP_SLOT 				= HOST + "api/map?mid={0}&token={1}";
	public static string GET_MAP_FORMATION 			= HOST + "api/map_formation?mid={0}&token={1}";
	public static string GET_USER_MAP_FORMATION		= HOST + "api/user_map_formation?uname={0}&token={1}";
	public static string POST_CREATE_MAP_FORMATION 	= HOST + "api/create/user_map_formation";
	public static string POST_UPDATE_MAP_FORMATION 	= HOST + "api/update/user_map_formation";
	public static string POST_CREATE_MERCENARY 		= HOST + "api/create/mercenary";
	public static string GET_ALL_MERCENARIES		= HOST + "api/all_mercenary?uname={0}&token={1}";
	public static string GET_SHOP					= HOST + "api/shop?token={0}";
	public static string POST_BUY_ITEM_GOLD			= HOST + "api/buy_item_gold";
	public static string GET_INVENTORY				= HOST + "api/inventory?uname={0}&token={1}";
	public static string POST_USE_ITEM_INVENTORY    = HOST + "api/use/inventory";
	public static string GET_USER_CONNECT_PVP		= HOST + "api/connect_pvp?page={0}&amount={1}&uname={2}&token={3}";
	public static string GET_PVP_MAP_FORMATION		= HOST + "api/pvp_map_formation?mapid={0}&pvpuname={1}&uname={2}&token={3}";
	public static string POST_BATTLE_LOG			= HOST + "api/create/battle_log";
	public static string POST_CLAIM_AWARD_BATTLE	= HOST + "api/update/battle_log";
	public static string POST_LEVEL_UP_MERCENARY	= HOST + "api/update/level_up/mercenary";
	public static string GET_LUCKY_WHEEL_TIME		= HOST + "api/time/lucky_wheel?uname={0}&token={1}";
	public static string POST_SCROLL_LUCKY_WHEEL = HOST + "api/scroll/lucky_wheel";

	private static Dictionary<string, string> headerValues;
	public static Dictionary<string, string> getHeaderValues() {
		if (headerValues == null) {
			headerValues = new Dictionary<string, string> ();
		}
		if (headerValues.ContainsKey ("verify") == false) {
			headerValues.Add ("verify", "TinyGame");
		}
		return headerValues;
	}
}
