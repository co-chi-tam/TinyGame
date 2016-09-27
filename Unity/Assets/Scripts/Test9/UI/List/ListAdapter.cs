using System;
using System.Collections;
using System.Collections.Generic;

public class ListAdapter<T> where T : CBaseData, new() {

	private List<T> m_ObjectArray;

	public T this[int index] {
		get { 
			if (m_ObjectArray != null) {
				return m_ObjectArray [index];
			} 
			return null;
		}
	}

	public int length {
		get { 
			return m_ObjectArray.Count;
		}
	}

	public Action OnValueChanged;
	public Action OnClear;

	public ListAdapter (int length, List<T> objects)
	{
		m_ObjectArray = new List<T> ();
		for (int i = 0; i < length; i++) {
			if (objects [i] != default(T)) {
				var obj = new T ();
				obj.CloneData (objects [i]);
				m_ObjectArray.Add (obj);
			} else {
				continue;
			}
		}
	}

	public virtual void Clear() {
		m_ObjectArray.Clear ();
		OnValueChanged = null;
		if (OnClear != null) {
			OnClear ();
		}
	}

	public virtual void AddValue(T value) {
		var obj = new T ();
		obj.CloneData (value);
		m_ObjectArray.Add (obj);
		if (OnValueChanged != null) {
			OnValueChanged ();
		}
	}

	public virtual void SetValue(int index, T value) {
		if (index >= m_ObjectArray.Count) {
			return;
		}
		var obj = new T ();
		obj.CloneData (m_ObjectArray [index]);
		m_ObjectArray [index] = obj;
		if (OnValueChanged != null) {
			OnValueChanged ();
		}
	}

	public virtual void RemoveValue(int index) {
		if (index >= m_ObjectArray.Count) {
			return;
		}
		m_ObjectArray.RemoveAt (index);
		if (OnValueChanged != null) {
			OnValueChanged ();
		}
	}

	public virtual void RemoveValue(T value) {
		if (m_ObjectArray.Contains(value) == false) {
			return;
		}
		m_ObjectArray.Remove (value);
		if (OnValueChanged != null) {
			OnValueChanged ();
		}
	}

	public virtual bool ContainValue(T value) {
		return m_ObjectArray.Contains (value);
	}

}
