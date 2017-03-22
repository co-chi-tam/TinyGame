using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class CTest9DataParser<K, V> where V : CBaseData, new() {

	#region Properties

	private string m_Url;

	private Dictionary<K, V> m_Data;

	private float m_TimeOut = 10f;

	public Action<CTest9DataParser<K, V>> OnClientComplete;
	public Action<ErrorCode> OnClientError;

	#endregion

	#region Contructor

	public CTest9DataParser ()
	{
		this.m_Url = string.Empty;
		m_Data = new Dictionary<K, V> ();
	}

	public CTest9DataParser (string url)
	{
		this.m_Url = url;
		m_Data = new Dictionary<K, V> ();
	}

	#endregion

	#region Internal class

	public struct ErrorCode
	{
		public int errorCode;
		public string errorContent;

		public ErrorCode (int code, string content)
		{
			this.errorCode = code;
			this.errorContent = content;
		}
	}

	#endregion

	#region Get Method

	public void ParseURL(string url, Action complete = null, Action<ErrorCode> error = null) {
		this.m_Url = url;
		ParseURL (complete, error);
	}

	private void ParseURL (Action complete = null, Action<ErrorCode> error = null) {
		CHandleEvent.Instance.AddEvent (HandleGetMethod(complete, error), null);
	}

	private IEnumerator HandleGetMethod(Action complete = null, Action<ErrorCode> error = null) {
		var www = new WWW (m_Url, null, CTest9ServerInfo.getHeaderValues());
		var timeOut = m_TimeOut;
		while (www.isDone == false && timeOut > 0f) {
			timeOut -= Time.fixedDeltaTime;
			yield return WaitHelper.WaitFixedUpdate;
		}
		if (www.isDone == false) {
			if (error != null) {
				error (new ErrorCode (1, "Request time out."));
			}
			www.Dispose ();
			yield break;
		}
		yield return www;
		if (string.IsNullOrEmpty (www.error) == false) {
			if (error != null) {
				error (new ErrorCode (1, www.error));
			}
		} else {
			ParseJSON (www.text, complete, error);
		}
		www.Dispose ();
	}

	#endregion

	#region Post Method

	public void PostURL(string url, Dictionary<string, string> paramValues) {
		this.m_Url = url;
		CHandleEvent.Instance.DoEventImmediately (HandlePostMethod (paramValues, null, null));
	}

	public void ParseURL(string url, Dictionary<string, string> paramValues, Action complete = null, Action<ErrorCode> error = null) {
		this.m_Url = url;
		ParseURL (paramValues, complete, error);
	}

	private void ParseURL (Dictionary<string, string> paramValues, Action complete = null, Action<ErrorCode> error = null) {
		CHandleEvent.Instance.AddEvent (HandlePostMethod(paramValues, complete, error), null);
	}

	private IEnumerator HandlePostMethod(Dictionary<string, string> paramValues, Action complete = null, Action<ErrorCode> error = null) {
		var formData = new WWWForm ();
		foreach (var item in paramValues) {
			formData.AddField (item.Key, item.Value);
		}
		var www = new WWW (m_Url, formData.data, CTest9ServerInfo.getHeaderValues());
		var timeOut = m_TimeOut;
		while (www.isDone == false && timeOut > 0f) {
			timeOut -= Time.fixedDeltaTime;
			yield return WaitHelper.WaitFixedUpdate;
		}
		if (www.isDone == false) {
			if (error != null) {
				error (new ErrorCode (1, "Request time out."));
			}
			www.Dispose ();
			yield break;
		}
		yield return www;
		if (string.IsNullOrEmpty (www.error) == false) {
			if (error != null) {
				error (new ErrorCode (1, www.error));
			}
		} else {
			ParseJSON (www.text, complete, error);
		}
		www.Dispose ();
	}

	#endregion

	#region Main method

	public void ParseJSON(string json, Action complete = null, Action<ErrorCode> error = null) {
		var instance = Json.Deserialize (json) as Dictionary<string, object>;
		if (instance != null) {
			if (instance.ContainsKey ("errorCode")) {
				if (error != null) {
					var errorCode = new ErrorCode (int.Parse (instance ["errorCode"].ToString ()), instance ["errorContent"].ToString ());
					error (errorCode);
					if (OnClientError != null) {
						OnClientError (errorCode);
					}
				}
			} else {
				if (instance.ContainsKey ("resultContent")) {
					var results = instance ["resultContent"] as List<object>;
					ParseJSON (results, complete, error);
				}
			}
		} else {
			if (error != null) {
				var errorCode = new ErrorCode (1, "ERROR PARSE JSON.");
				error (errorCode);
				if (OnClientError != null) {
					OnClientError (errorCode);
				}
			}
		}
	}

	private void ParseJSON(List<object> instance, Action complete = null, Action<ErrorCode> error = null) {
		m_Data.Clear ();
		var valueAlready = true;
		try {
			for (int i = 0; i < instance.Count; i++) {
				var item = instance [i] as Dictionary<string, object>;
				var parserItem = new V ();
				parserItem.ParseData (item);
				var gameType = (K)Convert.ChangeType(parserItem.id, typeof(K));
				m_Data.Add (gameType, parserItem);
			}
		} catch (Exception ex) {
			if (error != null) {
				var errorCode = new ErrorCode (1, ex.Message);
				error (errorCode);
				if (OnClientError != null) {
					OnClientError (errorCode);
				}
			}
			valueAlready = false;
		}
		if (valueAlready) {
			if (complete != null) {
				complete ();
			}
			if (OnClientComplete != null) {
				OnClientComplete(this);
			}
		}
	}

	#endregion

	#region Getter && Setter

	public V GetData (K key) {
		var parserItem = new V ();
		if (m_Data.ContainsKey (key)) {
			var item = m_Data [key];
			parserItem.CloneData (item);
			return parserItem;
		}
		return default(V);
	}

	public V[] GetValues() {
		var result = new V[m_Data.Values.Count];
		var i = 0;
		foreach (var item in m_Data) {
			result [i] = item.Value;
			i++;
		}
		return result;
	}

	public int Count() {
		return m_Data.Count;
	}

	#endregion
}
