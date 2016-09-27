using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class CChatControl : CControl {

	[Header("Chat Panel")]
	[SerializeField]	private GameObject m_ChatPanel;
	[SerializeField]	private Text m_ChatBoardText;
	[SerializeField]	private InputField m_ChatInputField;
	[SerializeField]	private Button m_SubmitChatButton;

	private List<string> m_ChatList;

	public bool OnShowChat = false;

	protected override void Awake ()
	{
		base.Awake ();
		m_ChatList = new List<string> ();
		OnShowChat = m_ChatPanel.activeInHierarchy;
	}

	public void ShowChatPanel(bool value, Action<string> submitChat = null) {
		m_ChatPanel.SetActive (value);
		OnShowChat = value;
		if (value) {
			m_SubmitChatButton.onClick.RemoveAllListeners ();
			m_SubmitChatButton.onClick.AddListener (() => {
				if (string.IsNullOrEmpty (m_ChatInputField.text) == false) {
					if (submitChat != null) {
						submitChat(m_ChatInputField.text);
					}
					m_ChatInputField.text = string.Empty;
				}
			});
		}
	}

	public void AddChat(string value, Action<int> chatCount = null) {
		m_ChatList.Add (value);
		var index = m_ChatList.Count > 9 ? m_ChatList.Count - 10 : 0;
		var chatBoard = string.Empty;
		for (int i = index; i < m_ChatList.Count; i++) {
			chatBoard += m_ChatList[i] + "\n";
		}
		m_ChatBoardText.text = chatBoard;
		if (chatCount != null) {
			chatCount (m_ChatList.Count < 99 ? m_ChatList.Count : 99);
		}
	}

}
