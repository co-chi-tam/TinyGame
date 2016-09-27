using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class CTest7ManagerHook : NetworkBehaviour {

	#region Singleton

	protected static CTest7ManagerHook m_Instance;
	private static object m_SingletonObject = new object();
	public static CTest7ManagerHook Instance {
		get { 
			lock (m_SingletonObject) {
				if (m_Instance == null) {
					var resourceLoads = Resources.FindObjectsOfTypeAll<CTest7ManagerHook> ();
					GameObject go = null;
					if (resourceLoads.Length == 0) {
						go = new GameObject ();
						m_Instance = go.AddComponent<CTest7ManagerHook> ();
					} else {
						go = Instantiate (resourceLoads [0].gameObject);
						m_Instance = go.GetComponent<CTest7ManagerHook> ();
					}
					go.SetActive (true);
					go.name = typeof(CTest7ManagerHook).Name;
				}
				return m_Instance;
			}
		}
	}

	public static CTest7ManagerHook GetInstance() {
		return Instance;
	}

	#endregion

	private CTest7Manager m_Manager;
	private int m_OnClientAlreadyCount = 0;
	[SyncVar]	private bool m_IsStartedGame;

	protected virtual void Awake() {
		m_Instance = this;
		m_OnClientAlreadyCount = 0;
		m_IsStartedGame = false;
	}

	protected virtual void Start() {
		m_Manager = CTest7Manager.GetInstance ();
	}

	public override void OnStartServer ()
	{
		base.OnStartServer ();
	}

	[ServerCallback]
	public void OnStartGame() {
		m_OnClientAlreadyCount++;
		if (m_OnClientAlreadyCount == m_Manager.GetPlayerCount () && m_IsStartedGame == false) {
			CHandleEvent.Instance.AddEvent (3f, HandleOnStartGame ());
			m_IsStartedGame = true;
		}
	}

	private IEnumerator HandleOnStartGame() {
		yield return null;
		m_Manager.OnStartGame ();
	}

	public void AddPlayer(CPlayer player) {
		m_Manager = CTest7Manager.GetInstance ();
		m_Manager.AddPlayerQueue (player);
		player.SetPlayerId (m_Manager.GetPlayerCount ());
	}

	public void RemovePlayer(CPlayer player) {
		m_Manager = CTest7Manager.GetInstance ();
		m_Manager.RemobePlayerQueue (player);
	}

	public void GetNextTurn() {
		m_Manager = CTest7Manager.GetInstance ();
		m_Manager.GetNextTurn ();
	}

	[ClientRpc]
	public void RpcOnClientCreateApartment(int placeIndex, int id) {
		m_Manager.RpcOnClientCreateApartment (placeIndex, id);
	}

	[ClientRpc]
	public void RpcOnEndGame(string playerName, string currentMoney) {
		m_Manager.RpcOnEndGame (playerName, currentMoney);
	}

	[ServerCallback]
	public void OnGameTimeTick(float time) {
		RpcOnGameTimeTick (time);
	}

	[ClientRpc]
	public void RpcOnGameTimeTick(float time) {
		m_Manager.RpcOnGameTimeTick (time);
	}

}
