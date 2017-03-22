using UnityEngine;
using System;
using System.Collections;

public class UIMessageCanvas : MonoBehaviour {

	#region Singleton
	private static UIMessageCanvas m_Instance;

	public static UIMessageCanvas Instance {
		get { 
			if (m_Instance == null) {
				var resourceLoads = Resources.LoadAll<UIMessageCanvas> ("");
				GameObject go = null;
				if (resourceLoads.Length == 0) {
					go = new GameObject ();
					m_Instance = go.AddComponent<UIMessageCanvas> ();
				} else {
					go = Instantiate (resourceLoads [0].gameObject);
					m_Instance = go.GetComponent<UIMessageCanvas> ();
				}
				go.SetActive (true);
				go.name = "MessageBoxCanvas";
			}
			return m_Instance;
		}
	}

	public static UIMessageCanvas GetInstance() {
		return Instance;
	}

	#endregion

	#region Properties

	[SerializeField]	public GameObject root;
	[SerializeField]	private UIMessageBoxSimple m_MessageOKPanel;
	[SerializeField]	private UIMessageBoxSimple m_MessageOKCancelPanel;

	#endregion

	#region Monobehaviour

	void Awake ()
	{
		m_MessageOKPanel.gameObject.SetActive (false);
		m_MessageOKCancelPanel.gameObject.SetActive (false);
		// Message OK
		m_MessageOKPanel.AddPositiveListener (DestroyAllMessage);
		m_MessageOKPanel.AddNegativeListener(DestroyAllMessage);
		// Message OK Cancel
		m_MessageOKCancelPanel.AddPositiveListener (DestroyAllMessage);
		m_MessageOKCancelPanel.AddNegativeListener (DestroyAllMessage);
	}

	#endregion

	#region Main methods

	public void DestroyAllMessage() {
		m_Instance = null;
		DestroyImmediate (this.gameObject);
	}

	public void ShowMessage(string title, string content, Action onPositiveClick) {
		m_MessageOKPanel.gameObject.SetActive (true);
		m_MessageOKCancelPanel.gameObject.SetActive (false);
		m_MessageOKPanel.title = title;
		m_MessageOKPanel.content = content;
		m_MessageOKPanel.AddPositiveListener (onPositiveClick);
	}

	public void ShowMessage(string title, string content, Action onPositiveClick, Action onNegativeClick) {
		m_MessageOKPanel.gameObject.SetActive (true);
		m_MessageOKCancelPanel.gameObject.SetActive (true);
		m_MessageOKCancelPanel.title = title;
		m_MessageOKCancelPanel.content = content;
		m_MessageOKCancelPanel.AddPositiveListener (onPositiveClick);
		m_MessageOKCancelPanel.AddNegativeListener (onNegativeClick);
	}

	#endregion

}

public class MessageBox {

	public static void Show(CMessageItem item) {
		var uiMessageCanvas = UIMessageCanvas.GetInstance ();
		item.AddPositiveListener(uiMessageCanvas.DestroyAllMessage);
		item.AddNegativeListener(uiMessageCanvas.DestroyAllMessage);
		item.Repair (uiMessageCanvas.root);
	}

	public static void Show(string title, string content, Action onPositiveClick) {
		UIMessageCanvas.Instance.ShowMessage (title, content, onPositiveClick);
	}

	public static void Show(string title, string content, Action onPositiveClick, Action onNegativeClick) {
		UIMessageCanvas.Instance.ShowMessage (title, content, onPositiveClick, onNegativeClick);
	}

}

