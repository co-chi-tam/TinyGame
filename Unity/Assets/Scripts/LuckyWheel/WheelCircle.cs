using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class WheelCircle : MonoBehaviour {

	[SerializeField]	private LuckyWheel m_LuckyWheel;
	[SerializeField]	private float[] m_Gilfs;
	[SerializeField]	private Animator m_Object;
	[SerializeField]	private Text m_GoldText;

	private bool m_OnCheckOut = false;
	private float m_Gold = 0f;

	void Start () {
		m_Gilfs = new float[]{ 1.25f, 0f, 1f, 0f, 3f, 0f, 1.25f, 0f, 1.25f, 0.5f, 1f, 0f, 4f, 0f, 1.25f, 0.5f };
		m_Gold = PlayerPrefs.GetFloat ("PLAYER_GOLD", m_Gold);
		m_GoldText.text = m_Gold.ToString ();
	}
	
	void Update () {
		if (Input.GetKeyUp (KeyCode.A)) {
			WheelRotate ();
		}
	}

	private void OnDestroy() {
		SaveGold ();
	}

	private void OnApplicationQuit() {
		SaveGold ();
	}

	public void WheelRotate() {
		var random = UnityEngine.Random.Range (1, 999);
		StartCoroutine (m_LuckyWheel.HandleRotation (random, () => {

		}, () => {
			Debug.Log(m_Gilfs[m_LuckyWheel.GetWheelTarget()].ToString());
			m_Object.Play("Scale", -1, 0f);
			m_Gold += m_Gilfs[m_LuckyWheel.GetWheelTarget()];
			m_GoldText.text = m_Gold.ToString();
		}));
	}

	public void ChangeScene(string scene) {
		SceneManager.LoadSceneAsync (scene);
	}

	private void SaveGold() {
		PlayerPrefs.SetFloat ("PLAYER_GOLD", m_Gold);
	}

}
