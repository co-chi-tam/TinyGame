using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CConnectControl : CControl {

	[Header("Connect Panel")]
	[SerializeField]	private GameObject m_ConnectPanel;
	[SerializeField]	private Button m_CreateHostButton;
	[SerializeField]	private Button m_JoinHostButton;
	[SerializeField]	private Button m_JoinServerButton;
	[SerializeField]	private InputField m_HostAddressInputField;
	[SerializeField]	private InputField m_CharacterNameInputField;
	[SerializeField]	private CWaitingControl m_WaitControl;
	[SerializeField]	private CServerListControl m_ServerListControl;

	public void ShowConnectPanel(bool value) {
		if (m_ConnectPanel != null) {
			m_ConnectPanel.SetActive (value);
		}
		if (value) {
			m_CreateHostButton.onClick.RemoveAllListeners ();
			m_CreateHostButton.onClick.AddListener (() => {
				m_WaitControl.ShowWaitingPanel(true, () => {
					CTest7LobbyManager.Instance.CancelCreateHost();
					ShowConnectPanel(true);
				});
				ShowConnectPanel(false);
				CTest7LobbyManager.Instance.CreateHost(() => {
					m_WaitControl.ShowWaitingPanel(false);
				});
			});
			m_JoinHostButton.onClick.RemoveAllListeners ();
			m_JoinHostButton.onClick.AddListener (() => {
				var ipAddress = string.IsNullOrEmpty(m_HostAddressInputField.text) ? "127.0.0.1" : m_HostAddressInputField.text;
				m_WaitControl.ShowWaitingPanel(true, () => {
					CTest7LobbyManager.Instance.CancelJoinHost(); 
					ShowConnectPanel(true);
				});
				ShowConnectPanel(false);
				CTest7LobbyManager.Instance.JoinHost(ipAddress, () => {
					m_WaitControl.ShowWaitingPanel(false);
				});
			});
			m_JoinServerButton.onClick.RemoveAllListeners ();
			m_JoinServerButton.onClick.AddListener (() => {
				ShowConnectPanel(false);
				m_ServerListControl.ShowServerListPanel(true, () => {
					ShowConnectPanel(true);
				});
			});
			m_CharacterNameInputField.text = "Player " + UnityEngine.Random.Range (1, 99);
		}
	}

	public string GetPlayerNameInput() {
		return m_CharacterNameInputField.text;
	}

}
