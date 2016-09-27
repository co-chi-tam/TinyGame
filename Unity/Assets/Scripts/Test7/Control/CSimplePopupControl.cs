using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CSimplePopupControl : CPopupControl {

	[Header ("Pop Info")]
	[SerializeField]	private Text m_PopupInfoText;

	public void ShowInfo(bool value, Vector3 position, string text) {
		m_Content.SetActive (value);
		if (value) {
			var screenPosition = Camera.main.WorldToScreenPoint (position);
			m_Content.transform.position = screenPosition;
			m_PopupInfoText.text = text;
			ShowFadeIn (value, Vector3.up, 20f, 0.5f, () => {
				ShowInfo (false, position, text);
			});
		}
	}

}
