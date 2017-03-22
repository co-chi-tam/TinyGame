using System;
using System.Collections;
using System.Collections.Generic;

public class CTest9Queue<T> where T : class {

	private LinkedList<T> m_Queue;

	public T this[int index] {
		get { 
			var count = m_Queue.Count;
			var node = m_Queue.GetEnumerator ();
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
		get { return m_Queue.Count; }
	}

	public CTest9Queue ()
	{
		m_Queue = new LinkedList<T> ();
	}

	public void Clear() {
		m_Queue.Clear ();
	}

	public T Pop(int index) {
		var tmp = this [index];
		if (m_Queue.Remove (tmp)) {
			Enqueue (tmp);
		}
		return tmp;
	}

	public T Peek() {
		if (m_Queue.Last != null) {
			return m_Queue.Last.Value;
		} 
		return default(T);
	}

	public void Enqueue(T value) {
		m_Queue.AddFirst (value);
	}

	public T Dequeue(bool continueQueue = true) {
		if (m_Queue.Last == null) {
			return null;
		}
		var lastQueue = m_Queue.Last.Value;
		m_Queue.RemoveLast ();
		if (lastQueue != null && continueQueue == true) {
			Enqueue (lastQueue);
		}
		return lastQueue;
	}

	public bool ContainQueue(T value) {
		return m_Queue.Contains (value);
	}

	public bool RemoveQueue(T value) {
		return m_Queue.Remove (value);
	}

}
