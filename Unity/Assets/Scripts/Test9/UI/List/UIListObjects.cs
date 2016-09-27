using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIListObjects : CBaseMonobehaviour {

	[SerializeField]	protected GameObject m_Root;
	[SerializeField]	protected GameObject m_Content;
	[SerializeField]	protected UIListItem m_ChildPrefab;

	public int length;
	public Action<int, Vector2, UIListItem> OnEventItemClicked;

	protected UIListItem[] m_ChildList;

	public virtual void OnListAlready() {
		m_ChildPrefab.SetActive (true);
		m_ChildList = new UIListItem[length];
		for (int i = 0; i < m_ChildList.Length; i++) {
			var child = OnListItemLoad (i);
			m_ChildList [i] = child;
			OnListItemUpdate (i, child);
		}
		m_ChildPrefab.SetActive (false);
	}

	public virtual UIListItem OnListItemLoad(int index) {
		var child = GameObject.Instantiate (m_ChildPrefab);
		child.transform.SetParent (m_Content.transform);
		child.transform.position = Vector3.zero;
		child.transform.localScale = Vector3.one;
		child.index = index;
		child.OnEventItemClicked 	-= OnListItemClicked;
		child.OnEventItemClicked 	+= OnListItemClicked;
		return child;
	}

	public virtual void OnListItemUpdate(int index, UIListItem item) { 

	}

	public virtual void OnListItemClicked(int index, Vector2 position, UIListItem item) {
		if (OnEventItemClicked != null) {
			OnEventItemClicked(index, position, item);
		}
	}

	public virtual void OnListClear() {
		if (m_ChildList == null)
			return;
		for (int i = 0; i < m_ChildList.Length; i++) {
			var child = m_ChildList [i];
			child.OnEventItemClicked -= OnListItemClicked;
			if (child.gameObject != null) {
				DestroyImmediate (child.gameObject);
			}
		}
		m_ChildList = null;
	}

}
