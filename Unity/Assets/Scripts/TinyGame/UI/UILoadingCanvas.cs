using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UILoadingCanvas : MonoBehaviour {

	[SerializeField]	private Image m_LoadingImage;

	private bool m_Manual = false;
	private float m_StartTime = 0f;
	private static UILoadingCanvas m_Instance;

	public static UILoadingCanvas Instance {
		get { 
			if (m_Instance == null) {
				var resourceLoads = Resources.LoadAll<UILoadingCanvas> ("");
				GameObject go = null;
				if (resourceLoads.Length == 0) {
					go = new GameObject ();
					m_Instance = go.AddComponent<UILoadingCanvas> ();
				} else {
					go = Instantiate (resourceLoads [0].gameObject);
					m_Instance = go.GetComponent<UILoadingCanvas> ();
				}
				go.SetActive (true);
				go.name = "LoadingCanvas";
			}
			return m_Instance;
		}
	}

	public static UILoadingCanvas GetInstance() {
		return Instance;
	}

	void Awake ()
	{
		FindImageLoading ();
	}

	void Update ()
	{
		if (m_Manual == false) {
			m_StartTime += Time.deltaTime;
			if (m_LoadingImage != null) {
				m_LoadingImage.fillAmount = m_StartTime % 2;
			} else {
				FindImageLoading ();
			}
		}
	}

	private void FindImageLoading() {
		if (m_LoadingImage != null) {
			// TODO
		} else {
			var imageObject = this.transform.FindChild ("LoadingImage");
			if (imageObject != null) {
				m_LoadingImage = imageObject.GetComponent<Image> ();
			}
		}
	}

	public void OnStartLoading(bool manual = false) {
		if (m_LoadingImage != null) {
			m_LoadingImage.gameObject.SetActive (true);
		} else {
			FindImageLoading ();
		}
		m_Manual = manual;
		m_StartTime = 0f;
	}

	public void OnProcessLoading(float processing) {
		m_Manual = false;
		m_LoadingImage.fillAmount = processing;
	}

	public void OnStopLoading() {
		DestroyImmediate (this.gameObject);
		m_Instance = null;
	}

}

public class Loading {

	public static void OnStart() {
		UILoadingCanvas.Instance.OnStartLoading(); 
	}
	public static void OnProcessing(float value) {
		UILoadingCanvas.Instance.OnProcessLoading(value); 
	}
	public static void OnStop() {
		UILoadingCanvas.Instance.OnStopLoading(); 
	}

}
