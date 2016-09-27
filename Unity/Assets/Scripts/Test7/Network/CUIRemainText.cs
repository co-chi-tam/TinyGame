using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CUIRemainText : InputField {

	private const string m_Key = "UI_REMAIN_TEXT";

	protected override void Start ()
	{
		base.Start ();
		var remainText = PlayerPrefs.GetString (m_Key, string.Empty);
		this.text = remainText;
	}

	public override void OnDeselect (UnityEngine.EventSystems.BaseEventData eventData)
	{
		base.OnDeselect (eventData);
		PlayerPrefs.SetString (m_Key, this.text);
		PlayerPrefs.Save ();
	}

}
