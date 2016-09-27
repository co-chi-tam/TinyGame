using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class CPlayerQueue {

	private LinkedList<CPlayer> m_Players = new LinkedList<CPlayer> ();

	public CPlayer this[int index] {
		get { 
			var count = m_Players.Count;
			var node = m_Players.GetEnumerator ();
			var i = 0;
			while (node.MoveNext ()) {
				if (i == count - index - 1) {
					return node.Current;
				}
				i++;
			}
			return null;
		}
	}

	public int Count {
		get { return m_Players.Count; }
	}

	public CPlayerQueue ()
	{
		
	}

	public void Clear() {
		m_Players.Clear ();
	}

	public void Enqueue(CPlayer value) {
		m_Players.AddFirst (value);
	}

	public CPlayer Dequeue() {
		if (m_Players.Last == null) {
			return null;
		}
		var player = m_Players.Last.Value;
		m_Players.RemoveLast ();
		if (player != null) {
			m_Players.AddFirst (player);
		}
		return player;
	}

	public bool ContainQueue(CPlayer value) {
		return m_Players.Contains (value);
	}

	public bool RemoveQueue(CPlayer value) {
		return m_Players.Remove (value);
	}

}
