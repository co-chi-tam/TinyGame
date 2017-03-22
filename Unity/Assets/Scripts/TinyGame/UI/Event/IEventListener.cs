using System;
using System.Collections;

public interface IEventListener {

	void AddListener(Action listener);
	void RemoveListener(Action listener);
	void RemoveAllListener();
	Action GetListener();
	void CallBack ();

}
