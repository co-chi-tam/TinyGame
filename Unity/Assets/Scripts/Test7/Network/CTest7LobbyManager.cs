using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class CTest7LobbyManager : NetworkLobbyManager {

	#region Singleton

	protected static CTest7LobbyManager m_Instance;
	private static object m_SingletonObject = new object();
	public static CTest7LobbyManager Instance {
		get { 
			lock (m_SingletonObject) {
				if (m_Instance == null) {
					var resourceLoads = Resources.LoadAll<CTest7LobbyManager> ("Prefabs");
					GameObject go = null;
					if (resourceLoads.Length == 0) {
						go = new GameObject ();
						m_Instance = go.AddComponent<CTest7LobbyManager> ();
					} else {
						go = Instantiate (resourceLoads [0].gameObject);
						m_Instance = go.GetComponent<CTest7LobbyManager> ();
					}
					go.SetActive (true);
					go.name = typeof(CTest7LobbyManager).Name;
				}
				return m_Instance;
			}
		}
	}

	public static CTest7LobbyManager GetInstance() {
		return Instance;
	}

	#endregion

	#region Internal Class

	public class CPlayerEntry
	{
		public CTest7LobbyPlayer lobbyPlayer;
		public CPlayer playerControl;
	}

	#endregion

	#region Properties

	public static bool OnServerStarted = false;
	public static bool OnGameServerStarted = false;
	private Dictionary<NetworkConnection, CPlayerEntry> m_LobbyPlayers;

	#endregion

	#region Event

	public Action OnServerStartEvent;

	#endregion

	#region Implementation Monobehaviour

	protected virtual void Awake() {
		m_Instance = this;
		this.m_LobbyPlayers = new Dictionary<NetworkConnection, CPlayerEntry> ();
	}

	#endregion

	#region Main methods

	public void LoadRoomList(int startPageNumber, int resultPageSize, string roomNameFilter, Action <UnityEngine.Networking.Match.ListMatchResponse> complete = null)  {
		this.StartMatchMaker ();
		this.matchMaker.ListMatches (startPageNumber, resultPageSize, roomNameFilter, delegate(UnityEngine.Networking.Match.ListMatchResponse response) {
			if (response.success) {
				if (complete != null) {
					complete (response);
				}
			}
		});
	}

	public void CreateRoom (string roomName, string password, Action<UnityEngine.Networking.Match.CreateMatchResponse> complete = null) {
		this.StartMatchMaker ();
		this.matchMaker.CreateMatch (roomName, (uint)maxPlayers, true, password, delegate(UnityEngine.Networking.Match.CreateMatchResponse response) {
			if (response.success) {
				if (complete != null) {
					complete (response);
				}
			}
		});
	}

	public void JoinRoom(UnityEngine.Networking.Types.NetworkID netId, string passWord, 
		Action<UnityEngine.Networking.Match.JoinMatchResponse> complete = null,
		Action lobbyAdded = null){
		this.StartMatchMaker ();
		this.matchMaker.JoinMatch (netId, passWord, delegate(UnityEngine.Networking.Match.JoinMatchResponse response) {
			if (response.success) {
				if (complete != null) {
					complete(response);
				}
				OnServerStartEvent -= lobbyAdded;
				OnServerStartEvent += lobbyAdded;
			}
		});
	}
		
	public void CreateHost (Action complete = null) {
		OnServerStartEvent -= complete;
		OnServerStartEvent += complete;
		this.networkPort = 7777;
		StartHost ();
	}

	public void JoinHost(string ip, Action complete = null) {
		OnServerStartEvent -= complete;
		OnServerStartEvent += complete;
		this.networkAddress = ip;
		this.networkPort = 7777;
		this.StartClient ();
	}

	public void CancelJoinHost() {
		this.StopClient ();
		this.SendReturnToLobby ();
	}

	public void CancelCreateHost() {
		this.StopHost ();
		this.SendReturnToLobby ();
	}

	public void BackToLobby() {
		this.StopMatchMaker ();
		this.StopServer ();
		this.StopHost ();
		this.StopClient ();
		this.SendReturnToLobby ();
	}

	public int GetPlayerLobbyCount() {
		return m_LobbyPlayers.Count;
	}

	public CTest7LobbyPlayer GetPlayerLobby(int index) {
		var count = 0;
		foreach (var item in m_LobbyPlayers) {
			if (count == index) {
				return item.Value.lobbyPlayer;			
			}
			count++;
		}	
		return null;
	}

	#endregion

	#region Server

	// ----------------- Server callbacks ------------------

	public override void OnLobbyStopHost ()
	{
		base.OnLobbyStopHost ();
		Debug.Log ("OnLobbyStopHost");
	}

	public override void OnStopHost ()
	{
		base.OnStopHost ();
		Debug.Log ("OnStopHost");
	}

	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
	{
		if (OnGameServerStarted == true)
			return;
		Debug.Log ("OnServerAddPlayer");
		base.OnServerAddPlayer (conn, playerControllerId);
	}

	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
	{
		if (OnGameServerStarted == true)
			return;
		Debug.Log ("OnServerAddPlayer");
		base.OnServerAddPlayer (conn, playerControllerId, extraMessageReader);
	}

	public override void OnLobbyServerConnect (NetworkConnection conn)
	{
		base.OnLobbyServerConnect (conn);
		Debug.Log ("OnLobbyServerConnect");
		if (OnServerStartEvent != null) {
			OnServerStartEvent ();
		}
		OnServerStarted = true;
	}

	public override GameObject OnLobbyServerCreateGamePlayer (NetworkConnection conn, short playerControllerId)
	{
		if (OnGameServerStarted == true)
			return null;
		Debug.Log ("OnLobbyServerCreateGamePlayer");
		var entry = m_LobbyPlayers [conn];
		entry.playerControl = Instantiate (Resources.Load<CPlayer>(entry.lobbyPlayer.GetCharacterPath()));
		entry.playerControl.SetPosition (Vector3.zero);
		entry.playerControl.SetPlayerName (entry.lobbyPlayer.GetPlayerName());
		entry.playerControl.SetIsPlaying (true);
		return entry.playerControl.gameObject;
	}

	public override GameObject OnLobbyServerCreateLobbyPlayer (NetworkConnection conn, short playerControllerId)
	{
		base.OnLobbyServerCreateLobbyPlayer (conn, playerControllerId);
		Debug.Log ("OnLobbyServerCreateLobbyPlayer");
		var entry = new CPlayerEntry ();
		entry.lobbyPlayer = Instantiate (lobbyPlayerPrefab) as CTest7LobbyPlayer;
		entry.lobbyPlayer.SetPosition (new Vector3 (m_LobbyPlayers.Count * 2f, 0f, 0f));
		entry.lobbyPlayer.SetUniqueID ("P" + (Guid.NewGuid().ToString()));
		m_LobbyPlayers.Add (conn, entry);
		return entry.lobbyPlayer.gameObject;
	}

	public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
	{
		Debug.Log ("OnLobbyServerSceneLoadedForPlayer");
		return true;
	}

	public override void OnServerSceneChanged (string sceneName)
	{
		base.OnServerSceneChanged (sceneName);
		if (sceneName == "Test7Play") {
			var hookManager = CTest7ManagerHook.GetInstance ();
			NetworkServer.Spawn (hookManager.gameObject);
			OnGameServerStarted = true;
		}
	}

	public override void OnStopServer ()
	{
		base.OnStopServer ();
		Debug.Log ("OnStopServer");
		m_LobbyPlayers.Clear ();
		OnServerStarted = false;
		OnGameServerStarted = false;
	}

	public override void OnLobbyServerDisconnect (NetworkConnection conn)
	{
		base.OnLobbyServerDisconnect (conn);
		Debug.Log ("OnLobbyServerDisconnect");
		var entry = m_LobbyPlayers [conn];
		m_LobbyPlayers.Remove (conn);
		CTest7ManagerHook.Instance.RemovePlayer (entry.playerControl);
		CTest7ManagerHook.Instance.GetNextTurn();
	}

	public override void OnLobbyServerPlayerRemoved (NetworkConnection conn, short playerControllerId)
	{
		base.OnLobbyServerPlayerRemoved (conn, playerControllerId);
		Debug.Log ("OnLobbyServerPlayerRemoved");
	}

	public override void OnLobbyServerPlayersReady ()
	{
		Debug.Log ("OnLobbyServerPlayersReady");
		CHandleEvent.Instance.AddEvent (HandleCountdownChangeScene((proc) => {
			foreach (var item in m_LobbyPlayers) {
				item.Value.lobbyPlayer.RpcShowCountDown(true, proc);
			}
		},() => {
			base.OnLobbyServerPlayersReady ();
		}));
	}

	private IEnumerator HandleCountdownChangeScene(Action<int> processing = null, Action complete = null) {
		var countDownInterval = 3f;
		var countDown = countDownInterval;
		var prevertCountDown = 0;
		while (countDown > 0f) {
			countDown -= Time.deltaTime;
			yield return WaitHelper.WaitFixedUpdate;
			if ((int)countDown != prevertCountDown) {
				prevertCountDown = (int)countDown;
				if (processing != null) {
					processing (prevertCountDown);
				}
			}
		}
		if (complete != null) {
			complete ();
		}
	}

	#endregion

	#region Client

	// ----------------- Client callbacks ------------------

	public override void OnLobbyClientConnect (NetworkConnection conn)
	{
		base.OnLobbyClientConnect (conn);
		Debug.Log ("OnLobbyClientConnect");
		if (OnServerStartEvent != null) {
			OnServerStartEvent ();
		}
	}

	public override void OnClientDisconnect (NetworkConnection conn)
	{
		base.OnClientDisconnect (conn);
		Debug.Log ("OnClientDisconnect");
	}

	public override void OnStopClient ()
	{
		base.OnStopClient ();
		Debug.Log ("OnStopClient");
	}

	public override void OnLobbyStopClient ()
	{
		base.OnLobbyStopClient ();
		Debug.Log ("OnLobbyStopClient");
	}

	public override void OnClientSceneChanged (NetworkConnection conn)
	{
		base.OnClientSceneChanged (conn);
		Debug.Log ("OnClientSceneChanged");
	}

	#endregion

	#region Getter && Setter 

	public CTest7LobbyPlayer GetPlayerLobbyBaseConnection(NetworkConnection conn) {
		return m_LobbyPlayers [conn].lobbyPlayer;
	}

	#endregion

}
