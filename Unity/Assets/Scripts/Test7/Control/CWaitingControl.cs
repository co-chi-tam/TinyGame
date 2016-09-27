using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CWaitingControl : CControl {

	[Header("Waiting Panel")]
	[SerializeField]	private GameObject m_WaitingPanel;
	[SerializeField]	private Button  m_CancelWaitingButton;

	public void ShowWaitingPanel(bool value, Action onCancel = null) {
		if (m_WaitingPanel != null) {
			m_WaitingPanel.SetActive (value);
		}
		if (value) {
			m_CancelWaitingButton.onClick.RemoveAllListeners ();
			m_CancelWaitingButton.onClick.AddListener (() => {
				if (onCancel != null) {
					onCancel();
				}
				ShowWaitingPanel(false);
			});
		}
	}

}
