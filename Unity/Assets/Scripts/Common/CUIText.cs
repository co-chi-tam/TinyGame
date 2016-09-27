using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CUIText : Text {

	private string m_Pattern = "...";

	protected override void Awake ()
	{
		base.Awake ();
	}

	public virtual void SetTextBaseWidth(string value) {
		if (this.horizontalOverflow == HorizontalWrapMode.Overflow) {
			this.text = value;
		} else {
			this.text = UpdateTextByLength (value);
		}
	}

	private string UpdateTextByLength (string value) {
		var tmpValue = value;
		var widthLength = this.rectTransform.sizeDelta.x;
		var patternWidthLength = GetPreferredWidth (m_Pattern);
		var stringResult = string.Empty;
		for (int i = 0; i < tmpValue.Length; i++) {
			stringResult += tmpValue [i];
			var currentLength = GetPreferredWidth (stringResult) + patternWidthLength;
			if (currentLength > widthLength - patternWidthLength) {
				break;
			}
		}
		return stringResult + m_Pattern;
	}

	private float GetPreferredWidth(string value) {
		var currentText = this.text;
		this.text = value;
		var widthLength = this.preferredWidth;
		this.text = currentText;
		return widthLength;
	}

}
