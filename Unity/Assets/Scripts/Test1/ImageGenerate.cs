using UnityEngine;
using System;
using System.Collections;
using System.IO;

public class ImageGenerate : MonoBehaviour {

	[SerializeField]	private Texture2D m_ImageContent;
	[SerializeField]	private int m_Column;
	[SerializeField]	private int m_Row;
	[SerializeField]	private Texture2D m_Top;
	[SerializeField]	private Texture2D m_Bottom;
	[SerializeField]	private Texture2D m_Left;
	[SerializeField]	private Texture2D m_Right;

	[SerializeField]	private Texture2D[] m_Results;

	private int m_ImageWidth;
	private int m_ImageHeight;

	void Start () {
		m_ImageWidth = m_ImageContent.width / m_Column;
		m_ImageHeight = m_ImageContent.height / m_Row;
		m_Results = new Texture2D[m_Column * m_Row];
		GenerateImages ((prc) => {
			Debug.Log("Processing " + prc);
		}, () => {
			Debug.Log("Complete " + Time.time);
		});
	}
	
	private void GenerateImages(Action<float> processing = null, Action complete = null) {
		for (int i = 0; i < m_Column; i++) {
			var x = m_ImageWidth * i;
			for (int j = 0; j < m_Row; j++) {
				var y = m_ImageHeight * j;
				var pixelImage = m_ImageContent.GetPixels (x, y, m_ImageWidth, m_ImageHeight);
				StartCoroutine (HandleGenerateImage (i, j, pixelImage, processing, complete));
			}
		}
	}

	private IEnumerator HandleGenerateImage(int i, int j, Color[] imageData, Action<float> processing = null, Action complete = null) {
		var newTexture = new Texture2D (m_ImageWidth, m_ImageHeight, TextureFormat.ARGB32, false);
		var index = (i * m_Column) + j;
		newTexture.SetPixels (imageData);
		newTexture.Apply ();
		m_Results [index] = newTexture;
		var path = Environment.CurrentDirectory + "/Assets/Resources/Pic_" + index + ".png";
		File.WriteAllBytes (path, newTexture.EncodeToPNG());
		yield return File.Exists (path);
		if (processing != null) {
			processing ((float)index / m_Results.Length);
		}
		if (complete != null) {
			if (index == m_Results.Length - 1) {
				complete ();
			}
		}
	}

}
