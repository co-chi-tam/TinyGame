using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CSceneManager : MonoBehaviour {

	void Start () {
		var sceneName = SceneManager.GetActiveScene ().name;
		switch (sceneName) {
		case "Test7Lobby":
			CTest7LobbyManager.GetInstance ();
			break;
		default:
			break;
		}
	}

}
