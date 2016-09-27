using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CEndGamePopUpControl : CPopupControl {

	[Header ("End Game Panel")]
	[SerializeField]	private Text m_WinnerNameText;
	[SerializeField]	private Text m_CurrrentAssetText;
	[SerializeField]	private Button m_BackLobbyButton;

	public virtual void ShowInfo(bool value, string winnerName, string currentAsset, System.Action exit = null) {
		m_Content.SetActive (value);
		if (value) {
			m_WinnerNameText.text = winnerName;
			m_CurrrentAssetText.text = currentAsset;
			m_BackLobbyButton.onClick.RemoveAllListeners ();
			m_BackLobbyButton.onClick.AddListener (() => {
				if (exit != null) {
					exit();
				}
			});
			ShowFadeIn (true, new Vector3(0f, 1f, 0f));
		}
	}

}
