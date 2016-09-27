using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;

public class CLobbyPlayerControl : CControl {

	[Header("Character Main Panel")]
	[SerializeField]	private GameObject m_CharacterMainPanel;
	[SerializeField]	private Button m_PrevertButton;
	[SerializeField]	private Button m_NextButton;
	[SerializeField]	private Button m_ActiveChatButton;
	[SerializeField]	private Text m_ChatCountText;
	[SerializeField]	private Button m_ReadyButton;
	[SerializeField]	private GameObject m_ListLobbyPlayer;
	[SerializeField]	private Button  m_BackLobbyButton;
	[SerializeField]	private CChatControl m_ChatControl;
	[SerializeField]	private CConnectControl m_ConnectControl;

	private CTest7LobbyPlayer m_Target;

	public void ShowChangeCharacterPanel(bool value, Action back = null) {
		m_CharacterMainPanel.SetActive (value);
		if (value) {
			m_PrevertButton.onClick.RemoveAllListeners ();
			m_PrevertButton.onClick.AddListener (() => {
				if (m_Target != null) {
					m_Target.Prevert();
				}
			});
			m_NextButton.onClick.RemoveAllListeners ();
			m_NextButton.onClick.AddListener (() => {
				if (m_Target != null) {
					m_Target.Next();
				}
			});
			m_ActiveChatButton.onClick.RemoveAllListeners ();
			m_ActiveChatButton.onClick.AddListener (() => {
				m_ChatControl.ShowChatPanel(!m_ChatControl.OnShowChat, m_Target.CmdPlayerSubmitChat);
			});
			m_ReadyButton.onClick.RemoveAllListeners ();
			m_ReadyButton.onClick.AddListener (() => {
				m_Target.SendReadyToBeginMessage();
			});
			if (m_Target != null) {
				m_Target.SetPlayerName (m_ConnectControl.GetPlayerNameInput());
			}
			m_BackLobbyButton.onClick.RemoveAllListeners ();
			m_BackLobbyButton.onClick.AddListener (() => {
				CTest7LobbyManager.Instance.BackToLobby();
				ShowChangeCharacterPanel (false);
				if (back != null) {
					back();
				}
			});
		}
		m_ChatCountText.text = "0";
	}

	public void SetTarget(CTest7LobbyPlayer player) {
		m_Target = player;
	}

	public Transform GetLobbyPlayerParent() {
		return m_ListLobbyPlayer.transform;
	} 

	public void SetChatCountText(string value) {
		m_ChatCountText.text = value;
	}

}
