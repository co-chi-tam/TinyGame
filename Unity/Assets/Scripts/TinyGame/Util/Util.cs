using UnityEngine;
using System.Collections;

public class Util {

	private static Sprite[] m_ResourceSprites;

	public static Sprite FindSprite(string name) {
		if (m_ResourceSprites == null) {
			var resourceLoad = Resources.LoadAll<Sprite> ("");
			m_ResourceSprites = new Sprite[resourceLoad.Length];
			for (int i = 0; i < m_ResourceSprites.Length; i++) {
				m_ResourceSprites [i] = resourceLoad [i];
			}
		} 
		for (int i = 0; i < m_ResourceSprites.Length; i++) {
			if (m_ResourceSprites [i].name == name) {
				return m_ResourceSprites [i];
			}
		}
		return null;
	}

}
