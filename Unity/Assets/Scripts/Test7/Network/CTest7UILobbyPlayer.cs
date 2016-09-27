using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CTest7UILobbyPlayer : MonoBehaviour {

	[SerializeField]	private GameObject m_IsHost;
	[SerializeField]	private GameObject m_IsReady;
	[SerializeField]	private GameObject[] m_AvatarImages;
	[SerializeField]	private Text m_PlayerName;
	[SerializeField]	private Image m_BGImage;
	[SerializeField]	private Color[] m_Colors;

	private void UpdateAvatars(bool value) {
		for (int i = 0; i < m_AvatarImages.Length; i++) {
			m_AvatarImages [i].SetActive (value);
		}
	}

	public void SetAvatarImage(int index) {
		UpdateAvatars (false);
		m_AvatarImages [index].SetActive (true);
	}

	public void SetIsHost(bool value) {
		m_IsHost.SetActive (value);
	}

	public void SetPlayerName (string value) {
		m_PlayerName.text = value;
	}

	public void SetBGImage(bool isLocalPlayer) {
		m_BGImage.color = isLocalPlayer == true ? m_Colors[0] : m_Colors[1];
	}

	public void SetReady(bool value) {
		m_IsReady.SetActive (value);
	}

}
