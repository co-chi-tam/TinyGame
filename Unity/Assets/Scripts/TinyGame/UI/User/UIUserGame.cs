using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIUserGame : CBaseMonobehaviour {

	[Header("User Info")]
	[SerializeField]	private GameObject m_UserGamePanel;
	[SerializeField]	private GameObject m_UserMainPanel;
	[SerializeField]	private GameObject m_UserWinPanel;
	[SerializeField]	private GameObject m_UserClosePanel;
	[SerializeField]	private SpriteRenderer m_BG;

	private CTest9UserManager m_UserManager;
	private CTest9Manager m_GameManager;

	protected override void Start ()
	{
		base.Start ();
		m_UserManager = CTest9UserManager.GetInstance ();
		m_GameManager = CTest9Manager.GetInstance ();
	}

	public void ShowMainPanel() {
		m_UserMainPanel.SetActive (true);
	}

	public void StartMatch() {
		m_GameManager.GetNextTurn (); 
	}

	public void ShowFinishPanel(bool value) {
		m_UserWinPanel.SetActive (value);
		m_UserClosePanel.SetActive (!value);
		m_UserMainPanel.SetActive (false);
	}

	public void BackUserInterface() {
		m_UserManager.OnClientEndMatch ();
	}

	public void SetBG(Sprite sprite) {
		m_BG.sprite = sprite;
	}

}
