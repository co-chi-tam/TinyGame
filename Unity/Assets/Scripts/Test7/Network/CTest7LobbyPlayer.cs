using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;

public class CTest7LobbyPlayer : NetworkLobbyPlayer {

	[SerializeField]	private int m_CharacterLength = 2;
	[SerializeField]	[SyncVar] private int m_SelectCharacterIndex = 0;
	[SerializeField]	[SyncVar] private string m_PlayerName = "";
	[SerializeField]	private string m_UniqueID = "";
	[SerializeField]	private bool m_IsReady = false;

	private CTes7UILobby m_UIManager;
	private CTest7UILobbyPlayer m_LobbyUI;
	private Transform m_Transform;
	private string[] m_Characters = new string[]{ "Prefabs/Test7/Character Male", "Prefabs/Test7/Character Female" };

	private void Awake() {
		m_Transform = this.transform;
		m_CharacterLength = m_Characters.Length;
	}

	private void Start() {
		m_UIManager = CTes7UILobby.GetInstance ();
		this.transform.SetParent (CTest7LobbyManager.Instance.transform);
		m_UIManager = CTes7UILobby.GetInstance ();
		m_LobbyUI = m_UIManager.AddLobbyPlayer (this);
		m_LobbyUI.SetPlayerName (m_PlayerName);
		m_LobbyUI.SetAvatarImage (m_SelectCharacterIndex);
		m_LobbyUI.SetReady (m_IsReady);
	}

	public override void OnStartLocalPlayer ()
	{
		base.OnStartLocalPlayer ();
		m_UIManager = CTes7UILobby.GetInstance ();
		m_UIManager.SetLobbyTarget (this);
		m_UIManager.ShowChangeCharacterPanel (true);
	}

	public override void OnStartClient ()
	{
		base.OnStartClient ();
		CmdUpdatePlayerLobby ();
	}

	public override void OnClientReady (bool readyState)
	{
		base.OnClientReady (readyState);
		CmdPlayerReadyChange (readyState);
		Debug.Log ("OnClientReady " + readyState);
	}

	public override void OnClientExitLobby ()
	{
		base.OnClientExitLobby ();
		OnDestroyUI (); 
	}

	public override void OnNetworkDestroy ()
	{
		base.OnNetworkDestroy ();
		Debug.Log ("OnNetworkDestroy");
		OnDestroyUI (); 
	}

	public virtual void OnDestroy() {
		Debug.Log ("OnDestroy");
	}

	public virtual void OnDestroyUI(){
		m_UIManager = CTes7UILobby.GetInstance ();
		m_UIManager.RemoveLobbyPlayer (this, m_LobbyUI);
	}

	public void Next() {
		m_SelectCharacterIndex = (m_SelectCharacterIndex + 1) % m_CharacterLength;
		CmdSelectCharacterIndex (m_SelectCharacterIndex);
	}

	public void Prevert() {
		m_SelectCharacterIndex = (m_SelectCharacterIndex - 1) < 0 ? m_CharacterLength - 1 : m_SelectCharacterIndex - 1;
		CmdSelectCharacterIndex (m_SelectCharacterIndex);
	}

	[ClientRpc]
	public void RpcShowCountDown(bool show, int value) {
		m_UIManager.ShowCountDownPanel (show);
		m_UIManager.SetTimeCountdown (value.ToString());
	}

	[Command]
	public void CmdPlayerReadyChange(bool value) {
		m_IsReady = value;
		RpcPlayerReadyChange (value);
	}

	[ClientRpc]
	public void RpcPlayerReadyChange(bool value) {
		m_IsReady = value;
		m_LobbyUI.SetReady (value);
	}

	[Command]
	public void CmdPlayerSubmitChat(string value) {
		RpcPlayerSubmitChat (value);
	}

	[ClientRpc]
	public void RpcPlayerSubmitChat(string value) {
		var color = isLocalPlayer ? "#1300FFFF" : "#FFA300FF";
		var chat = "<color='" + color + "'>" + m_PlayerName + "</color>: " + value;
		CTes7UILobby.Instance.AddChat (chat);
	}

	[Command]
	public void CmdUpdatePlayerLobby() {
		RpcUpdatePlayerLobby ();
	}

	[ClientRpc]
	public void RpcUpdatePlayerLobby() {
		
	}

	[Command]
	public void CmdSelectCharacterIndex(int value) {
		RpcSelectCharacterIndex (value);
	}

	[ClientRpc]
	public void RpcSelectCharacterIndex(int value) {
		m_SelectCharacterIndex = value;
		m_LobbyUI.SetAvatarImage (value);
	}

	public int GetCharacterIndex() {
		return m_SelectCharacterIndex;
	}

	public string GetCharacterPath() {
		return m_Characters [m_SelectCharacterIndex];
	}

	[ServerCallback]
	public void SetPosition(Vector3 position) {
		m_Transform.position = position;
		RpcSetPosition (position);
	}

	[ClientRpc]
	public void RpcSetPosition(Vector3 position) {
		m_Transform.position = position;
	}

	public void SetPlayerName(string value) {
		m_PlayerName = value;
		this.name = value;
		CmdSetPlayerName (value);
	}

	[Command]
	public void CmdSetPlayerName(string value) {
		m_PlayerName = value;
		this.name = value;
		RpcSetPlayerName (value);
	}

	[ClientRpc]
	public void RpcSetPlayerName(string value) {
		m_PlayerName = value;
		this.name = value;
		m_LobbyUI.SetPlayerName (value);
	}

	public string GetPlayerName() {
		return m_PlayerName;
	}

	[ServerCallback]
	public void SetUniqueID(string value) {
		m_UniqueID = value;
		RpcSetPlayerName (value);
	}

	[ClientRpc]
	public void RpcSetUniqueID(string value) {
		m_UniqueID = value;
	}

	public string GetUniqueID() {
		return m_UniqueID;
	}

}
