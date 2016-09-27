using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Threading;

public class CUITest7Manager : CMonoSingleton<CUITest7Manager> {

	[SerializeField]	private CPlayer m_Target;

	[Header("Main Game")]
	[SerializeField]	private GameObject m_MainGamePanel;
	[SerializeField]	private Text m_PlayingTimeText;
	[SerializeField]	private Text m_CurrentPlayerMoneyText;

	[Header("Place Info")]
	[SerializeField]	private CPlacePopControl m_PlacePopupControl;

	[Header ("Status")]
	[SerializeField]	private CPopupControl m_SuccessPanel;
	[SerializeField]	private CPopupControl m_FailPanel;

	[Header ("Money Info")]
	[SerializeField]	private CSimplePopupControl m_MoneyPopupControl;

	[Header ("Scroll Dice Info")]
	[SerializeField]	private CSimplePopupControl m_ScrollDicePopupControl;

	[Header ("End Game Panel")]
	[SerializeField]	private CEndGamePopUpControl m_EndGamePopupControl;

	private WaitForFixedUpdate m_WaitForFixedUpdate;
	private bool m_BrokenCountDown = false;

	protected override void Awake ()
	{
		base.Awake ();
		m_WaitForFixedUpdate = new WaitForFixedUpdate ();
		m_BrokenCountDown = false;
	}

	private object safe = new object();
	protected override void Start() {
		base.Start ();
		SetUpUI ();
	}

	public void SetUpUI() {
		ShowMainMenuPanel (true);
		ShowPlaceInfoPanel (false);
		ShowStatusPanel(false, false);
		ShowMoneyInfoPanel (false, Vector3.zero, 0f);
		ShowEndGamePanel (false, string.Empty, "0");
		ShowScrollDiveInfoPanel (false, Vector3.zero, 0);
	}

	public void ShowMainMenuPanel (bool value) {
		m_MainGamePanel.SetActive (value);
		if (value) {
			
		}
	}

	public void ShowPlaceInfoPanel(bool value, CPlace place = null, Action buy = null, Action next = null) {
//		m_PlacePopupControl.ShowInfo<CPlace> (value, 10f, place, buy, next);
		// testttttt
		m_PlacePopupControl.ShowInfo<CPlace> (value, 3f, place, buy, buy);
	}

	public void ShowStatusPanel(bool value, bool status) {
		if (value) {
			m_SuccessPanel.ShowFadeOut (status, Vector3.zero);
			m_FailPanel.ShowFadeOut (!status, Vector3.zero);
		} 
	}

	public void ShowMoneyInfoPanel(bool value, Vector3 position, float totalMoney) {
		var text = (totalMoney >= 0 ? "+" : "") + totalMoney;
		m_MoneyPopupControl.ShowInfo (value, position, text);
	}

	public void ShowScrollDiveInfoPanel(bool value, Vector3 position, int step) {
		m_ScrollDicePopupControl.ShowInfo (value, position, step.ToString());
	}

	public void ShowEndGamePanel(bool value, string winnerName, string currentAsset) {
		m_EndGamePopupControl.ShowInfo (value, winnerName, currentAsset, () => {
			CTest7LobbyManager.Instance.BackToLobby();
		});
	}

	public void SetTarget(CPlayer target) {
		m_Target = target;
	}

	public void SetPlayingGameTimeText(float value) {
		var minute = value / 60f;
		var second = value % 60f;
		m_PlayingTimeText.text = String.Format ("{0:00}:{1:00}", minute, second);
	}

	public void SetPlayerCurrentMoneyText(string value) {
		m_CurrentPlayerMoneyText.text = value;
	}

}
