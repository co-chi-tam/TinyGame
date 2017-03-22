using UnityEngine;
using System.Collections;

public class FixSizeWithCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ResizeSpriteToScreen ();
	}
	
	private void ResizeSpriteToScreen() {
		var sr = this.GetComponent<SpriteRenderer>();
		if (sr == null) return;

		this.transform.localScale = Vector3.one;

		var width = sr.sprite.bounds.size.x;
		var height = sr.sprite.bounds.size.y;

		var worldScreenHeight = Camera.main.orthographicSize * 2.0f;
		var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

		var newScale = Vector3.one;
		newScale.x = worldScreenWidth / width;
		newScale.y = worldScreenHeight / height;
		this.transform.localScale = newScale;
		var newPosition = Camera.main.transform.position;
		newPosition.z = 0f;
		this.transform.position = newPosition;
	}
}
