using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CMessageItem {

	public string prefabPath;
	public string title;
	public string content;

	protected EventListener OnPositiveClick;
	protected EventListener OnNegativeClick;
	protected UIMessageBox m_UIInstance;

	public CMessageItem ()
	{
		this.prefabPath = "MessageBoxPanel";
		this.title = string.Empty;
		this.content = string.Empty;
		OnPositiveClick = new EventListener ();
		OnNegativeClick = new EventListener ();
	}

	public virtual void AddPositiveListener(Action listener) {
		this.OnPositiveClick.AddListener (listener);
	}

	public virtual void RemovePositiveListener(Action listener) {
		this.OnPositiveClick.RemoveListener(listener);
	}

	public virtual void RemoveAllPositiveListener() {
		this.OnPositiveClick.RemoveAllListener();
	}

	public virtual void AddNegativeListener(Action listener) {
		this.OnNegativeClick.AddListener (listener);
	}

	public virtual void RemoveNegativeListener(Action listener) {
		this.OnNegativeClick.RemoveListener (listener);
	}

	public virtual void RemoveAllNegativeListener() {
		this.OnNegativeClick.RemoveAllListener();
	}

	public virtual void Repair(GameObject parent = null) {
		var findComponent = FindMessageBoxItem (prefabPath);
		if (findComponent != null) {
			m_UIInstance = GameObject.Instantiate (findComponent);
			m_UIInstance.title = title;
			m_UIInstance.content = content;
			m_UIInstance.AddPositiveListener (OnPositiveClick.GetListener());
			m_UIInstance.AddNegativeListener (OnNegativeClick.GetListener());
			var rectTransform = m_UIInstance.transform as RectTransform;
			if (parent != null) {
				rectTransform.SetParent (parent.transform);
			}
			rectTransform.localPosition = Vector3.zero;
			rectTransform.localScale = Vector3.one;
			rectTransform.anchoredPosition = Vector2.zero;
			rectTransform.sizeDelta = Vector2.zero;
		}
	}

	protected virtual UIMessageBox FindMessageBoxItem(string name) {
		var components = Resources.LoadAll<UIMessageBox> ("");
		for (int i = 0; i < components.Length; i++) {
			if (components[i].name == name) {
				return components[i];
			}
		}
		return null;
	}

}
