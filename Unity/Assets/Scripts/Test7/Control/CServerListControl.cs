using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Match;
using System;
using System.Collections;
using System.Collections.Generic;

public class CServerListControl : CControl {

	[Header("Server List Panel")]
	[SerializeField]	private GameObject m_ServerListPanel;
	[SerializeField] 	private Button m_NextServerPageButton;
	[SerializeField]	private Button m_PrevertServerPageButton;
	[SerializeField]	private InputField m_CreateRoomNameInputfield;
	[SerializeField]	private Button m_CreateRoomButton;
	[SerializeField]	private Button m_BackButton;
	[SerializeField]	private Text m_PageNumberText;
	[SerializeField]	private CWaitingControl m_WaitingControl;
	[SerializeField]	private CTest7UIRoom[] m_ServerRoomList;

	private List<MatchDesc> m_RoomList;
	private int m_CurrentPage = 0;
	private bool m_NeedRefesh = false;
	private float m_TimeRefeshInterval = 10f;
	private float m_TimeRefesh = 0f;

	protected override void Start ()
	{
		base.Start ();
		m_NeedRefesh = false;
		m_TimeRefesh = Time.time;
	} 

	protected override void UpdateBaseTime (float dt)
	{
		base.UpdateBaseTime (dt);
		if (m_NeedRefesh) {
			if ((Time.time - m_TimeRefesh) > m_TimeRefeshInterval) {
				LoadServerRoomList (false);
				m_TimeRefesh = Time.time;
			}
		}
	}

	public void ShowServerListPanel(bool value, Action back = null) {
		m_ServerListPanel.SetActive (value);
		m_NeedRefesh = value;
		if (value) {
			m_PageNumberText.text = (m_CurrentPage + 1).ToString ();

			m_NextServerPageButton.onClick.RemoveAllListeners ();
			m_NextServerPageButton.onClick.AddListener (() => {
				NextPage();
			});
			m_PrevertServerPageButton.onClick.RemoveAllListeners ();
			m_PrevertServerPageButton.onClick.AddListener (() => {
				PrevertPage();
			});
			m_CreateRoomNameInputfield.text = "Room " + UnityEngine.Random.Range (1, 99);
			m_CreateRoomButton.onClick.RemoveAllListeners ();
			m_CreateRoomButton.onClick.AddListener (() => {
				var roomName = string.IsNullOrEmpty (m_CreateRoomNameInputfield.text) ? "Room " + UnityEngine.Random.Range (1, 99) : m_CreateRoomNameInputfield.text;
				m_WaitingControl.ShowWaitingPanel (true);
				CTest7LobbyManager.Instance.CreateRoom (roomName, "", (response) => {
					CTest7LobbyManager.Instance.OnMatchCreate(response);
					m_CreateRoomNameInputfield.text = string.Empty;
					m_WaitingControl.ShowWaitingPanel (false);
					ShowServerListPanel (false);
					m_NeedRefesh = false;
				});
			});
			m_BackButton.onClick.RemoveAllListeners ();
			m_BackButton.onClick.AddListener (() => {
				CTest7LobbyManager.Instance.StopMatchMaker();
				ShowServerListPanel(false);
				if (back != null) {
					back();
				}
			});
			LoadServerRoomList (true);
		}
	}

	public void LoadServerRoomList(bool showWaiting = false) {
		if (showWaiting) {
			m_WaitingControl.ShowWaitingPanel (true);
		}
		CTest7LobbyManager.Instance.LoadRoomList (0, 125, "", (response) => {
			m_RoomList = new List<MatchDesc> (response.matches);
			if (showWaiting) {
				m_WaitingControl.ShowWaitingPanel (false);
			}
			LoadPage(m_CurrentPage);
		});
	}

	public void NextPage() {
		m_CurrentPage = (m_CurrentPage + 1) % 20;
		m_PageNumberText.text = (m_CurrentPage + 1).ToString ();
		LoadPage (m_CurrentPage);
	}

	public void PrevertPage() {
		m_CurrentPage = (m_CurrentPage - 1) < 0 ? 19 : m_CurrentPage - 1;
		m_PageNumberText.text = (m_CurrentPage + 1).ToString ();
		LoadPage (m_CurrentPage);
	}

	public void LoadPage(int page) {
		var index = page * m_ServerRoomList.Length;
		var max = index + m_ServerRoomList.Length;
		for (int i = index; i < max; i++) {
			var roomIndex = i % m_ServerRoomList.Length;
			var ui = m_ServerRoomList [roomIndex];
			ui.gameObject.SetActive (false);
			if (i >= m_RoomList.Count)
				continue;
			var room = m_RoomList [i];
			ui.gameObject.SetActive (true);
			ui.roomNameText.text = room.name;
			ui.roomMemberText.text = room.currentSize + "/" + room.maxSize;
			ui.joinRoomButton.onClick.RemoveAllListeners ();
			ui.joinRoomButton.onClick.AddListener (() => {
				m_WaitingControl.ShowWaitingPanel (true);
				CTest7LobbyManager.Instance.JoinRoom (room.networkId, "", (response) => {
					CTest7LobbyManager.Instance.OnMatchJoined (response);
					ShowServerListPanel (false);
					m_NeedRefesh = false;
				}, () => {
					m_WaitingControl.ShowWaitingPanel (false);
				});
			});
		}
	}

}
