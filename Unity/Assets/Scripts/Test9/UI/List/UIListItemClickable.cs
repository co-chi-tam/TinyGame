using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class UIListItemClickable : UIListItem {

	private Button m_ClickableButton;

	protected override void Awake ()
	{
		base.Awake ();
		m_ClickableButton = this.GetComponent<Button> ();
		if (m_ClickableButton != null) {
			// TODO
		} else {
			m_ClickableButton = this.gameObject.AddComponent<Button> ();
		}
		m_ClickableButton.onClick.RemoveAllListeners ();
		m_ClickableButton.onClick.AddListener (() => {
			OnItemClick(Input.mousePosition);
		});
	}

	protected virtual void OnItemClick (Vector2 position)
	{
		if (OnEventItemClicked != null) {
			OnEventItemClicked (index, position, this);
		}
	}

}
