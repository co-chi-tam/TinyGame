using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UIMessageBox : CBaseMonobehaviour {

	public virtual string title {
		get { return string.Empty; }
		set { }
	}

	public virtual string content {
		get { return string.Empty; }
		set { }
	}

	protected Action OnPositiveClick;
	protected Action OnNegativeClick;

	public virtual void AddPositiveListener(Action listener) {
		this.OnPositiveClick -= listener;
		this.OnPositiveClick += listener;
	}

	public virtual void AddNegativeListener(Action listener) {
		this.OnNegativeClick -= listener;
		this.OnNegativeClick += listener;
	}

	public virtual void Submit() {
		if (OnPositiveClick != null) {
			OnPositiveClick ();
		}
	}

	public virtual void Close() {
		if (OnNegativeClick != null) {
			OnNegativeClick ();
		}
	}

}
