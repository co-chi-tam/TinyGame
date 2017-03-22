using System;
using System.Collections;

public class EventListener : IEventListener {

	#region Properties

	public Action OnListener;

	#endregion

	#region Contructor

	public EventListener ()
	{
		OnListener = null;
	}

	#endregion

	#region IEventListener implementation

	public void AddListener (Action listener)
	{
		OnListener -= listener;
		OnListener += listener;
	}

	public void RemoveListener (Action listener)
	{
		OnListener -= listener;
	}

	public void RemoveAllListener ()
	{
		OnListener = null;
	}

	public Action GetListener() {
		return OnListener;
	}

	public void CallBack() {
		if (OnListener != null) {
			OnListener ();
		}
	}

	#endregion



}
