using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class CTes7UILobby : CMonoSingleton<CTes7UILobby> {

	[Header("Info")]
	[SerializeField]	private CTest7LobbyPlayer m_Target;

	[Header("Connect Panel")]
	[SerializeField]	private CConnectControl m_ConnectControl;

	[Header("Lobby Player Control")]
	[SerializeField]	private CLobbyPlayerControl m_LobbyPlayerControl;

	[Header("Chat Control")]
	[SerializeField]	private CChatControl m_ChatControl;

	[Header("Server List Control")]
	[SerializeField]	private CServerListControl m_ServerListControl;

	[Header("Waiting Control")]
	[SerializeField]	private CWaitingControl m_WaitControl;

	[Header("Countdown Panel")]
	[SerializeField]	private GameObject m_CountDownPanel;
	[SerializeField]	private Text m_CountDownText;

	protected override void Awake ()
	{
		base.Awake ();
	}

	private void Start() {
		SetupUI ();
	}

	private void OnLevelWasLoaded(int level) {
		SetupUI ();
	}

	private void SetupUI() {
		if (SceneManager.GetActiveScene ().name == "Test7Lobby") {
			m_ConnectControl.ShowConnectPanel (true);
		} else {
			m_ConnectControl.ShowConnectPanel (false);
		}
		m_LobbyPlayerControl.ShowChangeCharacterPanel (false);
		m_WaitControl.ShowWaitingPanel (false);
		m_ChatControl.ShowChatPanel (false);
		ShowCountDownPanel (false);
		m_ServerListControl.ShowServerListPanel (false);
	}

	public void ShowChangeCharacterPanel(bool value) {
		m_LobbyPlayerControl.ShowChangeCharacterPanel (value);
	}

	public void ShowCountDownPanel(bool value) {
		m_CountDownPanel.SetActive (value);
		if (value) {
			// TODO
		}
	}

	public void AddChat(string value) {
		m_ChatControl.AddChat (value, (count) => {
			m_LobbyPlayerControl.SetChatCountText (count.ToString());
		});
	}

	public CTest7UILobbyPlayer AddLobbyPlayer(CTest7LobbyPlayer player) {
		var lobbyUI = Instantiate (Resources.Load<CTest7UILobbyPlayer> ("Prefabs/Test7/Lobby/UILobbyPlayer"));

		lobbyUI.transform.SetParent (m_LobbyPlayerControl.GetLobbyPlayerParent());
		lobbyUI.transform.position = Vector3.zero;
		lobbyUI.transform.localScale = Vector3.one;

		lobbyUI.SetAvatarImage (player.GetCharacterIndex ());
		lobbyUI.SetPlayerName (player.GetPlayerName ());
		lobbyUI.SetBGImage (player.isLocalPlayer);
		lobbyUI.SetIsHost (player.isLocalPlayer);

		return lobbyUI;
	}

	public void RemoveLobbyPlayer(CTest7LobbyPlayer player, CTest7UILobbyPlayer ui) {
		if (ui != null && ui.gameObject != null) {
			Destroy (ui.gameObject);
		}
	}

	public void SetLobbyTarget (CTest7LobbyPlayer target) {
		m_Target = target;
		m_LobbyPlayerControl.SetTarget (target);
	}

	public void SetTimeCountdown(string value) {
		m_CountDownText.text = value;
	}

	public string GetPlayerName() {
		return m_ConnectControl.GetPlayerNameInput();
	}

}
