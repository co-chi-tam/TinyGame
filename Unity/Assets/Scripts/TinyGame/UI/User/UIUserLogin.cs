using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIUserLogin : CBaseMonobehaviour {

	[Header("User Login")]
	[SerializeField]	private GameObject m_UserInfoCanvas;
	[SerializeField]	private GameObject m_UserInfoPanel;
	[SerializeField]	private InputField m_UserNameInputField;
	[SerializeField]	private InputField m_UserPasswordInputField;
	[SerializeField]	private Button m_LoginButton;
	[SerializeField]	private Button m_FBLoginButton;
	[Header("Active User")]
	[SerializeField]	private GameObject m_UserDisplayNamePanel;
	[SerializeField]	private InputField m_UserDisplayNameInputField;
	[SerializeField]	private Button m_ActiveUserButton;

	private CTest9UserManager m_UserManager;

	protected override void Start ()
	{
		base.Start ();
		m_UserManager = CTest9UserManager.GetInstance ();
	}

	public void Login() {
		var userName = m_UserNameInputField.text;
		var userPassword = m_UserPasswordInputField.text;
		if (string.IsNullOrEmpty (userName)) {
			SetUserWarningText ("Please, Field Username !!");
		} else if (string.IsNullOrEmpty (userPassword)) {
			SetUserWarningText("Please, Field Password !!");
		} else {
			m_UserInfoPanel.SetActive (false);
			m_UserManager.OnClientLogin (userName, userPassword);
		}
	}

	public void ActiveUser() {
		var displayName = m_UserDisplayNameInputField.text;
		if (string.IsNullOrEmpty (displayName)) {
			SetUserWarningText ("Please, Field Your name !!");
		} if (displayName.Length < 5 || displayName.Length > 18) {
			SetUserWarningText ("Please, Your name must be greater 5 and lower 18 character. !!");
		} else {
			m_UserDisplayNamePanel.SetActive (false);
			m_UserManager.OnClientActiveUser (displayName);
		}
	}

	public void Logout() {
		m_UserManager.OnClientLogout ();
	}

	public void SetUserWarningText(string text) {
		MessageBox.Show ("Warning !!", text, null);
	}

	public void SetUserDisplayNameText(string text) {
		m_UserNameInputField.text = text;
	} 

	public void SetUserInfoActive(bool value) {
		m_UserInfoPanel.SetActive (value);
	}

	public void SetDisplayNameUserActive (bool value) {
		m_UserDisplayNamePanel.SetActive (value);
	}

}
