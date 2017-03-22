using UnityEngine;
using System;
using System.Collections;

public class CLuckyWheel : MonoBehaviour {

	[SerializeField]	private LuckyWheel m_LuckyWheel;

	public Action OnStartRotation;
	public Action OnEndRotation;

	public void WheelRotate() {
		var random = UnityEngine.Random.Range (1, 999);
		StartCoroutine (m_LuckyWheel.HandleRotation (random, () => {
			if (OnStartRotation != null) {
				OnStartRotation();
			}
		}, () => {
			if (OnEndRotation != null) {
				OnEndRotation();
			}
		}));
	}
}
